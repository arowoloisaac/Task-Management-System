namespace Project_Manager.Model
{
    //this table validated the users that was sent a request to join an organization
    public class Requests
    {
        public Guid Id { get; set; }

        public string InviteeEmail { get; set; } = string.Empty;

        public Guid OrganizationId { get; set; }

        public Guid UserId { get; set; }
    }
}
