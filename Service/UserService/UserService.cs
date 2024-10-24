using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project_Manager.DTO.UserDto;
using Project_Manager.Model;
using Project_Manager.Service.Configuration.TokenGenerator;

namespace Project_Manager.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, ITokenGenerator tokenGenerator, IMapper mapper)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
        }


        public async Task<TokenResponse> LoginUser(LoginDto loginDto)
        {
            var user = await ValidateUser(loginDto);

            var token = _tokenGenerator.GenerateToken(user, await _userManager.GetRolesAsync(user));

            return new TokenResponse(token);
        }

        public async Task<TokenResponse> RegisterUser(RegisterDto registerDto)
        {
            var findUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (findUser != null)
            {
                throw new Exception($"User with Email - {registerDto.Email} already exist in the database");
            }

            else
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var minimumBirthDate = today.AddYears(-10);

                if (registerDto.BirthDate >= minimumBirthDate)
                {
                    throw new Exception("BirthDate must be at least 10 years in the past.");
                }
                

                var createUser = await _userManager.CreateAsync( new User
                {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    Birthdate = registerDto.BirthDate,
                    CreatedAt = DateTime.UtcNow,
                }, registerDto.Password);

                if (!createUser.Succeeded)
                {
                    throw new Exception("Unable to register user into the database");
                }

                else
                {
                    var retrieveRegisteredUser = await _userManager.FindByEmailAsync(registerDto.Email);

                    var token = _tokenGenerator.GenerateToken(retrieveRegisteredUser);
                    return new TokenResponse(token);
                }
            }
            
        }

        public async Task<GetProfileDto> UpdateProfile(UpdateDto updateDto, string userId)
        {
            var getUser = await _userManager.FindByIdAsync(userId);

            if (getUser == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                if (!string.IsNullOrEmpty(updateDto.FirstName))
                {
                    getUser.FirstName = updateDto.FirstName;
                }

                if (!string.IsNullOrEmpty(updateDto.LastName))
                {
                    getUser.LastName = updateDto.LastName;
                }

                if (!string.IsNullOrEmpty(updateDto.PhoneNumber))
                {
                    getUser.PhoneNumber = updateDto.PhoneNumber;
                }
                if(updateDto.BirthDate != null)
                {
                    var today = DateOnly.FromDateTime(DateTime.Now);

                    var minimumBirthDate = today.AddYears(-10);

                    if(updateDto.BirthDate >= minimumBirthDate)
                    {
                        throw new Exception("BirthDate must be at least 10 years in the past.");
                    }
                    else
                    {
                        getUser.Birthdate = updateDto.BirthDate;
                    }
                }

                var updateUser = await _userManager.UpdateAsync(getUser);

                if (updateUser.Succeeded)
                {
                    var newProfile = _mapper.Map<GetProfileDto>(getUser);
                    return newProfile;
                }

                //var newProfile = _mapper.Map<GetProfileDto>(updateUser);
                throw new Exception("can't");
            }
        }


        public async Task<GetProfileDto> UserProfile(string userId)
        {
            var getUser = await _userManager.FindByIdAsync(userId);

            if (getUser == null)
            {
                throw new Exception("User doesn't exist");
            }
            else
            {
                var profile = _mapper.Map<GetProfileDto>(getUser);

                return profile; 
                /*return new GetProfileDto
                {
                    FirstName = getUser.FirstName,
                    LastName = getUser.LastName,
                    Birthdate = getUser.Birthdate,
                    Email = getUser.Email,
                    PhoneNumber = getUser.PhoneNumber
                };*/
            }
        }


        private async Task<User> ValidateUser(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception($"User can be found {loginDto.Email}");
            }

            else
            {
                var validatePassword = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

                return validatePassword == PasswordVerificationResult.Success ? user: null;
            }
        }
    }
}


