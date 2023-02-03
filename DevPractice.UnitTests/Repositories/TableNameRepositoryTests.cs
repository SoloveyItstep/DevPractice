using AutoFixture;
using DevPracice.Domain.Interfaces.DBContexts;
using DevPracice.Domain.Interfaces.Repositories;
using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using DevPractice.Infrastructure.Data.DBContexts;
using DevPractice.Infrastructure.Data.Repositories;
using EntityFrameworkCore.Testing.Moq;
using EntityFrameworkCore.Testing.Moq.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DevPractice.UnitTests.Repositories
{
    [Trait("Category", "Unit")]
    [ExcludeFromCodeCoverage]
    public class TableNameRepositoryTests
    {
        private readonly Mock<ILogger<TableNameRepository>> _loggerMock;
        private readonly ITableNameContext _context;
        private readonly ITableNameRepository _repository;
        private readonly Fixture _fixture;

        public TableNameRepositoryTests()
        {
            this._fixture = new Fixture();

            var options = new DbContextOptionsBuilder<TableNameContext>()
            .UseInMemoryDatabase(databaseName: "table_name")
            .Options;
            _context = Create.MockedDbContextFor<TableNameContext>(options);

            this._loggerMock = new Mock<ILogger<TableNameRepository>>();
            this._repository = new TableNameRepository(_loggerMock.Object, _context);
        }

        [Fact]
        public async Task GetVeboses()
        {
            var tableName = _fixture.Create<List<Verbose>>();
            _context.Verboses.AddRangeToReadOnlySource(tableName);

            var result = await _repository.GetTableNames();

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Collection(result,
                item => Assert.Equal(tableName.First().Name, item.Name),
                item => Assert.Equal(tableName[1].Name, item.Name),
                item => Assert.Equal(tableName.Last().Name, item.Name));

            var varbose = tableName.FirstOrDefault(x => x.ID == tableName.Last().ID);
            Assert.NotNull(varbose);
        }

        [Fact]
        public async Task LoggerExecute()
        {
            var tableName = _fixture.Create<List<Verbose>>();
            _context.Verboses.AddRangeToReadOnlySource(tableName);
            
            List<string> messages = new();
            _loggerMock.SetupMock(messages);

            await _repository.GetTableNames();

            Assert.True(messages.Any());
            Assert.Equal(messages.First(), $"Verbose: {tableName.First().Name}");
        }

        [Fact]
        public async Task ContextThrow()
        {
            var repository = new TableNameRepository(_loggerMock.Object, new TableNameContext());

            List<string> messages = new();
            _loggerMock.SetupMock(messages);

            try
            {
                await repository.GetTableNames();
            }
            catch { }

            Assert.True(messages.Any());
            Assert.Equal("Can't get the data or lorem ipsum", messages.First());
        }

        [Fact]
        public async Task AddItemTest()
        {
            string verboseName = "test";
            await _repository.Add(verboseName);
            var result = await _repository.FindVerboses(x => x.Name == verboseName);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(_context.Verboses.Any());

            await _repository.Remove(result.FirstOrDefault());
        }

        [Fact]
        public async Task AddThrow()
        {
            try
            {
                await _repository.Add(null);
            }
            catch(KeyNotFoundException ex) 
            {
                Assert.Equal("Verbose value is empty", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task Update()
        {
            string verboseName = "test";
            await _repository.Add(verboseName);
            var result = await _repository.FindVerboses(x => x.Name == verboseName);
            var verbose = result.FirstOrDefault();
            string newName = "test updated";

            await _repository.Update(new UpdateVerboseDTO { ID = verbose.ID, Name = newName });
            result = await _repository.FindVerboses(x => x.Name == newName);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(newName, result.FirstOrDefault().Name);

            await _repository.Remove(result.FirstOrDefault());
        }

        [Fact]
        public async Task UpdateThrows()
        {
            try
            {
                await _repository.Update(null);
            }
            catch(KeyNotFoundException ex)
            {
                Assert.Equal("request model empty", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task UpdateThrowsNotFound()
        {
            try
            {
                await _repository.Update(new UpdateVerboseDTO { ID = -1, Name = "test" });
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("Verbose not found: -1", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task Remove_FirstMethos()
        {
            var verboseNames = _fixture.CreateMany<string>(1);
            var verboseName = verboseNames.First();
            await _repository.Add(verboseName);
            var result = await _repository.FindVerboses(x => x.Name== verboseName);

            Assert.NotEmpty(result);

            await _repository.Remove(result.First().ID);
            result = await _repository.FindVerboses(x => x.Name == verboseName);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Remove_SecondMethos()
        {
            var verboseNames = _fixture.CreateMany<string>(1);
            var verboseName = verboseNames.First();
            await _repository.Add(verboseName);
            var result = await _repository.FindVerboses(x => x.Name == verboseName);

            Assert.NotEmpty(result);

            await _repository.Remove(result.First());
            result = await _repository.FindVerboses(x => x.Name == verboseName);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RemoveThrows()
        {
            try
            {
                await _repository.Remove(null);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("Verbose not found", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }

    }
}
