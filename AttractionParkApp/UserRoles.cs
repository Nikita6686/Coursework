namespace AttractionParkApp
{
    public static class UserRoles
    {
        public const string Admin = "Администратор";
        public const string Visitor = "Пользователь";

        public static bool IsAdmin(string role) =>
            Normalize(role) == Admin;

        public static bool IsVisitor(string role) =>
            Normalize(role) == Visitor;

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
                default:
                    return role.Trim();
            }
        }
    }
}
