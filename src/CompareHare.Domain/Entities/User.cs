#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Domain.Entities
{
    public class User : IdentityUser<int>, ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        [Required]
        public bool Active { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(45)]
        public string TimeZone { get; set; }

        [MaxLength(128)]
        public string PasswordResetToken { get; set; }
        public DateTimeOffset? PasswordResetTokenExpirationDate { get; set; }
        public DateTime? FirstLogin { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? FullAccessGrantedDate { get; set; }

        public virtual ISet<Role> Roles { get; set; }
        // public virtual IEnumerable<Alert> Alerts { get; set; }
        // public virtual IEnumerable<AlertMatch> AlertMatches { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
