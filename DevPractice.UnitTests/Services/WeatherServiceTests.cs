using AutoFixture;
using DevPracice.Domain.Interfaces.Repositories;
using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using DevPractice.Infrastructure.Business.Services;
using DevPractice.Services.Interfaces.Services;
using Moq;

namespace DevPractice.UnitTests.Services
{
    [Trait("Category", "Unit")]
    public class WeatherServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ITableNameRepository> _repositoryMock;
        private readonly IWeatherService _weatherService;

        public WeatherServiceTests()
        {
            this._fixture = new Fixture();
            this._repositoryMock= new Mock<ITableNameRepository>();
            this._weatherService = new WeatherService(this._repositoryMock.Object);
        }

        [Fact]
        public async Task WeatherServiceTest()
        {
            var verboses = _fixture.CreateMany<Verbose>(5);
            _repositoryMock.Setup(x => x.GetTableNames())
                .ReturnsAsync(() => verboses.ToList());
            var result = await _weatherService.GetWeatherForecasts();

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(verboses.Any(x => result.First().Summary == x.Name));
        }

        [Fact]
        public async Task Update()
        {
            var verboseDTO = _fixture.Create<UpdateVerboseDTO>();
            bool updateCalled = false;
            _repositoryMock.Setup(x => x.Update(It.IsAny<UpdateVerboseDTO>()))
                .Callback(() => { 
                    updateCalled= true;
                })
                .Returns(Task.CompletedTask);

            await _weatherService.Update(verboseDTO);

            Assert.True(updateCalled);
        }

        [Fact]
        public async Task Add()
        {
            var verboseName = _fixture.Create<string>();
            bool addCalled = false;
            _repositoryMock.Setup(x => x.Add(It.IsAny<string>()))
                .Callback(() => {
                    addCalled = true;
                })
                .Returns(Task.CompletedTask);

            await _weatherService.Add(verboseName);

            Assert.True(addCalled);
        }

        [Fact]
        public async Task GetOnDate()
        {
            var date = _fixture.Create<DateTime>();
            var verboses = _fixture.CreateMany<Verbose>(1);
            _repositoryMock.Setup(x => x.GetTableNames())
                .ReturnsAsync(() => verboses.ToList());

            var result = await _weatherService.GetOnDate(date);

            Assert.NotNull(result);
            Assert.Equal(date, result.Date);
            Assert.Equal(verboses.First().Name, result.Summary);
        }
    }
}
