using Project_Manager.Model;

namespace Project_Manager.Service.Configuration.TokenGenerator
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user, IList<string>? userRoles);

        string GenerateToken(User user);
    }
}
