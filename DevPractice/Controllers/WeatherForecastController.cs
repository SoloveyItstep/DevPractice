using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;
using DevPractice.Domain.Core.Response;
using DevPractice.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService service)
        {
            _logger = logger;
            _weatherService = service;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public async Task<Response<List<WeatherForecastDTO>>> Get()
        {
            _logger.LogInformation("Get started");

            var result = await _weatherService.GetWeatherForecasts();

            _logger.LogInformation("Get ended");

            return new Response<List<WeatherForecastDTO>> (result);
        }

        [HttpGet]
        [Route("GetWeatherForecastOnDate")]
        public async Task<Response<WeatherForecastDTO>> Get(DateTime date)
        {
            _logger.LogInformation("Get on date started");

            var result = await _weatherService.GetOnDate(date);

            _logger.LogInformation("Get on date ended");

            return new Response<WeatherForecastDTO>(result);
        }

        [HttpPost]
        [Route("AddVerbose")]
        public async Task AddVerbose(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogError("Argument is null or empty: Add weather forecast");
                throw new ArgumentNullException("name", "Name value is null or empty");
            }

            _logger.LogInformation("Add started");
            await _weatherService.Add(name);
            _logger.LogInformation("Add ended");
        }

        [HttpPut]
        [Route("UpdateVerbose")]
        public async Task UpdateVerbose(UpdateVerboseDTO verbose)
        {
            if(verbose == null || string.IsNullOrEmpty(verbose.Name)) 
            {
                _logger.LogError("Argument is null or empty: Update varbose");
                throw new ArgumentNullException("verbose", "Update mpodel is null or Name value is empty");
            }

            _logger.LogInformation("Update started");
            await _weatherService.Update(verbose);
            _logger.LogInformation("Update ended");
        }
    }
}