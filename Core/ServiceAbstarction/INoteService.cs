using Shared.DTOs.NoteDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface INoteService
    {
        Task<List<NoteDto>> GetStudentNotesAsync(string academicId);
        Task<NoteDto?> AddNoteAsync(string academicId, CreateNoteDto dto);
        Task<bool> UpdateNoteAsync(int noteId, string academicId, UpdateNoteDto dto);
        Task<bool> DeleteNoteAsync(int noteId, string academicId);
    }
}
