namespace Project_Manager.DTO.CommentDto
{
    public class RetrieveCommentDto
    {
        public Guid CommentId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
