using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.OrganizationProjectDto;
using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;
using Project_Manager.Service.OrganizationProjectService;

namespace Project_Manager.Controllers
{
    [Route("api/organization")]
    [ApiController]
    [Authorize]
    public class OrganizationProjectController : ControllerBase
    {
        private readonly IOrganizationProjectService service;

        public OrganizationProjectController(IOrganizationProjectService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> CreateProject(CreateDto dto, Guid organizationId, Guid? groupId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(dto);
        }

        public async Task<IActionResult> AssignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> UnassignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> UpdateProject(Guid projectId, string? name, string? description, Progress? progress, Complexity? complexity, Guid organizationId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> DeleteProject(Guid projectId, Guid organizationId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> EditProject(Guid projectId, UpdateProjectDto dto, Guid organizationId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetProjectById(Guid projectId, Guid organizationId, Guid? groupId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        //for the admin page
        public async Task<IActionResult> GetProjects(Progress? progress, Complexity? complexity, bool isAssigned, Guid organizationId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetGroupProjects(Guid organizationId, Guid groupId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetProjectPaginated(Progress? progress, Complexity? complexity, int? page, int itemPerPage, Guid organizationId, Guid groupId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

    }
}
