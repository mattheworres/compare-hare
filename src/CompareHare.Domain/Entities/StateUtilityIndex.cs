using System;
using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class StateUtilityIndex : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
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
