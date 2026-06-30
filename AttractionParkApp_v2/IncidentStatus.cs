namespace AttractionParkApp
{
    public static class IncidentStatus
    {
        public const string New = "Новая";
        public const string InProgress = "В работе";
        public const string Fixed = "Исправлена";

        public static string Normalize(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return New;

            switch (status.Trim().ToLowerInvariant())
            {
                case "новая":
                case "new":
                    return New;
                case "в работе":
                case "in progress":
                    return InProgress;
                case "исправлена":
                case "fixed":
                case "закрыта":
                    return Fixed;
                default:
                    return status.Trim();
            }
        }
    }
}
