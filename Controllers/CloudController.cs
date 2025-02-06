using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.ExternalServices.CloudSetting;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CloudController : ControllerBase
    {
        private readonly ICloudService _s3Service;

        public CloudController(ICloudService cloudService)
        {
            _s3Service = cloudService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListObjects()
        {
            try
            {
                var objects = await _s3Service.GetObjects();
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
