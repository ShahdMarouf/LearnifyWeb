using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;

namespace Presistance.Repositories
{
    public class NoteRepository(storeDbContext dbContext) : INoteRepository
    {
        public async Task<List<Note>> GetNotesByStudentIdAsync(int studentId)
        {
            return await dbContext.Set<Note>()
                .Where(n => n.StudentId == studentId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await dbContext.Set<Note>()
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(Note note)
        {
            await dbContext.Set<Note>().AddAsync(note);
            await dbContext.SaveChangesAsync();
        }

        public void Update(Note note)
        {
            dbContext.Set<Note>().Update(note);
            dbContext.SaveChanges();
        }

        public void Delete(Note note)
        {
            dbContext.Set<Note>().Remove(note);
            dbContext.SaveChanges();
        }
    }
}
