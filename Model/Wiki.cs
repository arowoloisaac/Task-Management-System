namespace Project_Manager.Model
{
    public class Wiki : UserSection
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedTime { get; set; }

        //this wroks with the last update
        public DateTime UpdatedTime { get; set; }

        public DateTime DeletedTime { get; set; }

        public Project? Project { get; set; }

        public User? User { get; set; }
    }
}
