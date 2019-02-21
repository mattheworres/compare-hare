using System.Collections.Generic;

namespace CompareHare.Api.Domain.Features.Authentication.Models
{
    public class UserIdentityModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
