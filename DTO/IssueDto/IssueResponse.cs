using Project_Manager.DTO.ProjectDto;
using Project_Manager.DTO.TaskDto;
using Project_Manager.Model;

namespace Project_Manager.DTO.IssueDto
{
    public class IssueResponse
    {
        public IEnumerable<RetrieveIssue> Issues { get; set; } = new List<RetrieveIssue>();

        public Pagination Pagination { get; set; }

        public IssueResponse(List<RetrieveIssue> issue, int page, int total, int count, int start, int end, int totalItem)
        {
            Issues = issue;

            Pagination = new Pagination
            {
                Count = count,
                Current = page,
                Size = total,
                Start = start,
                End = end,
                TotalItems = totalItem
            };
        }
    }
}
