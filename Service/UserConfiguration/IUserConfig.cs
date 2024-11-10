using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration
{
    public interface IUserConfig
    {
        Task<User> GetUser(string mail); 

        Task<User> GetUserById(string id);
    }
}
