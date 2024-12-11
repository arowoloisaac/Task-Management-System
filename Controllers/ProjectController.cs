using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.ProjectService;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/project")]
    [ApiController]
    [EnableCors]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProject(CreateDto projectDto)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound($"User not found");
                }

                return Ok(await _projectService.CreateProject(projectDto, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("delete/{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
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
                    return Ok(await _projectService.DeleteProject(projectId, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{projectId}")]
        public async Task<IActionResult> GetProjectById(Guid projectId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Email);

                if(user == null)
                {
                    return NotFound("User not found");
                }
                else
                {
                    return Ok(await _projectService.GetProjectById(projectId, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetUserProjects([FromQuery] Progress? progress, [FromQuery] Complexity? complexity)
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
                    return Ok(await _projectService.GetProjects(progress, complexity, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("get")]
        [SwaggerOperation(Summary = "Get the list of projects")]
        public async Task<IActionResult> GetPaginated([FromQuery] Progress? progress, [FromQuery] Complexity? complexity, [FromQuery] int? page)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null) {
                    return NotFound("User not found");
                }

                else
                {
                    var projects = await _projectService.GetProjectPaginated(progress, complexity, page, user.Value);



                    if (projects is not null)
                    {
                        return Ok(projects);
                    }

                    else { return NotFound("Dish not fount"); }

                }
                
            }

            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                var response = new ResponseBody
                {
                    Status = "Error"+ ex.Source,
                    Message = ex.Message,
                };

                return StatusCode(500, response);
            }

        }


        [HttpPut]
        [Route("update/{projectId}")]// user parameters instead
        public async Task<IActionResult> UpdateProject(Guid projectId, string? Name, string? Description, Progress? progress, Complexity? complexity)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("user not found");
                }
                else
                {
                    return Ok(await _projectService.UpdateProject(projectId, Name, Description, progress, complexity, user.Value));
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
