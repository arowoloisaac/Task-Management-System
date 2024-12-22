using Project_Manager.DTO.AvatarDto;

namespace Project_Manager.Service.AvatarService
{
    public interface IAvatarService
    {
    //to consider having an enum which serves as a separator of either maile or female
        Task AddAvatar(string avatarUrl);

        Task<GetAvatarDto> GetAvatar(Guid Id);

        Task<IEnumerable<GetAvatarDto>> GetAvatars();

        Task DeleteAvatar(Guid Id);
    }
}
