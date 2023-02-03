using DevPractice.Services.Interfaces.Services;
using DevPracice.Domain.Interfaces.Repositories;
using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;

namespace DevPractice.Infrastructure.Business.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ITableNameRepository _repository;

        public WeatherService(ITableNameRepository repository)
        {
            this._repository = repository;
        }

        public Task Add(string name)
        {
            return _repository.Add(name);
        }

        public async Task<WeatherForecastDTO> GetOnDate(DateTime date)
        {
            var verboses = await _repository.GetTableNames();

            return new WeatherForecastDTO
            {
                Date = date,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = verboses[Random.Shared.Next(verboses.Count)].Name
            };
        }

        public async Task<List<WeatherForecastDTO>> GetWeatherForecasts()
        {
            var verboses = await _repository.GetTableNames();

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecastDTO
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = verboses[Random.Shared.Next(verboses.Count)].Name
            })
            .ToList();

            return result;
        }

        public Task Update(UpdateVerboseDTO verbose)
        {
            return _repository.Update(verbose);
        }
    }
}
