namespace CompareHare.Domain.Entities
{
    public class AlertUtilityPriceHistory
    {
        public int AlertMatchId { get; set; }
        public AlertMatch AlertMatch { get; set; }

        public int UtilityPriceHistoryId { get; set; }
        public UtilityPriceHistory UtilityPriceHistory { get; set; }

        public byte Order { get; set; }
    }
}
