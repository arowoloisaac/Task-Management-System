using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.IssueDto;
using Project_Manager.Enum;
using Project_Manager.Service.IssueService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/issue")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IssueService _issueService;

        public IssueController(IssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]
        [Route("ceate-issue/{projectId}")]
        public async Task<IActionResult> CreateIssue([Required]Guid projectId, CreateIssue createIssue)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User is not found");
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
        [Route("project={projectId}/{parentIssueId}/create-subIssue")]
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

        [HttpDelete]
        [Route("{issueId}/delete")]
        public async Task<IActionResult> DeleteIssue(Guid issueId, [Required] bool isDeleteChildren)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.DeleteIssues(issueId, isDeleteChildren, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{issueId}/update")]
        public async Task<IActionResult> UpdateIssue(Guid issueId, string? name, string? description, Complexity? complexity, 
            uint? estimatedTimeInMinute, uint timeSpent, int issueLevel)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _issueService.UpdateIssues(issueId, name, description, complexity,estimatedTimeInMinute, timeSpent, issueLevel,user.Value));
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
        [Route("yo")]
        public async Task<IActionResult> GetProjectIssus(Guid projectId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                return Ok(await _issueService.GetIssue(projectId));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
