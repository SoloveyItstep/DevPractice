using AutoFixture;
using DevPractice.Controllers;
using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using DevPractice.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace DevPractice.UnitTests.Controllers
{
    [Trait("Category", "Unit")]
    public class WeatherForecastControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<WeatherForecastController>> _loggerMock;
        private readonly WeatherForecastController _weatherController;

        public WeatherForecastControllerTests()
        {
            this._fixture = new Fixture();
            this._weatherServiceMock = new Mock<IWeatherService>();
            this._loggerMock = new Mock<ILogger<WeatherForecastController>>();
            this._weatherController = new WeatherForecastController(_loggerMock.Object, _weatherServiceMock.Object);
        }

        [Fact]
        public async Task GetFive()
        {
            var expectedDTO = _fixture.Create<List<WeatherForecastDTO>>();
            _weatherServiceMock.Setup(x => x.GetWeatherForecasts()).ReturnsAsync(expectedDTO);

            var result = await _weatherController.Get();

            Assert.NotNull(result);
            Assert.Equal(3, result.Result.Count);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GetOnDate()
        {
            var expected = _fixture.Create<WeatherForecastDTO>();
            _weatherServiceMock.Setup(x => x.GetOnDate(It.IsAny<DateTime>()))
                .ReturnsAsync(expected);

            var result = await _weatherController.Get(expected.Date);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(expected.Date, result.Result.Date);
            Assert.Equal(expected.Summary, result.Result.Summary);
            Assert.Equal(expected.TemperatureF, result.Result.TemperatureF);
            Assert.True(string.IsNullOrEmpty(result.Error));
        }

        [Fact]
        public async Task Add()
        {
            var verboseName = _fixture.Create<string>();
            var addCalled = false;
            _weatherServiceMock.Setup(x => x.Add(It.IsAny<string>()))
                .Callback(()=> { 
                    addCalled = true;
                });

            await _weatherController.AddVerbose(verboseName);
            Assert.True(addCalled);
        }

        [Fact]
        public async Task AddThrow()
        {
            try
            {
                await _weatherController.AddVerbose(null);
            }
            catch(ArgumentNullException ex)
            {
                Assert.Equal("Name value is null or empty (Parameter 'name')", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task Update()
        {
            var dto = _fixture.Create<UpdateVerboseDTO>();
            var updateCalled = false;
            _weatherServiceMock.Setup(x => x.Update(It.IsAny<UpdateVerboseDTO>()))
                .Callback(() => { 
                    updateCalled= true;
                });

            await _weatherController.UpdateVerbose(dto);
            Assert.True(updateCalled);
        }

        [Fact]
        public async Task UpdateThrows()
        {
            try
            {
                await _weatherController.UpdateVerbose(null);
            }
            catch(ArgumentNullException ex)
            {
                Assert.Equal("Update mpodel is null or Name value is empty (Parameter 'verbose')", ex.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }
}
