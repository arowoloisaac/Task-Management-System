namespace Project_Manager.Model
{
    public class Note : DateTimeClass
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid? IssueId { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public User Users { get; set; }
    }
}
