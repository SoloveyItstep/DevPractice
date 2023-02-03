using DevPracice.Domain.Interfaces.DBContexts;
using DevPractice.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace DevPractice.Infrastructure.Data.DBContexts
{
    public class TableNameContext: DbContext, ITableNameContext
    {
        public TableNameContext()
        { }

        public TableNameContext(DbContextOptions options)
            :base(options)
        { }

        public virtual DbSet<Verbose> Verboses { get; set; }

        public int Commit()
        {
            return SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return SaveChangesAsync();
        }
    }
}
