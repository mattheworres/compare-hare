using System;
using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class UtilityPrice : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int StateUtilityIndexId { get; set; }
        public StateUtilityIndex StateUtilityIndex { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string PriceUnit { get; set; }

        public float PricePerUnit { get; set; }

        [MaxLength(120)]
        public string FlatRate { get; set; }

        [MaxLength(255)]
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
        public float RenewablePercentage { get; set; }

        public int TermMonthLength { get; set; }

        public DateTime TermEndDate { get; set; }

        [MaxLength(256)]
        public string OfferUrl { get; set; }

        [MaxLength(512)]
        public string Comments { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
