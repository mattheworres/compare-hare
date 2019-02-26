using System;
using CompareHare.Domain.Services;

namespace CompareHare.Domain.Entities
{
    public class PendingAlertNotification : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AlertId { get; set; }
        public Alert Alert { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
