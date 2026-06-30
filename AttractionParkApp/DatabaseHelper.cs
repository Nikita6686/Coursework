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

                NormalizeDatabase(connection);
                SeedData(connection);
            }
        }

        public static bool TryAuthenticate(string login, string password, out int userId, out string fullName, out string role)
        {
            userId = 0;
            fullName = null;
            role = null;

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT ID_User, FullName, Role FROM Users WHERE Login = ? AND [Password] = ?";
                AddParameter(command, OleDbType.VarWChar, login.Trim());
                AddParameter(command, OleDbType.VarWChar, password);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;

                    userId = Convert.ToInt32(reader[0]);
                    fullName = reader.IsDBNull(1) ? login.Trim() : reader.GetString(1);
                    role = UserRoles.Normalize(reader.IsDBNull(2) ? string.Empty : reader.GetString(2));
                    return true;
                }
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
            if (string.Equals(login.Trim(), "admin", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Этот логин зарезервирован для администратора.");

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Users (FullName, Login, [Password], Role) VALUES (?, ?, ?, ?)";
                AddParameter(command, OleDbType.VarWChar, fullName.Trim());
                AddParameter(command, OleDbType.VarWChar, login.Trim());
                AddParameter(command, OleDbType.VarWChar, password);
                AddParameter(command, OleDbType.VarWChar, UserRoles.Visitor);
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
            // Исправленный SQL-запрос с экранированием и приведением типов
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


        public static void BuyTicket(int userId, int attractionId, decimal price)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
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

                    var dbPrice = reader.IsDBNull(1) ? 0m : Convert.ToDecimal(reader[1]);
                    if (dbPrice <= 0)
                        throw new InvalidOperationException("Для аттракциона не задана цена билета.");

                    price = dbPrice;
                }
            }

            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Tickets (UserID, AttractionID, PurchaseDate, Price) VALUES (?, ?, ?, ?)";
                AddParameter(command, OleDbType.Integer, userId);
                AddParameter(command, OleDbType.Integer, attractionId);
                AddParameter(command, OleDbType.Date, DateTime.Now);
                AddParameter(command, OleDbType.Currency, price);
                command.ExecuteNonQuery();
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

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Admin + "' " +
                "WHERE Role IN ('Admin', 'Админ', 'admin', 'ADMIN')");

            ExecuteNonQuery(connection,
                "UPDATE Users SET Role = '" + UserRoles.Visitor + "' " +
                "WHERE Role IN ('User', 'Посетитель', 'user', 'USER')");

            ExecuteNonQuery(connection,
                "UPDATE Attractions SET TicketPrice = 500 " +
                "WHERE TicketPrice IS NULL OR TicketPrice = 0");
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
            EnsureUser(connection, "Администратор системы", "admin", "1234", UserRoles.Admin);
            EnsureUser(connection, "Иван Иванов", "user", "user", UserRoles.Visitor);

            if (GetRecordCount(connection, "Attractions") == 0)
            {
                var today = "#" + DateTime.Today.ToString("MM/dd/yyyy") + "#";
                ExecuteNonQuery(connection,
                    "INSERT INTO Attractions (Name, Zone, CurrentStatus, LastServiceDate, TicketPrice) " +
                    "VALUES ('Колесо обозрения', 'Центр', '" + AttractionStatus.Working + "', " + today + ", 500)");
                ExecuteNonQuery(connection,
                    "INSERT INTO Attractions (Name, Zone, CurrentStatus, LastServiceDate, TicketPrice) " +
                    "VALUES ('Американские горки', 'Экстрим', '" + AttractionStatus.Working + "', " + today + ", 800)");
                ExecuteNonQuery(connection,
                    "INSERT INTO Attractions (Name, Zone, CurrentStatus, LastServiceDate, TicketPrice) " +
                    "VALUES ('Карусель', 'Детская зона', '" + AttractionStatus.Maintenance + "', " + today + ", 300)");
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
                    "INSERT INTO Users (FullName, Login, [Password], Role) VALUES (?, ?, ?, ?)";
                AddParameter(command, OleDbType.VarWChar, fullName);
                AddParameter(command, OleDbType.VarWChar, login);
                AddParameter(command, OleDbType.VarWChar, password);
                AddParameter(command, OleDbType.VarWChar, role);
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
