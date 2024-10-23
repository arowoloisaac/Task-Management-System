namespace Project_Manager.Model
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid IssueId { get; set; }

        public User? Users { get; set; }
    }
}
