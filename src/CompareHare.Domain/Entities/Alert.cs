using System;
using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class Alert : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StateUtilityIndexId { get; set; }
        public StateUtilityIndex StateUtilityIndex { get; set; }

        [MaxLength(40)]
        public string StateUtilityIndexHash { get; set; }

        public bool Active { get; set; }

        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public bool? HasRenewable { get; set; }
        public decimal? MinimumRenewablePercent { get; set; }
        public decimal? MaximumRenewablePercent { get; set; }
        public int? MinimumMonthLength { get; set; }
        public int? MaximumMonthLength { get; set; }

        public bool? HasCancellationFee { get; set; }
        public bool? HasMonthlyFee { get; set; }
        public bool? HasEnrollmentFee { get; set; }
        public bool? HasNetMetering { get; set; }
        public bool? RequiresDeposit { get; set; }
        public bool? HasBulkDiscounts { get; set; }

        public AlertMatch AlertMatch { get; set; }

        [MaxLength(512)]
        public string Comments { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
