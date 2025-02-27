using Project_Manager.DTO.CommentDto;
using Project_Manager.DTO.ProjectDto;

namespace Project_Manager.Service.IssueAnalyserService
{
    public interface IIssueAnalyserService
    {
        Task<string> UpdateAnalysis(string? note, string? comment, Guid analysisId, string authorId);

        Task<string> DeleteAnalysis(Guid analysisId, string authorId);

        Task<IEnumerable<ProjectTimelineDto>> GetAnalyses(Guid projectId, string authorId);

        Task<IEnumerable<ProjectTimelineDto>> GetIssueAnalyses(Guid projectId, Guid issueId, string authorId); 
    }
}
