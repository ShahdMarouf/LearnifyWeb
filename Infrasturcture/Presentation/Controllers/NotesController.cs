using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using Shared.DTOs.NoteDto;
using System.Threading.Tasks;

namespace Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize(Roles = "Student")]
    public class NotesController(INoteService noteService) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMyNotes()
        {
            var academicId = User.FindFirst("StudentAcademicId")?.Value;
            if (string.IsNullOrEmpty(academicId)) return StatusCode(401, "Student ID not found in token.");

            var notes = await noteService.GetStudentNotesAsync(academicId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            var academicId = User.FindFirst("StudentAcademicId")?.Value;
            if (string.IsNullOrEmpty(academicId)) return StatusCode(401, "Student ID not found in token.");

            var createdNote = await noteService.AddNoteAsync(academicId, dto);
            if (createdNote == null) return BadRequest("Could not create note. Student not found.");

            return CreatedAtAction(nameof(GetMyNotes), new { }, createdNote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            var academicId = User.FindFirst("StudentAcademicId")?.Value;
            if (string.IsNullOrEmpty(academicId)) return StatusCode(401, "Student ID not found in token.");

            var success = await noteService.UpdateNoteAsync(id, academicId, dto);
            if (!success) return NotFound("Note not found or you do not have access to it.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var academicId = User.FindFirst("StudentAcademicId")?.Value;
            if (string.IsNullOrEmpty(academicId)) return StatusCode(401, "Student ID not found in token.");

            var success = await noteService.DeleteNoteAsync(id, academicId);
            if (!success) return NotFound("Note not found or you do not have access to it.");

            return NoContent();
        }
    }
}
