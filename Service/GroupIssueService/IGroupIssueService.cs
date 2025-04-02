using Project_Manager.DTO.GroupIssueDto;
using Project_Manager.DTO.IssueDto;
using Project_Manager.DTO.TaskDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.GroupIssueService
{
    public interface IGroupIssueService
    {
        Task<IEnumerable<RetrieveIssue>> GetIssues(IssueType? issueType, Complexity? complexity, Progress? progress, Guid projectId, string mail);

        Task<IssueResponse> GetIssuesPaginated(IssueType? issueType, Complexity? complexity, Progress? progress, int? page, int itemPerPage, Guid projectId, string mail);

        Task<string> CreateIssues(Guid projectId, CreateGroupIssueDto issueDto, Guid assignTo, string mail);

        Task<string> UpdateIssues(Guid issueId, string? Name, string? Description,
            Complexity? complexity, uint? EstimatedTimeInMinute, uint? timeSpent,
            int? issueLevel, string? comment, string? note, Guid projectId, string mail);


        Task<string> CreateSubIssue(Guid projectId, CreateIssue subIssueDto, Guid assignTo, Guid parentIssueId, string mail);

        Task<List<RetrieveIssue>> GetIssue(Guid projectId, string userMail);

        Task<IEnumerable<RetrieveIssue>> GetSubIssues(Guid parentId, Guid projectId, string userId);

        Task<RetrieveIssue> GetIssueById(Guid projectId, Guid issueId, string userId);

        Task AssignIssue();

        Task UnassignIssue();
        //Task AddRelatedIssue(Guid originId, Guid stateId, string userId);

        //Task RemoveRelatedIssue(Guid issueId, string userId);

        //Task GetRelatedIssues(Guid issueId, string userId);

        Task<IEnumerable<IssueAndChild>> GetIssueAndChild(Guid projectId, string userMail);
    }
}
