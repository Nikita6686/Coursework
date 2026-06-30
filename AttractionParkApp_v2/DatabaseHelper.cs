using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using AttractionParkApp.Properties;

namespace AttractionParkApp
{
    public static class DatabaseHelper
    {
        public static string ConnectionString => Settings.Default.БДConnectionString;

        public static string GetConnectionErrorMessage(Exception ex)
        {
            var message = ex.Message;
            if (message.IndexOf("ACE.OLEDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("не зарегистрирован", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Не удалось подключиться к базе данных Access.\n\n" +
                       "Проверьте, что в свойствах проекта выбрана платформа x86, " +
                       "затем пересоберите и запустите проект снова.";
            }

            return "Не удалось подключиться к базе данных.\n\n" + message;
        }

        public static void EnsureDatabase()
        {
            using (var connection = OpenConnection())
            {
                if (!TableExists(connection, "Tickets"))
                {
                    ExecuteNonQuery(connection,
                        "CREATE TABLE Tickets (" +
                        "ID_Ticket AUTOINCREMENT PRIMARY KEY, " +
                        "UserID INTEGER, " +
                        "AttractionID INTEGER, " +
                        "PurchaseDate DATETIME, " +
                        "Price CURRENCY)");
                }

                if (!ColumnExists(connection, "Attractions", "TicketPrice"))
                {
                    ExecuteNonQuery(connection, "ALTER TABLE Attractions ADD COLUMN TicketPrice CURRENCY");
                }

                if (!ColumnExists(connection, "Users", "Balance"))
                {
                    ExecuteNonQuery(connection, "ALTER TABLE Users ADD COLUMN Balance CURRENCY");
                }

                NormalizeDatabase(connection);
                SeedData(connection);
                RepairStaffAccounts(connection);
            }
        }

        public static bool TryAuthenticate(string login, string password, out int userId, out string fullName, out string role)
        {
            userId = 0;
            fullName = null;
            role = null;

            login = login?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(login))
                return false;

            using (var connection = OpenConnection())
            {
                var userRow = FindUserRow(connection, login);
                if (userRow == null)
                    return false;

                var dbPassword = userRow["Password"] == DBNull.Value ? string.Empty : userRow["Password"].ToString();
                if (!string.Equals(dbPassword, password, StringComparison.Ordinal) &&
                    !string.Equals(dbPassword.Trim(), password, StringComparison.Ordinal))
                    return false;

                userId = Convert.ToInt32(userRow["ID_User"]);
                fullName = userRow["FullName"] == DBNull.Value ? login : userRow["FullName"].ToString();
                role = UserRoles.Normalize(userRow["Role"] == DBNull.Value ? string.Empty : userRow["Role"].ToString());
                return true;
            }
        }

        public static bool LoginExists(string login)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = ?";
                AddParameter(command, OleDbType.VarWChar, login.Trim());
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public static void RegisterUser(string fullName, string login, string password)
        {
            if (IsReservedStaffLogin(login))
                throw new InvalidOperationException("Этот логин зарезервирован для служебной учётной записи.");

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Users (FullName, Login, [Password], Role, Balance) VALUES (?, ?, ?, ?, ?)";
                AddParameter(command, OleDbType.VarWChar, fullName.Trim());
                AddParameter(command, OleDbType.VarWChar, login.Trim());
                AddParameter(command, OleDbType.VarWChar, password);
                AddParameter(command, OleDbType.VarWChar, UserRoles.Visitor);
                AddParameter(command, OleDbType.Currency, AppInfo.RegistrationBonus);
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetAttractionsForAdmin()
        {
            return FillTable(
                "SELECT [ID_Attraction], CStr([Name]) AS [Name], CStr([Zone]) AS [Zone], " +
                "CStr([CurrentStatus]) AS [CurrentStatus], [LastServiceDate], [TicketPrice] " +
                "FROM [Attractions] ORDER BY [Name]");
        }


        public static void SaveAttractions(DataTable table)
        {
            using (var connection = OpenConnection())
            using (var adapter = new OleDbDataAdapter(
                "SELECT ID_Attraction, Name, Zone, CurrentStatus, LastServiceDate, TicketPrice FROM Attractions",
                connection))
            {
                new OleDbCommandBuilder(adapter);
                adapter.Update(table);
            }
        }

        public static DataTable GetAttractionsForVisitors()
        {
            // Исправленный SQL-запрос с экранированием зарезервированных слов и преобразованием типов
            var all = FillTable(
                "SELECT [ID_Attraction], CStr([Name]) AS [Name], CStr([Zone]) AS [Zone], " +
                "CStr([CurrentStatus]) AS [CurrentStatus], [TicketPrice] " +
                "FROM [Attractions] ORDER BY [Name]");

            var available = all.Clone();
            foreach (DataRow row in all.Rows)
            {
                var status = row["CurrentStatus"] == DBNull.Value ? string.Empty : row["CurrentStatus"].ToString();
                if (!AttractionStatus.CanBuyTicket(status))
                    continue;

                if (row["TicketPrice"] == DBNull.Value || Convert.ToDecimal(row["TicketPrice"]) <= 0)
                    continue;

                available.ImportRow(row);
            }

            return available;
        }


        public static void BuyTicket(int userId, int attractionId)
        {
            if (!IsVisitorUser(userId))
                throw new InvalidOperationException("Покупка билетов доступна только посетителям.");

            using (var connection = OpenConnection())
            {
                var transaction = connection.BeginTransaction();
                try
                {
                    decimal dbPrice;
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText =
                            "SELECT CurrentStatus, TicketPrice FROM Attractions WHERE ID_Attraction = ?";
                        AddParameter(command, OleDbType.Integer, attractionId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new InvalidOperationException("Аттракцион не найден.");

                            var status = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            if (!AttractionStatus.CanBuyTicket(status))
                                throw new InvalidOperationException(
                                    "Аттракцион недоступен. Текущий статус: " + AttractionStatus.Normalize(status));

                            dbPrice = reader.IsDBNull(1) ? 0m : Convert.ToDecimal(reader[1]);
                            if (dbPrice <= 0)
                                throw new InvalidOperationException("Для аттракциона не задана цена билета.");
                        }
                    }

                    decimal balance;
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "SELECT Balance FROM Users WHERE ID_User = ?";
                        AddParameter(command, OleDbType.Integer, userId);
                        var result = command.ExecuteScalar();
                        balance = result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
                    }

                    if (balance < dbPrice)
                        throw new InvalidOperationException(
                            "Недостаточно средств на балансе.\n" +
                            "Баланс: " + balance.ToString("0") + " руб., нужно: " + dbPrice.ToString("0") + " руб.");

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "UPDATE Users SET Balance = Balance - ? WHERE ID_User = ?";
                        AddParameter(command, OleDbType.Currency, dbPrice);
                        AddParameter(command, OleDbType.Integer, userId);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText =
                            "INSERT INTO Tickets (UserID, AttractionID, PurchaseDate, Price) VALUES (?, ?, ?, ?)";
                        AddParameter(command, OleDbType.Integer, userId);
                        AddParameter(command, OleDbType.Integer, attractionId);
                        AddParameter(command, OleDbType.Date, DateTime.Now);
                        AddParameter(command, OleDbType.Currency, dbPrice);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static decimal GetUserBalance(int userId)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Balance FROM Users WHERE ID_User = ?";
                AddParameter(command, OleDbType.Integer, userId);
                var result = command.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
            }
        }

        public static void TopUpBalance(int userId, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Сумма пополнения должна быть больше нуля.");

            if (!IsVisitorUser(userId))
                throw new InvalidOperationException("Баланс доступен только для посетителей.");

            var newBalance = GetUserBalance(userId) + amount;
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Users SET Balance = ? WHERE ID_User = ?";
                AddParameter(command, OleDbType.Currency, newBalance);
                AddParameter(command, OleDbType.Integer, userId);
                if (command.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Пользователь не найден.");
            }
        }

        public static DataTable GetUsersForAdmin()
        {
            // Защищаем все поля скобками и принудительно приводим текстовые поля/подстановки
            return FillTable(
                "SELECT [ID_User], CStr([FullName]) AS [FullName], CStr([Login]) AS [Login], " +
                "[Password], CStr([Role]) AS [Role], [Balance] " +
                "FROM [Users] ORDER BY [FullName]");
        }

        public static void SaveUsers(DataTable table)
        {
            using (var connection = OpenConnection())
            using (var adapter = new OleDbDataAdapter(
                "SELECT [ID_User], [FullName], [Login], [Password], [Role], [Balance] FROM [Users]",
                connection))
            {
                // CommandBuilder автоматически создаст корректные Insert/Update/Delete запросы 
                // с учетом защитных квадратных скобок
                new OleDbCommandBuilder(adapter);
                adapter.Update(table);
            }
        }


        public static DataTable GetTicketsForUser(int userId)
        {
            var table = new DataTable();
            using (var connection = OpenConnection())
            using (var adapter = new OleDbDataAdapter(
                "SELECT t.ID_Ticket, a.Name AS AttractionName, t.PurchaseDate, t.Price " +
                "FROM (Tickets AS t LEFT JOIN Attractions AS a ON t.AttractionID = a.ID_Attraction) " +
                "WHERE t.UserID = ? ORDER BY t.PurchaseDate DESC", connection))
            {
                AddParameter(adapter.SelectCommand, OleDbType.Integer, userId);
                adapter.Fill(table);
            }

            return table;
        }

        public static DataTable GetIncidentsForAdmin()
        {
            return FillTable(
                "SELECT i.ID_Incident, a.Name AS AttractionName, o.FullName AS OperatorName, " +
                "t.FullName AS TechnicianName, i.Description, i.Status, i.DateOpen, i.DateClose " +
                "FROM (((Incidents AS i " +
                "LEFT JOIN Attractions AS a ON i.AttractionID = a.ID_Attraction) " +
                "LEFT JOIN Users AS o ON i.OperatorID = o.ID_User) " +
                "LEFT JOIN Users AS t ON i.TechID = t.ID_User) " +
                "ORDER BY i.DateOpen DESC");
        }

        public static DashboardStats GetDashboardStats()
        {
            var stats = new DashboardStats();
            using (var connection = OpenConnection())
            {
                stats.AttractionCount = GetScalarInt(connection, "SELECT COUNT(*) FROM Attractions");
                stats.TicketCount = GetScalarInt(connection, "SELECT COUNT(*) FROM Tickets");
                stats.UserCount = GetScalarInt(connection, "SELECT COUNT(*) FROM Users");
                stats.OpenIncidents = GetScalarInt(connection,
                    "SELECT COUNT(*) FROM Incidents WHERE Status IN ('" + IncidentStatus.New + "', '" +
                    IncidentStatus.InProgress + "')");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT SUM(Price) FROM Tickets";
                    var result = command.ExecuteScalar();
                    stats.TotalRevenue = result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
                }
            }

            return stats;
        }

        private static int GetScalarInt(OleDbConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static DataTable GetTickets()
        {
            return FillTable(
                "SELECT t.ID_Ticket, t.UserID, u.FullName AS UserName, " +
                "t.AttractionID, a.Name AS AttractionName, t.PurchaseDate, t.Price " +
                "FROM ((Tickets AS t " +
                "LEFT JOIN Users AS u ON t.UserID = u.ID_User) " +
                "LEFT JOIN Attractions AS a ON t.AttractionID = a.ID_Attraction) " +
                "ORDER BY t.PurchaseDate DESC");
        }

        public static DataTable GetAttractionsForOperator()
        {
            return FillTable(
                "SELECT [ID_Attraction], CStr([Name]) AS [Name], CStr([Zone]) AS [Zone], " +
                "CStr([CurrentStatus]) AS [CurrentStatus], [LastServiceDate] " +
                "FROM [Attractions] ORDER BY [Name]");
        }


        public static void CreateIncident(int attractionId, int operatorId, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new InvalidOperationException("Укажите описание поломки.");

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT COUNT(*) FROM Incidents WHERE AttractionID = ? AND Status IN (?, ?)";
                AddParameter(command, OleDbType.Integer, attractionId);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.New);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.InProgress);
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    throw new InvalidOperationException("По этому аттракциону уже есть открытая заявка.");
            }

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Incidents (AttractionID, OperatorID, Description, Status, DateOpen) " +
                    "VALUES (?, ?, ?, ?, ?)";
                AddParameter(command, OleDbType.Integer, attractionId);
                AddParameter(command, OleDbType.Integer, operatorId);
                AddParameter(command, OleDbType.VarWChar, description.Trim());
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.New);
                AddParameter(command, OleDbType.Date, DateTime.Now);
                command.ExecuteNonQuery();
            }

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Attractions SET CurrentStatus = ? WHERE ID_Attraction = ?";
                AddParameter(command, OleDbType.VarWChar, AttractionStatus.Maintenance);
                AddParameter(command, OleDbType.Integer, attractionId);
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetIncidentsForTechnician(int technicianId)
        {
            var table = FillTable(
                "SELECT i.ID_Incident, i.AttractionID, a.Name AS AttractionName, " +
                "i.OperatorID, o.FullName AS OperatorName, i.Description, i.Status, i.DateOpen, " +
                "i.TechID, t.FullName AS TechnicianName " +
                "FROM (((Incidents AS i " +
                "LEFT JOIN Attractions AS a ON i.AttractionID = a.ID_Attraction) " +
                "LEFT JOIN Users AS o ON i.OperatorID = o.ID_User) " +
                "LEFT JOIN Users AS t ON i.TechID = t.ID_User) " +
                "WHERE i.Status IN ('" + IncidentStatus.New + "', '" + IncidentStatus.InProgress + "') " +
                "ORDER BY i.DateOpen");

            table.Columns.Add("Assignment", typeof(string));

            foreach (DataRow row in table.Rows)
            {
                row["Status"] = IncidentStatus.Normalize(row["Status"]?.ToString());
                row["Assignment"] = GetIncidentAssignment(row, technicianId);
            }

            return table;
        }

        private static string GetIncidentAssignment(DataRow row, int technicianId)
        {
            var status = IncidentStatus.Normalize(row["Status"]?.ToString());
            if (status == IncidentStatus.New)
                return "Не назначена";

            if (row["TechID"] != DBNull.Value && Convert.ToInt32(row["TechID"]) == technicianId)
                return "Моя заявка";

            var techName = row["TechnicianName"] == DBNull.Value ? null : row["TechnicianName"].ToString();
            return string.IsNullOrWhiteSpace(techName) ? "Другой техник" : "Техник: " + techName;
        }

        public static void TakeIncident(int incidentId, int technicianId)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Incidents SET TechID = ?, Status = ? " +
                    "WHERE ID_Incident = ? AND Status = ?";
                AddParameter(command, OleDbType.Integer, technicianId);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.InProgress);
                AddParameter(command, OleDbType.Integer, incidentId);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.New);
                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException("Заявка уже взята в работу или не найдена.");
            }
        }

        public static void CompleteIncident(int incidentId, int technicianId)
        {
            int attractionId;
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT AttractionID FROM Incidents WHERE ID_Incident = ?";
                AddParameter(command, OleDbType.Integer, incidentId);
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("Заявка не найдена.");
                attractionId = Convert.ToInt32(result);
            }

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Incidents SET TechID = ?, Status = ?, DateClose = ? " +
                    "WHERE ID_Incident = ? AND Status = ? AND TechID = ?";
                AddParameter(command, OleDbType.Integer, technicianId);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.Fixed);
                AddParameter(command, OleDbType.Date, DateTime.Now);
                AddParameter(command, OleDbType.Integer, incidentId);
                AddParameter(command, OleDbType.VarWChar, IncidentStatus.InProgress);
                AddParameter(command, OleDbType.Integer, technicianId);
                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException("Заявку можно завершить только если она взята вами в работу.");
            }

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Attractions SET CurrentStatus = ?, LastServiceDate = ? WHERE ID_Attraction = ?";
                AddParameter(command, OleDbType.VarWChar, AttractionStatus.Working);
                AddParameter(command, OleDbType.Date, DateTime.Today);
                AddParameter(command, OleDbType.Integer, attractionId);
                command.ExecuteNonQuery();
            }
        }

        public static bool HasDuplicateLogins(DataTable users, out string duplicateLogin)
        {
            duplicateLogin = null;
            var logins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in users.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                var login = row["Login"] == DBNull.Value ? string.Empty : row["Login"].ToString().Trim();
                if (string.IsNullOrEmpty(login))
                    continue;

                if (!logins.Add(login))
                {
                    duplicateLogin = login;
                    return true;
                }
            }

            return false;
        }

        private static void NormalizeDatabase(OleDbConnection connection)
        {
            RemoveDuplicateUsers(connection);
            NormalizeAttractionStatuses(connection);
            NormalizeIncidentStatuses(connection);

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Admin + "' " +
                "WHERE Role IN ('Admin', 'Админ', 'admin', 'ADMIN')");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Visitor + "' " +
                "WHERE Role IN ('User', 'Посетитель', 'user', 'USER')");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Operator + "' " +
                "WHERE Role IN ('Operator', 'operator', 'ОПЕРАТОР')");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Technician + "' " +
                "WHERE Role IN ('Tech', 'Technician', 'техник', 'ТЕХНИК')");

            ExecuteNonQuery(connection,
                "UPDATE Attractions SET TicketPrice = 500 " +
                "WHERE TicketPrice IS NULL OR TicketPrice = 0");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Balance = 2000 " +
                "WHERE Balance IS NULL AND Role = '" + UserRoles.Visitor + "'");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Balance = 0 WHERE Role <> '" + UserRoles.Visitor + "'");
        }

        private static void NormalizeAttractionStatuses(OleDbConnection connection)
        {
            var attractions = new DataTable();
            using (var adapter = new OleDbDataAdapter(
                "SELECT ID_Attraction, CurrentStatus FROM Attractions", connection))
            {
                adapter.Fill(attractions);
            }

            foreach (DataRow row in attractions.Rows)
            {
                var id = Convert.ToInt32(row["ID_Attraction"]);
                var current = row["CurrentStatus"] == DBNull.Value ? string.Empty : row["CurrentStatus"].ToString();
                var normalized = AttractionStatus.Normalize(current);

                if (string.Equals(current, normalized, StringComparison.Ordinal))
                    continue;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Attractions SET CurrentStatus = ? WHERE ID_Attraction = ?";
                    AddParameter(command, OleDbType.VarWChar, normalized);
                    AddParameter(command, OleDbType.Integer, id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void RemoveDuplicateUsers(OleDbConnection connection)
        {
            var users = new DataTable();
            using (var adapter = new OleDbDataAdapter(
                "SELECT ID_User, Login FROM Users ORDER BY Login, ID_User", connection))
            {
                adapter.Fill(users);
            }

            var seenLogins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in users.Rows)
            {
                var login = row["Login"] == DBNull.Value ? string.Empty : row["Login"].ToString().Trim();
                var id = Convert.ToInt32(row["ID_User"]);

                if (string.IsNullOrEmpty(login))
                    continue;

                if (!seenLogins.Add(login))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM Users WHERE ID_User = ?";
                        AddParameter(command, OleDbType.Integer, id);
                        command.ExecuteNonQuery();
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "DELETE FROM Users WHERE Role = '" + UserRoles.Admin + "' AND Login <> 'admin'";
                command.ExecuteNonQuery();
            }
        }

        private static void SeedData(OleDbConnection connection)
        {
            EnsureUser(connection, "Иван Иванов", "user", "user", UserRoles.Visitor);
            EnsureDefaultAttractions(connection);
        }

        private static void EnsureDefaultAttractions(OleDbConnection connection)
        {
            EnsureAttraction(connection, "Колесо обозрения", "Центр", AttractionStatus.Working, 450);
            EnsureAttraction(connection, "Американские горки", "Экстрим", AttractionStatus.Working, 850);
            EnsureAttraction(connection, "Карусель", "Детская зона", AttractionStatus.Maintenance, 250);
            EnsureAttraction(connection, "Автодром", "Семейная", AttractionStatus.Working, 350);
            EnsureAttraction(connection, "Батут-центр", "Семейная", AttractionStatus.Working, 400);
            EnsureAttraction(connection, "Лабиринт", "Детская зона", AttractionStatus.Working, 200);
            EnsureAttraction(connection, "Канатная дорога", "Центр", AttractionStatus.Working, 550);
            EnsureAttraction(connection, "Речная прогулка", "Зона отдыха", AttractionStatus.Working, 600);
            EnsureAttraction(connection, "Детская железная дорога", "Детская зона", AttractionStatus.Working, 150);
            EnsureAttraction(connection, "Галактика", "Экстрим", AttractionStatus.Working, 750);
        }

        private static void EnsureAttraction(
            OleDbConnection connection,
            string name,
            string zone,
            string status,
            decimal ticketPrice)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Attractions WHERE Name = ?";
                AddParameter(command, OleDbType.VarWChar, name);
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    command.Parameters.Clear();
                    command.CommandText =
                        "UPDATE Attractions SET TicketPrice = ? WHERE Name = ? AND " +
                        "(TicketPrice IS NULL OR TicketPrice = 0 OR TicketPrice = 500)";
                    AddParameter(command, OleDbType.Currency, ticketPrice);
                    AddParameter(command, OleDbType.VarWChar, name);
                    command.ExecuteNonQuery();
                    return;
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Attractions (Name, [Zone], CurrentStatus, LastServiceDate, TicketPrice) " +
                    "VALUES (?, ?, ?, ?, ?)";
                AddParameter(command, OleDbType.VarWChar, name);
                AddParameter(command, OleDbType.VarWChar, zone);
                AddParameter(command, OleDbType.VarWChar, status);
                AddParameter(command, OleDbType.Date, DateTime.Today);
                AddParameter(command, OleDbType.Currency, ticketPrice);
                command.ExecuteNonQuery();
            }
        }

        private static void RepairStaffAccounts(OleDbConnection connection)
        {
            RemoveStaffLoginDuplicates(connection, "admin");
            RemoveStaffLoginDuplicates(connection, "operator");
            RemoveStaffLoginDuplicates(connection, "tech");

            EnsureStaffAccount(connection, "Администратор системы", "admin", "1234", UserRoles.Admin);
            EnsureStaffAccount(connection, "Пётр Операторов", "operator", "operator", UserRoles.Operator);
            EnsureStaffAccount(connection, "Сергей Техников", "tech", "tech", UserRoles.Technician);
        }

        private static void RemoveStaffLoginDuplicates(OleDbConnection connection, string login)
        {
            var table = new DataTable();
            using (var adapter = new OleDbDataAdapter("SELECT ID_User, Login FROM Users", connection))
            {
                adapter.Fill(table);
            }

            var ids = new List<int>();
            foreach (DataRow row in table.Rows)
            {
                var dbLogin = row["Login"] == DBNull.Value ? string.Empty : row["Login"].ToString().Trim();
                if (string.Equals(dbLogin, login, StringComparison.OrdinalIgnoreCase))
                    ids.Add(Convert.ToInt32(row["ID_User"]));
            }

            if (ids.Count <= 1)
                return;

            ids.Sort();
            for (int i = 1; i < ids.Count; i++)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Users WHERE ID_User = ?";
                    AddParameter(command, OleDbType.Integer, ids[i]);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static DataRow FindUserRow(OleDbConnection connection, string login)
        {
            var table = new DataTable();
            using (var adapter = new OleDbDataAdapter(
                "SELECT ID_User, FullName, Login, [Password], Role FROM Users", connection))
            {
                adapter.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                var dbLogin = row["Login"] == DBNull.Value ? string.Empty : row["Login"].ToString().Trim();
                if (string.Equals(dbLogin, login, StringComparison.OrdinalIgnoreCase))
                    return row;
            }

            return null;
        }

        private static void EnsureStaffAccount(OleDbConnection connection, string fullName, string login, string password, string role)
        {
            var existing = FindUserRow(connection, login);
            if (existing != null)
            {
                var currentRole = UserRoles.Normalize(existing["Role"]?.ToString());
                if (currentRole != role)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "UPDATE Users SET FullName = ?, Role = ?, Balance = 0 WHERE ID_User = ?";
                        AddParameter(command, OleDbType.VarWChar, fullName);
                        AddParameter(command, OleDbType.VarWChar, role);
                        AddParameter(command, OleDbType.Integer, Convert.ToInt32(existing["ID_User"]));
                        command.ExecuteNonQuery();
                    }
                }

                return;
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Users (FullName, Login, [Password], Role, Balance) VALUES (?, ?, ?, ?, 0)";
                AddParameter(command, OleDbType.VarWChar, fullName);
                AddParameter(command, OleDbType.VarWChar, login);
                AddParameter(command, OleDbType.VarWChar, password);
                AddParameter(command, OleDbType.VarWChar, role);
                command.ExecuteNonQuery();
            }
        }

        private static void NormalizeIncidentStatuses(OleDbConnection connection)
        {
            var incidents = new DataTable();
            using (var adapter = new OleDbDataAdapter(
                "SELECT ID_Incident, Status FROM Incidents", connection))
            {
                adapter.Fill(incidents);
            }

            foreach (DataRow row in incidents.Rows)
            {
                var id = Convert.ToInt32(row["ID_Incident"]);
                var current = row["Status"] == DBNull.Value ? string.Empty : row["Status"].ToString();
                var normalized = IncidentStatus.Normalize(current);

                if (string.Equals(current, normalized, StringComparison.Ordinal))
                    continue;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Incidents SET Status = ? WHERE ID_Incident = ?";
                    AddParameter(command, OleDbType.VarWChar, normalized);
                    AddParameter(command, OleDbType.Integer, id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool IsReservedStaffLogin(string login)
        {
            var value = login.Trim();
            return string.Equals(value, "admin", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "operator", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "tech", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsVisitorUser(int userId)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Role FROM Users WHERE ID_User = ?";
                AddParameter(command, OleDbType.Integer, userId);
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return false;
                return UserRoles.IsVisitor(UserRoles.Normalize(result.ToString()));
            }
        }

        private static void EnsureUser(OleDbConnection connection, string fullName, string login, string password, string role)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = ?";
                AddParameter(command, OleDbType.VarWChar, login);
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    return;
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Users (FullName, Login, [Password], Role, Balance) VALUES (?, ?, ?, ?, ?)";
                AddParameter(command, OleDbType.VarWChar, fullName);
                AddParameter(command, OleDbType.VarWChar, login);
                AddParameter(command, OleDbType.VarWChar, password);
                AddParameter(command, OleDbType.VarWChar, role);
                var balance = UserRoles.IsVisitor(role) ? AppInfo.DefaultVisitorBalance : 0m;
                AddParameter(command, OleDbType.Currency, balance);
                command.ExecuteNonQuery();
            }
        }

        private static void AddParameter(OleDbCommand command, OleDbType type, object value)
        {
            var parameter = command.Parameters.Add(null, type);
            parameter.Value = value ?? DBNull.Value;
        }

        private static DataTable FillTable(string sql)
        {
            var table = new DataTable();
            using (var connection = OpenConnection())
            using (var adapter = new OleDbDataAdapter(sql, connection))
            {
                adapter.Fill(table);
            }

            return table;
        }

        private static OleDbConnection OpenConnection()
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        private static void ExecuteNonQuery(OleDbConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        private static int GetRecordCount(OleDbConnection connection, string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM [" + tableName + "]";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static bool TableExists(OleDbConnection connection, string tableName)
        {
            var restrictions = new string[] { null, null, tableName, "TABLE" };
            var schema = connection.GetSchema("Tables", restrictions);
            return schema.Rows.Count > 0;
        }

        private static bool ColumnExists(OleDbConnection connection, string tableName, string columnName)
        {
            var restrictions = new string[] { null, null, tableName, columnName };
            var schema = connection.GetSchema("Columns", restrictions);
            return schema.Rows.Count > 0;
        }
    }
}
