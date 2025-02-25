using Project_Manager.Model;

namespace Project_Manager.DTO.IssueDto
{
    public class DeadlineListDto
    {
        public string User {  get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateOnly EndDate { get; set; }
    }
}
