using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_Manager.DTO.UserDto;
using Project_Manager.Model;

namespace Project_Manager.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<User, GetProfileDto>();

        }
    }
}
