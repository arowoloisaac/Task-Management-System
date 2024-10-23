namespace Project_Manager.Model
{
    public class Comment : UserSection
    {
        public Guid Id { get; set; }

        public string Descriptiom { get; set; } = string.Empty;

        public Issue? Issue { get; set; }

        public User? User { get; set; }
    }
}
