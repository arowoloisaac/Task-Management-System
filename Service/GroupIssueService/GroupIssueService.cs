using Project_Manager.Data;
using Project_Manager.DTO.GroupIssueDto;
using Project_Manager.DTO.IssueDto;
using Project_Manager.DTO.TaskDto;
using Project_Manager.Enum;
using Project_Manager.Service.UserConfiguration;

namespace Project_Manager.Service.GroupIssueService
{
    public class GroupIssueService : IGroupIssueService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserConfig config;

        public GroupIssueService(ApplicationDbContext context, IUserConfig config)
        {
            this.context = context;
            this.config = config;
        }

        public Task AssignIssue()
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateIssues(Guid projectId, CreateGroupIssueDto issueDto, Guid assignTo, string mail)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateSubIssue(Guid projectId, CreateIssue subIssueDto, Guid assignTo, Guid parentIssueId, string mail)
        {
            throw new NotImplementedException();
        }


        public Task<List<RetrieveIssue>> GetIssue(Guid projectId, string userMail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IssueAndChild>> GetIssueAndChild(Guid projectId, string userMail)
        {
            throw new NotImplementedException();
        }

        public Task<RetrieveIssue> GetIssueById(Guid projectId, Guid issueId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RetrieveIssue>> GetIssues(IssueType? issueType, Complexity? complexity, Progress? progress, Guid projectId, string mail)
        {
            throw new NotImplementedException();
        }

        public Task<IssueResponse> GetIssuesPaginated(IssueType? issueType, Complexity? complexity, Progress? progress, int? page, int itemPerPage, Guid projectId, string mail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RetrieveIssue>> GetSubIssues(Guid parentId, Guid projectId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task UnassignIssue()
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateIssues(Guid issueId, string? Name, string? Description, Complexity? complexity, uint? EstimatedTimeInMinute, uint? timeSpent, int? issueLevel, string? comment, string? note, Guid projectId, string mail)
        {
            throw new NotImplementedException();
        }
    }
}
