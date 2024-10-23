using System.ComponentModel.DataAnnotations;

namespace Project_Manager.DTO.UserDto
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage ="Enter the correct format of your mail")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
