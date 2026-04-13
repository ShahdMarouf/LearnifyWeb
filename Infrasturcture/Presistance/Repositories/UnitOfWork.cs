using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repositories
{
    public class UnitOfWork(storeDbContext _dbcontext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();
        private IStudentCourseRepository? _studentCourseRepository;
        private IMaterialRepository? _materialRepository;

        public IStudentCourseRepository StudentCourseRepository =>
            _studentCourseRepository ??= new StudentCourseRepository(_dbcontext);

        public IMaterialRepository MaterialRepository =>
            _materialRepository ??= new MaterialRepository(_dbcontext);

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var typename = typeof(T).Name;
            if (_repositories.ContainsKey(typename))
            {
                return (IGenericRepository<T>)_repositories[typename];
            }
            var repo = new GenericRepository<T>(_dbcontext);
            _repositories[typename] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }
    }


}
