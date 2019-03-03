using System;

namespace CompareHare.Api.Features.Offers.Models
{
    //For purposes of hashing new data and determining if it has changed
    public class UtilityPriceHashModel
    {
        public int StateUtilityIndexId { get; set; }
        public string Name { get; set; }
        public string PriceUnit { get; set; }
        public float? PricePerUnit { get; set; }
        public string FlatRate { get; set; }
        public string PriceStructure { get; set; }
        public bool HasCancellationFee { get; set; }
        public string CancellationFee { get; set; }
        public bool HasMonthlyFee { get; set; }
        public string MonthlyFee { get; set; }
        public bool HasEnrollmentFee { get; set; }
        public string EnrollmentFee { get; set; }
        public bool HasNetMetering { get; set; }
        public string NetMetering { get; set; }
        public bool RequiresDeposit { get; set; }
        public bool HasBulkDiscounts { get; set; }
        public bool IsIntroductoryPrice { get; set; }
        public bool HasRenewable { get; set; }
        public float? RenewablePercentage { get; set; }
        public int TermMonthLength { get; set; }
        public string SupplierPhone { get; set; }
        public bool HasTermEndDate { get; set; }
        public DateTime? TermEndDate { get; set; }
        public string OfferId { get; set; }
        public string OfferUrl { get; set; }
        public string Comments { get; set; }
    }
}
