using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Configuration;
using Project_Manager.DTO.IssueDto;
using Project_Manager.Enum;
using Project_Manager.Service.GroupIssueService;
using Project_Manager.Service.IssueService;
using Project_Manager.Service.UserConfiguration.UserRoleConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{

    [Route("api/organization")]
    [ApiController]
    [Authorize]
    public class GroupIssueController : ControllerBase
    {
        private readonly IIssueService issue;
        private readonly IGroupIssueService group;
        private readonly IUserRoleConfiguration roleClaim;

        /***
* potential expectations
* this controller will be using both the normal issue service and the group issus service
* reasons:
* a normal group member only possess similar attritutes to creating issues and can be assigned to only themself ***/

        public GroupIssueController(IIssueService issueService, IGroupIssueService groupIssue, IUserRoleConfiguration roleConfiguration)
        {
            this.issue = issueService;
            this.group = groupIssue;
            this.roleClaim = roleConfiguration;
        }


        /*public async Task<IActionResult> AssignIssueToMember()
        {
            throw new NotImplementedException();
        }


        public async Task<IActionResult> UnAssignIssueFromMember()
        {
            throw new NotImplementedException();
        }*/

        [HttpPost]
        [Route("project={projectId}/create-issue")]
        public async Task<IActionResult> CreateIssue([Required] Guid projectId, CreateIssue createIssue)
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
                    return Ok(await issue.CreateIssues(projectId, createIssue, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("project={projectId}/issue={parentIssueId}/create-subIssue")]
        public async Task<IActionResult> CreateSubIssue(Guid projectId, CreateIssue issueDto, Guid parentIssueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await issue.CreateSubIssue(projectId, issueDto, parentIssueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //add the project id later
        [HttpDelete]
        [Route("={organizationId}/group={groupId}/project={projectId}/issue={issueId}/delete")]
        public async Task<IActionResult> DeleteIssue(Guid issueId, Guid projectId, [Required] bool isDeleteChildren, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                var role = await roleClaim.GetGroupRoleByEmail(user.Value,organizationId, groupId);

                if (role == null || role.Name != ApplicationRoleNames.GroupAdministrator)
                {
                    return Forbid("You can't perform this action");
                }
                return Ok(await issue.DeleteIssues(issueId, projectId, isDeleteChildren, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //same here
        [HttpPut]
        [Route("={organizationId}/group={groupId}/project={projectId}/issue={issueId}/update")]
        public async Task<IActionResult> UpdateIssue(Guid issueId, Guid projectId,UpdateIssueDto dto, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                var role = await roleClaim.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.UpdateIssues(issueId,dto, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("={organizationId}/group={groupId}/projectId={projectId}/issues")]
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

                var role = await roleClaim.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember || role.Name == ApplicationRoleNames.OrganizationAdministrator)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssues(issueType, complexity, progress, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("={organizationId}/group={groupId}/projectId={projectId}/issues/page")]
        public async Task<IActionResult> GetProjectIssuesPaginated
            (Guid projectId, [FromQuery] IssueType? issueType, [FromQuery] Complexity? complexity, [FromQuery] Progress? progress, int? page, int itemPerPage, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return Unauthorized("user not found");
                }

                var role = await roleClaim.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssuesPaginated(issueType, complexity, progress, page, itemPerPage, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("={organizationId}/group={groupId}/project={projectId}/default")]
        public async Task<IActionResult> GetProjectIssue(Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return Unauthorized("User does not exist");
                }

                var role = await roleClaim.GetGroupRoleByEmail(user.Value, organizationId, groupId);

                if (role == null || role.Name == ApplicationRoleNames.OrganizationMember)
                {
                    return Forbid("You can't perform this action");
                }

                return Ok(await issue.GetIssue(projectId, user.Value));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("={organizationId}/group={groupId}/project={projectId}/issue={issueId}")]
        public async Task<IActionResult> GetIssueById(Guid projectId, Guid issueId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await issue.GetIssueById(projectId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("={organizationId}/group={groupId}/parentId={parentIssueId}")]
        public async Task<IActionResult> GetSubIssues(Guid projectId, Guid parentIssueId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await issue.GetSubIssues(parentIssueId, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("={organizationId}/group={groupId}/project={projectId}/issues")]
        public async Task<IActionResult> GetIssueAndChild(Guid projectId, Guid groupId, Guid organizationId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await issue.GetIssueAndChild(projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
