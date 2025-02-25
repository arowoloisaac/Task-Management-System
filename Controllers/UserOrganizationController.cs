using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.UserOrganizationService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class UserOrganizationController : ControllerBase
    {
        private readonly IOrganizationUserService _organizationUser;

        public UserOrganizationController(IOrganizationUserService organizationUserService)
        {
            _organizationUser = organizationUserService;
        }


        [HttpGet]
        [Route("organization/{id}")]
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

        [HttpGet]
        [Route("organization={id}/users")]
        public async Task<IActionResult> GetOrganizationUsers(Guid id)
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
                    return Ok(await _organizationUser.OrganizationUsers(id, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("organization")]
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
        [Route("organization/{organizationId}/invite={inviteeMail}")]
        public async Task<IActionResult> SendInvitationRequests(Guid organizationId, string inviteeMail)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User doesn't exist");
                }

                return Ok(await _organizationUser.SendOrganizationRequest(organizationId, inviteeMail, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("organization/{organizationId}/invite={inviteeMail}")]
        public async Task<IActionResult> RevokeInvitationRequests(Guid organizationId, string inviteeMail)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User doesn't exist");
                }

                return Ok(await _organizationUser.RevokeOrganizationRequest(organizationId, inviteeMail, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("organization/{id}/remove/{memberEmail}")]
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


        [HttpPost]
        [Route("notification/request/accept/{organizationId}")]
        public async Task<IActionResult> AcceptOrganizationRequest(Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if( user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await _organizationUser.AcceptOrganizationRequest(organizationId, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("notification/request/reject/{organizationId}")]
        public async Task<IActionResult> RejectOrganizationRequest(Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _organizationUser.RejectOrganizationRequest(organizationId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("notification/request")]
        public async Task<IActionResult> GetInvitationList()
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _organizationUser.InvitationList(user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
