using System;
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
