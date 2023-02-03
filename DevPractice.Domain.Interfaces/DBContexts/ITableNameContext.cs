using DevPractice.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace DevPracice.Domain.Interfaces.DBContexts
{
    public interface ITableNameContext
    {
        DbSet<Verbose> Verboses { get; set; }
        Task<int> CommitAsync();
        int Commit();
    }
}
