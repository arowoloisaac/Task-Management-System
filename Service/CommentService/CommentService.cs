
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.DTO.CommentDto;
using Project_Manager.Model;

namespace Project_Manager.Service.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<User> _userManger;

        public CommentService(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbcontext = dbContext;
            _userManger = userManager; 
        }

        public async Task<string> CreateComment(string description, Guid issueId, string authorId)
        {
            var getIssue = await _dbcontext.Issues.FirstOrDefaultAsync(iss => iss.Id == issueId && iss.CreatedBy.ToString() == authorId);

            if (getIssue == null)
            {
                throw new Exception("Issue does not exist");
            }

            else
            {
                var getAutor = await _userManger.FindByIdAsync(authorId);
                var createComment = await _dbcontext.Comments.AddAsync( new Comment
                {
                    Id = Guid.NewGuid(),
                    Issue = getIssue,
                    Description = description,
                    CreatedDate = DateTime.Now,
                    User = getAutor,
                });

                await _dbcontext.SaveChangesAsync();
                return "Created";
            }
        }

        public async Task<string> DeleteComment(Guid commentId, Guid issueId, string authorId)
        {
            var getComment = await _dbcontext.Comments.SingleOrDefaultAsync(com => com.Id == commentId && com.Issue.Id == issueId && com.User.Id.ToString() == authorId);

            if (getComment == null)
            {
                throw new Exception("Commit does not exist");
            }
            else
            {
                var deleteComment =  _dbcontext.Comments.Remove(getComment);

                await _dbcontext.SaveChangesAsync();
                return "Succeeded";
            }
        }

        public async Task<IEnumerable<RetrieveCommentDto>> GetAllComments(Guid issueId, string authorId)
        {
            var retrieveComments = await _dbcontext.Comments.Where(com => com.Issue.Id == issueId && com.User.Id.ToString() == authorId).ToListAsync();

            
            var mappedComment = retrieveComments.Select( comment => new RetrieveCommentDto
            {
                CommentId = comment.Id,
                Description = comment.Description,
                CreatedAt = comment.CreatedDate,
                UpdatedAt = comment.ModifiedDate

            }).ToList();

            return mappedComment;
        }

        public async Task<RetrieveCommentDto> GetComment(Guid commentId, Guid issueId, string authorId)
        {
            var _getComment = await getComment(commentId, issueId, authorId);

            return new RetrieveCommentDto
            {
                CommentId = commentId,
                Description = _getComment.Description,
                Author = _getComment.User.UserName,
                CreatedAt = _getComment.CreatedDate,
                UpdatedAt = _getComment.ModifiedDate
            };
        }

        public async Task<string> UpdateComment(string description, Guid commentId, Guid issueId, string authorId)
        {
            var _getComment = await getComment(commentId, issueId, authorId);

            if (!string.IsNullOrEmpty(description))
            {
                _getComment.Description = description;
                _getComment.ModifiedDate = DateTime.Now;
            }

            await _dbcontext.SaveChangesAsync();

            return "Completed";
        }

        private async Task<Comment> getComment(Guid commentId, Guid issueId, string authorId)
        {
            var getComment = await _dbcontext.Comments.SingleOrDefaultAsync(com => com.Id == commentId && com.Issue.Id == issueId && com.User.Id.ToString() == authorId);
            if (getComment == null)
            {
                throw new Exception("Commit does not exist");
            }
            else
            {
                return getComment;
            }
        }
    }
}
