using System.ComponentModel.DataAnnotations;

namespace Project_Manager.DTO.UserDto
{
    public class UpdateDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateOnly BirthDate {  get; set; }
    }
}
