using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Service.OrganizationService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        [Route("create-organization")]
        public async Task<IActionResult> CreateOrganization([FromQuery] CreateOrganizationDto organizationDto)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Email);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(await _organizationService.CreateOrganization(organizationDto, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                else
                {
                    return Ok(await _organizationService.DeleteOrganization(id, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
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
                    return Ok(await _organizationService.GetOrganization(id, user.Value));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
