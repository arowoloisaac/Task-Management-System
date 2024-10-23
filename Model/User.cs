using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Project_Manager.Model
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Organization>? Organizations { get; set; }

        public ICollection<Project>? Projects { get; set; }

        public ICollection<Wiki>? Wiki { get; set; }

        public ICollection<Group>? Groups { get; set; }

        public ICollection<Comment>? Comment { get; set; }

        public ICollection<Note>? Notes { get; set; }
    }
}
