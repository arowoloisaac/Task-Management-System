namespace Project_Manager.Model
{
    public class OrganizationUser
    {
        public Guid Id { get; set; }

        public required Organization Organization { get; set; }

        public required User User { get; set; }

        public required Role Role { get; set; }
    }
}
