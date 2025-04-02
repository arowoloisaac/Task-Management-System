using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace Project_Manager.Model
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<OrganizationUser>? OrganizationUser { get; set; }

        public ICollection<GroupUser>? GroupUser { get; set; }
    }
}
