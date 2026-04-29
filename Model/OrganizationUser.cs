namespace Project_Manager.Model
{
    public class OrganizationUser
    {
        public Guid Id { get; set; }

        public Organization? Organization { get; set; }

        public User? User { get; set; }

        public Role? Role { get; set; }
    }
}
