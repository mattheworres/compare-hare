namespace CompareHare.Domain.Entities
{
    public class AlertUtilityPriceHistory
    {
        public int AlertId { get; set; }
        public Alert Alert { get; set; }

        public int UtilityPriceHistoryId { get; set; }
        public UtilityPriceHistory UtilityPriceHistory { get; set; }

        public byte Order { get; set; }
    }
}
