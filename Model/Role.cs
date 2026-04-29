using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Model
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<OrganizationUser>? OrganizationUser { get; set; }
    }
}
