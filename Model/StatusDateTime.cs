namespace Project_Manager.Model
{
    public class StatusDateTime
    {
        public DateTime CreatedTime { get; set; }

        //this wroks with the last update
        public DateTime UpdatedTime { get; set; }

        public DateTime DeletedTime { get; set; }

        public DateTime ArchivedTime { get; set; }
    }
}
