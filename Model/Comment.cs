namespace Project_Manager.Model
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Descriptiom { get; set; } = string.Empty;

        public Guid TaskId { get; set; }
    }
}
