using Project_Manager.Enum;

namespace Project_Manager.Model
{
    public class IssueAnalyser : DateTimeClass
    {
        public Guid Id { get; set; }

        public string Note { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public WorkComponent? WorkComponent { get; set; }

        public Issue? Issue { get; set; }

        public Project? Project { get; set; }

        public Guid? User { get; set; }
    }
}
