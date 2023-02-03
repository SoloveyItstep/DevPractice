using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using System.Linq.Expressions;

namespace DevPracice.Domain.Interfaces.Repositories
{
    public interface ITableNameRepository
    {
        Task<List<Verbose>> GetTableNames();
        Task<List<Verbose>> FindVerboses(Expression<Func<Verbose, bool>> expression);
        Task Add(string verbose);
        Task Update(UpdateVerboseDTO verbose);
        Task Remove(int id);
        Task Remove(Verbose verbose);
    }
}
