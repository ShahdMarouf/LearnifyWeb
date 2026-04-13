using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        IStudentCourseRepository StudentCourseRepository { get; }
        IMaterialRepository MaterialRepository { get; }
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
