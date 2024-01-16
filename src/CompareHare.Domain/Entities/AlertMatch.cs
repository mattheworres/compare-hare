#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class AlertMatch : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public AlertMatch()
        {
            AlertOfferHash = "";
        }

        public int Id { get; set; }

        public int AlertId { get; set; }
        public Alert Alert { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StateUtilityIndexId { get; set; }
        public StateUtilityIndex StateUtilityIndex { get; set; }

        public virtual IEnumerable<AlertMatchUtilityPriceHistory> UtilityPriceHistories { get; set; }

        [Required, MaxLength(40)]
        public string AlertOfferHash { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
