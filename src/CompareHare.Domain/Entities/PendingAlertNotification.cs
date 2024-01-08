#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class PendingAlertNotification : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AlertMatchId { get; set; }
        public AlertMatch AlertMatch { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
