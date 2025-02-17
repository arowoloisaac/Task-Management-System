using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.NoteDto;
using Project_Manager.Service.NoteService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/note")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _note;

        public NoteController(INoteService note)
        {
            _note = note;
        }


        [HttpPost]
        [Route("{projectId}")]
        public async Task<IActionResult> CreateNote(Guid projectId, Guid? issueId, [Required]CreateNoteDto content)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("user not found");
                }

                await _note.CreateNote(content, projectId, issueId, user.Value);
                return Ok("Sucessfully created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet][Route("{Id}")]
        public async Task<IActionResult> GetNoteById(Guid Id)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("user not found");
                }

                return Ok(await _note.GetNote(Id, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("projectId/{projectId}")]
        public async Task<IActionResult> GetNotes(Guid projectId, Guid? issueId = null)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("user not found");
                }

                return Ok(await _note.GetNotes(projectId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateNote(Guid Id, [Required]string content)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("user not found");
                }

                return Ok(await _note.UpdateNote(Id, content, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("user not found");
                }
                await _note.DeleteNote(id, user.Value);
                return Ok("successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
