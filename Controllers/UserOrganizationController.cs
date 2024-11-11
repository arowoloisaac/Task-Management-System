using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Enum;
using Project_Manager.Service.UserOrganizationService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserOrganizationController : ControllerBase
    {
        private readonly IOrganizationUserService _organizationUser;

        public UserOrganizationController(IOrganizationUserService organizationUserService)
        {
            _organizationUser = organizationUserService;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                else
                {
                    return Ok(await _organizationUser.GetOrganization(id, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetOrganization([FromQuery] OrganizationFilter? filter)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("user not found");
                }
                return Ok(await _organizationUser.GetOrganizations(filter, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
