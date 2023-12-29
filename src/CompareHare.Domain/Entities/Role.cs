using Microsoft.AspNetCore.Identity;

namespace CompareHare.Domain.Entities
{
    public class Role : IdentityRole<int> // TODO: this needs updated for sure
    {
        public Role() {}
        public Role(string roleName)
            : base(roleName) {}
    }
}
