using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface INoteRepository
    {
        Task<List<Note>> GetNotesByStudentIdAsync(int studentId);
        Task<Note?> GetByIdAsync(int id);
        Task AddAsync(Note note);
        void Update(Note note);
        void Delete(Note note);
    }
}
