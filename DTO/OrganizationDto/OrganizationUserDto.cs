namespace Project_Manager.DTO.OrganizationDto
{
    public class OrganizationUserDto
    {
        public Guid UserId { get; set; }

        //public Guid OrganizationId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string UserRole { get; set; } = string.Empty;
     }
}
