namespace Project_Manager.DTO.RequestDto
{
    public class InvitationRequestDto
    {
        public Guid RequestId { get; set; }

        public Guid OrganizationId { get; set; }

        public string OrganizationName { get; set; } = string.Empty;
    }
}
