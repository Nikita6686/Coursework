namespace AttractionParkApp
{
    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Login { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }
        public static decimal Balance { get; set; }

        public static bool IsAdmin => UserRoles.IsAdmin(Role);
        public static bool IsVisitor => UserRoles.IsVisitor(Role);
        public static bool IsOperator => UserRoles.IsOperator(Role);
        public static bool IsTechnician => UserRoles.IsTechnician(Role);

        public static void RefreshBalance()
        {
            if (Id > 0)
                Balance = DatabaseHelper.GetUserBalance(Id);
        }

        public static void Clear()
        {
            Id = 0;
            Login = null;
            FullName = null;
            Role = null;
            Balance = 0;
        }
    }
}
