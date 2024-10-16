using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Project_Manager.Model
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public ICollection<Organization>? Organizations { get; set; }

        public ICollection<Project>? Projects { get; set; }

        public ICollection<Wiki>? Wiki { get; set; }
    }
}
