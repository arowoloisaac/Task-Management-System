using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.IssueDto;
using Project_Manager.Enum;
using Project_Manager.Service.IssueService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]
        [Route("project={projectId}/create-issue")]
        public async Task<IActionResult> CreateIssue([Required]Guid projectId, CreateIssue createIssue)
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
                    return Ok(await _issueService.CreateIssues(projectId, createIssue, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("project={projectId}/issue={parentIssueId}/create-subIssue")]
        public async Task<IActionResult> CreateSubIssue(Guid projectId ,CreateIssue issueDto, Guid parentIssueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.CreateSubIssue(projectId, issueDto, parentIssueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //add the project id later
        [HttpDelete] 
        [Route("project={projectId}/issue={issueId}/delete")]
        public async Task<IActionResult> DeleteIssue(Guid issueId, Guid projectId, [Required] bool isDeleteChildren)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.DeleteIssues(issueId,projectId, isDeleteChildren, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //same here
        [HttpPut]
        [Route("project={projectId}/issue={issueId}/update")]
        public async Task<IActionResult> UpdateIssue(UpdateIssueDto dto, Guid projectId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.UpdateIssues(issueId, dto, projectId,user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("projectId={projectId}/issues")]
        public async Task<IActionResult> GetProjectIssues(Guid projectId, [FromQuery]IssueType? issueType, [FromQuery]Complexity? complexity, [FromQuery]Progress? progress)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("user not found");
                }
                return Ok(await _issueService.GetIssues(issueType, complexity, progress, projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("projectId={projectId}/issues/page")]
        public async Task<IActionResult> GetProjectIssuesPaginated(Guid projectId, [FromQuery] IssueType? issueType, [FromQuery] Complexity? complexity, [FromQuery] Progress? progress, int? page, int itemPerPage)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("user not found");
                }
                return Ok(await _issueService.GetIssuesPaginated(issueType, complexity, progress, page, itemPerPage,projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("project={projectId}/default")]
        public async Task<IActionResult> GetProjectIssue(Guid projectId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    throw new Exception("User does not exist");
                }

                return Ok(await _issueService.GetIssue(projectId, user.Value));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("project={projectId}/issue={issueId}")]
        public async Task<IActionResult> GetIssueById(Guid projectId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.GetIssueById(projectId, issueId, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("parentId={parentIssueId}")]
        public async Task<IActionResult> GetSubIssues(Guid projectId, Guid parentIssueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await _issueService.GetSubIssues(parentIssueId, projectId, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("project={projectId}/issues")]
        public async Task<IActionResult> GetIssueAndChild(Guid projectId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await _issueService.GetIssueAndChild( projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
