using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project_Manager.Configuration;
using Project_Manager.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_Manager.Service.Configuration.TokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtBearerSetting _tokenSetting;

        public TokenGenerator(IOptionsSnapshot<JwtBearerSetting> jwtBearer)
        {
            _tokenSetting = jwtBearer.Value;
        }


        public string GenerateToken(User user, IList<string>? userRoles)
        {
            var TokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenSetting.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, user.Id.ToString()),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(_tokenSetting.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _tokenSetting.Audience,
                Issuer = _tokenSetting.Issuer,
            };

            var token = TokenHandler.CreateToken(descriptor);

            return TokenHandler.WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            var TokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenSetting.SecretKey);

            var credentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature  
                );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, user.Id.ToString()),
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(_tokenSetting.ExpiryTimeInSeconds),
                SigningCredentials = credentials,
                Audience = _tokenSetting.Audience,
                Issuer = _tokenSetting.Issuer,
            };

            var token = TokenHandler.CreateToken(descriptor);

            return TokenHandler.WriteToken(token);
        }
    }
}
