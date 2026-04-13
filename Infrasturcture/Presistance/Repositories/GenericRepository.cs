using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class GenericRepository<T> (storeDbContext _context): IGenericRepository<T> where T : class
    {

        public async Task AddAsync(T entity) =>
            await _context.Set<T>().AddAsync(entity);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id) =>
            await _context.Set<T>().FindAsync(id);

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public void Remove(T entity) =>
            _context.Set<T>().Remove(entity);
    }
}


