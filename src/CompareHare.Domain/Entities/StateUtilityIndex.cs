#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class StateUtilityIndex : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public StateUtilityIndex()
        {
            LastUpdatedHash = "";
        }

        public int Id { get; set; }

        [MaxLength(256)]
        public virtual string LoaderDataIdentifier { get; set; }

        [Required]
        public UtilityStates UtilityState { get; set; }

        [Required]
        public UtilityTypes UtilityType { get; set; }

        [Required]
        public bool Active { get; set; }

        [MaxLength(40)]
        public string LastUpdatedHash { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
