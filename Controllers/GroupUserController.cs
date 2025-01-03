using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    [Route("api/org")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        public GroupUserController()
        {
            
        }

        public async Task<IActionResult> AddUserToGroup()
        {
            return Ok();
        }

        public async Task<IActionResult> RemoveUserFromGroup()
        {
            return Ok();
        }

        public async Task<IActionResult> RetrieveGroupUsers()
        {
            return Ok();
        }
    }
}
    