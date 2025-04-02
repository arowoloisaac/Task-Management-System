namespace Project_Manager.Model
{
    public class ObjectUpload
    {
        public required IFormFile File { get; set; } 

        public string FileName { get; set; } = string.Empty;
    }
}
