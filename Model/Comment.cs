namespace Project_Manager.Model
{
    public class Comment : DateTimeClass
    {
        public Guid Id { get; set; }

        public string Descriptiom { get; set; } = string.Empty;

        public required Issue Issue { get; set; }

        public User? User { get; set; }
    }
}
