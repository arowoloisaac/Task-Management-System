using Project_Manager.DTO.IssueDto;
using Project_Manager.DTO.TaskDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.IssueService
{
    public interface IIssueService
    {
        //to be able to filter with task assigned to the actor
        Task<IEnumerable<RetrieveIssue>> GetIssues(string mail);

        Task<string> CreateIssues(Guid projectId, CreateIssue issueDto, string mail);

        Task<string> UpdateIssues(Guid issueId, string? Name, string? Description,
            Complexity? complexity, uint? EstimatedTimeInMinute, string mail);

        Task<string> DeleteIssues(Guid issueId, bool isDeleteChildren, string mail);

        Task<string> CreateSubIssue(Guid projectId, CreateIssue subIssueDto, Guid parentIssueId, string mail);

        Task<List<RetrieveIssue>> GetIssue(Guid projectId);
    }
}
