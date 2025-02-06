using Project_Manager.DTO.CommentDto;

namespace Project_Manager.Service.CommentService
{
    public interface ICommentService
    {
        Task<string> CreateComment(string description, Guid issueId, string authorId);

        Task<string> UpdateComment(string description, Guid commentId, Guid issueId, string authorId);

        Task<string> DeleteComment(Guid commentId, Guid issueId, string authorId);

        Task<IEnumerable<RetrieveCommentDto>> GetAllComments(Guid issueId,string authorId);

        Task<RetrieveCommentDto> GetComment(Guid commentId, Guid issueId, string authorId);


    }
}
