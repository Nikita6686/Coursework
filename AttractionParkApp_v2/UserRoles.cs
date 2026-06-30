namespace AttractionParkApp
{
    public static class UserRoles
    {
        public const string Admin = "Администратор";
        public const string Visitor = "Пользователь";
        public const string Operator = "Оператор";
        public const string Technician = "Техник";

        public static bool IsAdmin(string role) => Normalize(role) == Admin;
        public static bool IsVisitor(string role) => Normalize(role) == Visitor;
        public static bool IsOperator(string role) => Normalize(role) == Operator;
        public static bool IsTechnician(string role) => Normalize(role) == Technician;

        public static string Normalize(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Visitor;

            switch (role.Trim().ToLowerInvariant())
            {
                case "admin":
                case "админ":
                case "администратор":
                    return Admin;
                case "user":
                case "пользователь":
                case "посетитель":
                    return Visitor;
                case "operator":
                case "оператор":
                    return Operator;
                case "tech":
                case "technician":
                case "техник":
                    return Technician;
                default:
                    return role.Trim();
            }
        }
    }
}
