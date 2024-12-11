using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Model;
using Project_Manager.Service.OrganizationProjectService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableCors]
    public class GroupController : ControllerBase
    {
        private readonly IOrganizationGroupService _groupService;

        public GroupController(IOrganizationGroupService groupService)
        {
            _groupService = groupService;
        }


        [HttpPost]
        [Route("org={organizationId}/create")]
        public async Task<IActionResult> CreateGroup(string groupName, Guid organizationId)
        {
            try
            {
                var getAdmin = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (getAdmin != null)
                {
                    return Ok(await _groupService.CreateOrganizationGroup(groupName, organizationId, getAdmin.Value));
                }
                return NotFound("User not found");
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("org={organizationId}/upate/grp={groupId}")]
        public async Task<IActionResult> UpdateGroup([Required] Guid groupId, [Required] Guid organizationId, string? groupName)
        {
            try
            {
                var getAdmin = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (getAdmin != null)
                {
                    return Ok(await _groupService.UpdateOrganizationGroup(groupId, groupName, organizationId, getAdmin.Value));
                }
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("org={organizationId}/grp={groupId}")]
        public async Task<IActionResult> RetrieveGroupById(Guid groupId, Guid organizationId)
        {
            try
            {
                var getAdmin = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (getAdmin != null)
                {
                    return Ok(await _groupService.RetrieveOrganizationGroupById(groupId, organizationId, getAdmin.Value));
                }
                return NotFound("User not found");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("org={organizationId}/groups")]
        public async Task<IActionResult> RetrieveGroups(Guid organizationId)
        {
            try
            {
                var getAdmin = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (getAdmin != null)
                {
                    return Ok(await _groupService.RetrieveOrganizationGroup(getAdmin.Value, organizationId));
                }
                return NotFound("User not found");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("org={organizationId}/delete/{groupId}")]
        public async Task<IActionResult> DeleteGroup(Guid groupId, Guid organizationId)
        {
            try
            {
                var getAdmin = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (getAdmin != null)
                {
                    return Ok(await _groupService.DeleteOrganizationGroup(groupId, organizationId, getAdmin.Value));
                }
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
