using Microsoft.AspNetCore.Identity;

namespace CompareHare.Domain.Entities
{
    public class Role : IdentityRole<int> // TODO: this needs updated for sure
    {
        public Role() {}
        public const string Admin = "Admin";
        public const string User = "User";
        public Role(string roleName)
            : base(roleName) {}
    }
}
