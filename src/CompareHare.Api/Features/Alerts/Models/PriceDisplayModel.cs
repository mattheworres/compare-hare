

namespace CompareHare.Api.Features.Alerts.Models
{
    public class PriceDisplayModel
    {
        public PriceDisplayModel()
        {
            UtilityState = "";
            UtilityType = "";
            Name = "";
            PriceUnit = "";
            FlatRate = "";
            PriceStructure = "";
            CancellationFee = "";
            MonthlyFee = "";
            EnrollmentFee = "";
            NetMetering = "";
            SupplierPhone = "";
            OfferId = "";
            OfferUrl = "";
        }

        public int Id { get; set; }

        public string UtilityState { get; set; }
        public string UtilityType { get; set; }

        public string Name { get; set; }

        public string PriceUnit { get; set; }

        public float? PricePerUnit { get; set; }

        public string FlatRate { get; set; }

        public string PriceStructure { get; set; }

        public bool? HasCancellationFee { get; set; }
        public string CancellationFee { get; set; }

        public bool? HasMonthlyFee { get; set; }
        public string MonthlyFee { get; set; }

        public bool? HasEnrollmentFee { get; set; }
        public string EnrollmentFee { get; set; }

        public bool? HasNetMetering { get; set; }
        public string NetMetering { get; set; }

        public bool? RequiresDeposit { get; set; }
        public bool? HasBulkDiscounts { get; set; }
        public bool? IsIntroductoryPrice { get; set; }

        public bool? HasRenewable { get; set; }
        public float? RenewablePercentage { get; set; }

        public int? TermMonthLength { get; set; }

        public string SupplierPhone { get; set; }

        public bool? HasTermEndDate { get; set; }
        public DateTime? TermEndDate { get; set; }

        public string OfferId { get; set; }

        public string OfferUrl { get; set; }

    }
}
