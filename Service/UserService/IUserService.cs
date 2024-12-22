using Project_Manager.DTO.UserDto;
using Project_Manager.Model;

namespace Project_Manager.Service.UserService
{
    public interface IUserService
    {
        Task<TokenResponse> RegisterUser(RegisterDto registerDto);

        Task<TokenResponse> LoginUser(LoginDto loginDto);

        Task<GetProfileDto> UserProfile(string userId);

        Task<GetProfileDto> UpdateProfile(UpdateDto updateDto, Guid? avatar, string userId);
    }
}
