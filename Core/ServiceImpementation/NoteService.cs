using Domain.Contracts;
using Domain.Models;
using ServiceAbstarction;
using Shared.DTOs.NoteDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class NoteService(INoteRepository noteRepository, IStudentRepository studentRepository) : INoteService
    {
        public async Task<List<NoteDto>> GetStudentNotesAsync(string academicId)
        {
            if (!int.TryParse(academicId, out int studentId)) return new List<NoteDto>();

            var student = await studentRepository.GetByAcademicIdAsync(studentId);
            if (student == null) return new List<NoteDto>();

            var notes = await noteRepository.GetNotesByStudentIdAsync(student.Id);

            return notes.Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task<NoteDto?> AddNoteAsync(string academicId, CreateNoteDto dto)
        {
            if (!int.TryParse(academicId, out int studentId)) return null;

            var student = await studentRepository.GetByAcademicIdAsync(studentId);
            if (student == null) return null;

            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                StudentId = student.Id
            };

            await noteRepository.AddAsync(note);

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<bool> UpdateNoteAsync(int noteId, string academicId, UpdateNoteDto dto)
        {
            if (!int.TryParse(academicId, out int studentId)) return false;

            var student = await studentRepository.GetByAcademicIdAsync(studentId);
            if (student == null) return false;

            var note = await noteRepository.GetByIdAsync(noteId);
            if (note == null) return false;

            if (note.StudentId != student.Id) return false; // Unauthorized

            note.Title = dto.Title;
            note.Content = dto.Content;

            noteRepository.Update(note);
            return true;
        }

        public async Task<bool> DeleteNoteAsync(int noteId, string academicId)
        {
            if (!int.TryParse(academicId, out int studentId)) return false;

            var student = await studentRepository.GetByAcademicIdAsync(studentId);
            if (student == null) return false;

            var note = await noteRepository.GetByIdAsync(noteId);
            if (note == null) return false;

            if (note.StudentId != student.Id) return false; // Unauthorized

            noteRepository.Delete(note);
            return true;
        }
    }
}
