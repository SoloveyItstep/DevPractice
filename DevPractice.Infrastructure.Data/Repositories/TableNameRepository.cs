using DevPracice.Domain.Interfaces.DBContexts;
using DevPracice.Domain.Interfaces.Repositories;
using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DevPractice.Infrastructure.Data.Repositories
{
    public class TableNameRepository : ITableNameRepository
    {
        private readonly ILogger<TableNameRepository> _logger;
        private readonly ITableNameContext _tableNameContext;
        private const string connectionErrorMessage = "Can't get the data or lorem ipsum";

        public TableNameRepository(ILogger<TableNameRepository> logger, ITableNameContext context)
        {
            this._logger = logger;
            this._tableNameContext = context;
        }

        public async Task<List<Verbose>> GetTableNames()
        {
            List<Verbose> result = new();

            try
            {
                result = await _tableNameContext.Verboses
                    .AsNoTracking().ToListAsync().ConfigureAwait(false);

                foreach (var verbose in result)
                {
                    _logger.LogInformation($"Verbose: {verbose.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(connectionErrorMessage, ex);
                throw;
            }

            return result;
        }

        public async Task Add(string verbose)
        {
            if (string.IsNullOrEmpty(verbose))
            {
                var ex = new KeyNotFoundException("Verbose value is empty");
                _logger.LogError(connectionErrorMessage, ex);
                throw ex;
            }

            await _tableNameContext.Verboses.AddAsync(new Verbose { Name = verbose } ).ConfigureAwait(false);
            await _tableNameContext.CommitAsync().ConfigureAwait(false);
        }

        public async Task Update(UpdateVerboseDTO verbose)
        {
            if(verbose == null || string.IsNullOrEmpty(verbose.Name))
            {
                var ex = new KeyNotFoundException($"request model empty");
                _logger.LogError(connectionErrorMessage, ex);
                throw ex;
            }

            var verboseToUpdate = await _tableNameContext.Verboses.FirstOrDefaultAsync(x => x.ID == verbose.ID);
            if(verboseToUpdate == null)
            {
                var ex = new KeyNotFoundException($"Verbose not found: {verbose.ID}");
                _logger.LogError(connectionErrorMessage, ex);
                throw ex;
            }

            verboseToUpdate.Name = verbose.Name;
            _tableNameContext.Verboses.Update(verboseToUpdate);

            await _tableNameContext.CommitAsync().ConfigureAwait(false);
        }

        public Task<List<Verbose>> FindVerboses(Expression<Func<Verbose, bool>> expression)
        {
            return _tableNameContext.Verboses.Where(expression).ToListAsync();
        }

        public async Task Remove(int id)
        {
            var verbose = await _tableNameContext.Verboses.FindAsync(id).ConfigureAwait(false);
            await Remove(verbose);
        }

        public async Task Remove(Verbose verbose)
        {
            if (verbose == null)
            {
                var ex = new KeyNotFoundException("Verbose not found");
                _logger.LogError(connectionErrorMessage, ex);
                throw ex;
            }

            _tableNameContext.Verboses.Remove(verbose);
            await _tableNameContext.CommitAsync().ConfigureAwait(false);
        }
    }
}
