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
    }
}
