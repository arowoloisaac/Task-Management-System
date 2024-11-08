namespace Project_Manager.Model
{
    public class GroupUser
    {
        public Guid Id { get; set; }

        public User? User { get; set; }

        public Group? Group { get; set; }

        public Guid RoleId { get; set; }
    }
}
