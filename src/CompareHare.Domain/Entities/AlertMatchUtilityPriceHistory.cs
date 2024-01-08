
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace CompareHare.Domain.Entities
{
    public class AlertMatchUtilityPriceHistory
    {
        public int AlertMatchId { get; set; }

        public AlertMatch AlertMatch { get; set; }

        public int UtilityPriceHistoryId { get; set; }

        public UtilityPriceHistory UtilityPriceHistory { get; set; }


        public byte Order { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
