using Microsoft.AspNetCore.Identity;
using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration
{
    public class UserConfig : IUserConfig
    {
        private readonly UserManager<User> _userManager;

        public UserConfig(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if (user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            
            return user;
        }
    }
}
