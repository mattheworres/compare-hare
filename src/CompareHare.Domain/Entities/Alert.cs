using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class Alert : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int AlertCriteriaId { get; set; }
        public AlertCriteria AlertCriteria { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StateUtilityIndexId { get; set; }
        public StateUtilityIndex StateUtilityIndex { get; set; }

        public virtual IEnumerable<AlertUtilityPriceHistory> UtilityPriceHistories { get; set; }

        [Required, MaxLength(40)]
        public string AlertOfferHash { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
