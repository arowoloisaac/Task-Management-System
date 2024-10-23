namespace Project_Manager.Model
{
    public class Project : ObjectDateTime
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        //foreign key for personal projects
        public Guid UserId { get; set; }

        public ICollection<Issue>? Issues { get; set; }

        public ICollection<Wiki>? Wiki { get; set; }

        public Guid GroupId { get; set; }
    }
}
