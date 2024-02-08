namespace CompareHare.Api.Features.Alerts.Models
{
    public class CreateAlertModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int UtilityState { get; set; }
        public int UtilityType { get; set; }
        public string Zip { get; set; }
        public int DistributorId { get; set; }
        public string DistributorRate { get; set; }

        public bool HasMinimumPrice { get; set; }
        public decimal MinimumPrice { get; set; }
        public bool HasMaximumPrice { get; set; }
        public decimal MaximumPrice { get; set; }

        public bool HasMinimumMonthLength { get; set; }
        public int MinimumMonthLength { get; set; }
        public bool HasMaximumMonthLength { get; set; }
        public int MaximumMonthLength { get; set; }

        public bool FilterRenewable { get; set; }
        public bool HasRenewable { get; set; }
        public decimal MinimumRenewablePercent { get; set; }
        public decimal MaximumRenewablePercent { get; set; }

        public bool FilterCancellationFee { get; set; }
        public bool HasCancellationFee { get; set; }

        public bool FilterMonthlyFee { get; set; }
        public bool HasMonthlyFee { get; set; }

        public bool FilterEnrollmentFee { get; set; }
        public bool HasEnrollmentFee { get; set; }

        public bool FilterNetMetering { get; set; }
        public bool HasNetMetering { get; set; }

        public bool FilterRequiresDeposit { get; set; }
        public bool RequiresDeposit { get; set; }

        public bool FilterBulkDiscounts { get; set; }
        public bool HasBulkDiscounts { get; set; }

        public string Comments { get; set; }
    }
}
