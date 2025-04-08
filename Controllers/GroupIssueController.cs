using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Configuration;
using Project_Manager.DTO.GroupIssueDto;
using Project_Manager.DTO.IssueDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.GroupIssueService;
using Project_Manager.Service.IssueService;
using Project_Manager.Service.UserConfiguration.UserRoleConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{

    [Route("api/")]
    [ApiController]
    [Authorize]
    public class GroupIssueController : ControllerBase
    {
        private readonly IGroupIssueService issue;
        private readonly IUserRoleConfiguration configuration;

        public GroupIssueController( IGroupIssueService groupIssue, IUserRoleConfiguration roleConfiguration)
        {
            this.issue = groupIssue;
            this.configuration = roleConfiguration;
        }

        [HttpPut]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={issueId}/assignee={assignedTo}")]
        public async Task<IActionResult> AssignIssueToMember([Required] Guid projectId, Guid groupId,  Guid organizationId, Guid assignedTo, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                else
                {

                    var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                    if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator || r.Name == ApplicationRoleNames.GroupUser))
                    {
                        return Forbid("Access Denied: You are not an administrator");
                    }


                    await issue.AssignIssue(issueId, assignedTo, projectId);
                    return Ok("task assigned successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={issueId}/unassignee")]
        public async Task<IActionResult> UnAssignIssueFromMember(Guid projectId, Guid groupId, Guid organizationId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                else
                {
                    var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                    if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator))
                    {
                        return Forbid("Access Denied");
                    }

                    await issue.UnassignIssue(issueId, projectId);
                    return Ok("task unassigned successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/create-issue")]
        public async Task<IActionResult> CreateIssue([Required] Guid projectId, Guid groupId, 
            Guid organizationId, Guid? assignedTo,CreateGroupIssueDto createIssue)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                else
                {
                    var role = await configuration
                        .GetGroupRoleByEmail(user.Value, organizationId, groupId);

                    if (role == null || role.Name == ApplicationRoleNames.OrganizationMember
                        || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                    {
                        return Forbid("Access Denied: You are not a member in the group");
                    }

                    return Ok(await issue.
                        CreateIssues(projectId, createIssue, assignedTo, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={parentIssueId}/create-subIssue")]
        public async Task<IActionResult> CreateSubIssue(Guid projectId, Guid groupId, Guid? assignedTo, Guid organizationId, CreateGroupIssueDto issueDto, Guid parentIssueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }
                else
                {
                    var role = await configuration
                        .GetGroupRoleByEmail(user.Value, organizationId, groupId);

                    if (role == null || role.Name == ApplicationRoleNames.OrganizationMember
                        || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                    {
                        return Forbid("Access Denied: You are not a member in the group");
                    }

                    return Ok(await issue.CreateChildTask(projectId, issueDto, assignedTo, parentIssueId, user.Value));
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //add the project id later
        [HttpDelete]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={issueId}/delete")]
        public async Task<IActionResult> DeleteIssue(Guid issueId, Guid projectId, [Required] bool isDeleteChildren, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                var role = await configuration.GetGroupRoleByEmail(user.Value,organizationId, groupId);

                if (role == null || role.Name != ApplicationRoleNames.GroupAdministrator)
                {
                    return Forbid("You can't perform this action");
                }
                return Ok();
                //return Ok(await issue.DeleteIssues(issueId, projectId, isDeleteChildren, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //same here
        [HttpPut]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={issueId}/update")]
        public async Task<IActionResult> UpdateIssue(Guid issueId, Guid projectId,UpdateIssueDto dto, Guid? assignTo, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                var role = await configuration.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.UpdateIssues(issueId, dto, assignTo, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/projectId={projectId}/issues")]
        public async Task<IActionResult> GetProjectIssues
            (Guid projectId, [FromQuery] IssueType? issueType, [FromQuery] Complexity? complexity, [FromQuery] Progress? progress, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("user not found");
                }

                var role = await configuration.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssues(issueType, complexity, progress, projectId));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/projectId={projectId}/issues/page")]
        public async Task<IActionResult> GetProjectIssuesPaginated
            (Guid projectId, [FromQuery] IssueType? issueType, [FromQuery] Complexity? complexity, 
            [FromQuery] Progress? progress, int? page, int itemPerPage, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("user not found");
                }

                var role = await configuration.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssuesPaginated(issueType, complexity, progress, page, itemPerPage, projectId));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/default")]
        public async Task<IActionResult> GetProjectIssue(Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return Unauthorized("User does not exist");
                }

                var role = await configuration.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssue(projectId));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issue={issueId}")]
        public async Task<IActionResult> GetIssueById(Guid projectId, Guid issueId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                else
                {
                    var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                    if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator || r.Name == ApplicationRoleNames.GroupUser))
                    {
                        return Forbid("Access Denied: You do not belong to group");
                    }

                    return Ok(await issue.GetIssueById(projectId, issueId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/parent={parentIssueId}")]
        public async Task<IActionResult> GetSubIssues(Guid projectId, Guid parentIssueId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                else
                {
                    var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                    if (role == null || role.Any())
                    {
                        return Forbid("Access Denied: You do not have the permission");
                    }
                    return Ok(await issue.GetSubIssues(parentIssueId, projectId));
                }
                    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/issues")]
        public async Task<IActionResult> GetIssueAndChild(Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await issue.GetIssueAndChild(projectId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/origin={originId}/related={stateId}")]
        public async Task<IActionResult> AddRelatedIssue(Guid originId, Guid stateId, Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator || r.Name == ApplicationRoleNames.GroupUser))
                {
                    return Forbid("Access Denied: Permission denied");
                }

                await issue.AddRelatedIssue(originId, stateId, projectId);

                return Ok("Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/origin={originId}/related")]
        public async Task<IActionResult> GetRelatedIssues(Guid originId, Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator || r.Name == ApplicationRoleNames.GroupUser))
                {
                    return Forbid("Access Denied: permission denied");
                }

                return Ok(await issue.GetRelatedIssues(originId, projectId ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("organization={organizationId}/group={groupId}/project={projectId}/origin={originId}/remove")]
        public async Task<IActionResult> RemoveRelatedIssue(Guid originId, Guid stateId, Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var role = await configuration.GetUserRoles(Guid.Parse(user.Value), organizationId, groupId);

                if (role == null || !role.Any(r => r.Name == ApplicationRoleNames.GroupAdministrator || r.Name == ApplicationRoleNames.GroupUser))
                {
                    return Forbid("Access Denied: You are not an administrator");
                }

                await issue.RemoveRelatedIssue(originId, stateId, projectId);
                return Ok("successfully removed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
