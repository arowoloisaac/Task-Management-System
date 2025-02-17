namespace Project_Manager.DTO.NoteDto
{
    public class NoteDto
    {
        public Guid Id { get; set; }

        public required string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
