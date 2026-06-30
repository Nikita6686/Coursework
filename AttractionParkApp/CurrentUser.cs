namespace AttractionParkApp
{
    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Login { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }

        public static bool IsAdmin => UserRoles.IsAdmin(Role);
        public static bool IsVisitor => UserRoles.IsVisitor(Role);

        public static void Clear()
        {
            Id = 0;
            Login = null;
            FullName = null;
            Role = null;
        }
    }
}
