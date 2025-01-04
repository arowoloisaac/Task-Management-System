using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        public GroupUserController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToGroup()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromGroup()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveGroupUsers()
        {
            return Ok();
        }
    }
}
    