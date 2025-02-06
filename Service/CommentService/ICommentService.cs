namespace Project_Manager.Service.CommentService
{
    public interface ICommentService
    {
        Task CreateComment(string description, Guid issueId, string authorId);

        Task UpdateComment(string description, Guid commentId, string authorId);

        Task DeleteComment(Guid commentId, Guid issueId, string authorId);
    }
}
