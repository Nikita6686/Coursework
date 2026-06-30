namespace AttractionParkApp
{
    public static class AttractionStatus
    {
        public const string Working = "Работает";
        public const string Maintenance = "На обслуживании";
        public const string Broken = "Не работает";

        public static string Normalize(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Working;

            switch (status.Trim().ToLowerInvariant())
            {
                case "работает":
                case "открыт":
                case "open":
                case "рабочий":
                    return Working;
                case "на обслуживании":
                case "обслуживание":
                case "maintenance":
                    return Maintenance;
                case "не работает":
                case "поломка":
                case "broken":
                case "closed":
                    return Broken;
                default:
                    return status.Trim();
            }
        }

        public static bool CanBuyTicket(string status) =>
            Normalize(status) == Working;
    }
}
