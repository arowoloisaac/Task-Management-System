using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Service.IssueAnalyserService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IIssueAnalyserService analyser;

        public TimelineController(IIssueAnalyserService service)
        {
            this.analyser = service;
        }

        [HttpGet]
        [Route("project={projectId}/timeline")]
        public async Task<IActionResult> ProjectTimeline(Guid projectId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await analyser.GetAnalyses(projectId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("project={projectId}/issue={issueId}/timeline")]
        public async Task<IActionResult> IssueTimeline(Guid projectId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(await analyser.GetIssueAnalyses(projectId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
