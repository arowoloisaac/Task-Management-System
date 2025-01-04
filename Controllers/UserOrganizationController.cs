using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Enum;
using Project_Manager.Service.UserOrganizationService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/organization")]
    [ApiController]
    [EnableCors]
    public class UserOrganizationController : ControllerBase
    {
        private readonly IOrganizationUserService _organizationUser;

        public UserOrganizationController(IOrganizationUserService organizationUserService)
        {
            _organizationUser = organizationUserService;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrganization(Guid id)
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

        //[HttpGet]
        //[Route("/users/org={id}")]
        //public async Task<IActionResult> GetOrganizationUsers(Guid id)
        //{
        //    try
        //    {
        //        var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

        //        if (user == null)
        //        {
        //            return NotFound("User not found");
        //        }
        //        else
        //        {
        //            return Ok(await _organizationUser.organizationUsers(id, user.Value));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetOrganizations([FromQuery] OrganizationFilter? filter)
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


        [HttpPost]
        [Route("{organizationId}/invite={inviteeMail}")]
        public async Task<IActionResult> SendInvitationRequests(Guid organizationId, string inviteeMail)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User doesn't exist");
                }

                return Ok(await _organizationUser.AddUserToOrganization(organizationId, inviteeMail, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id}/remove/{memberEmail}")]
        public async Task<IActionResult> RemoveUserFromOrganization(Guid id, string memberEmail)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if(user == null)
                {
                    return NotFound("User does not exist");
                }

                return Ok(await _organizationUser.RemoveUserFromOrganization(id, memberEmail, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
