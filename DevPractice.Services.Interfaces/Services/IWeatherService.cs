using DevPractice.Domain.Core;
using DevPractice.Domain.Core.DTOs;

namespace DevPractice.Services.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<List<WeatherForecastDTO>> GetWeatherForecasts();
        Task Add(string name);
        Task Update(UpdateVerboseDTO verbose);
        Task<WeatherForecastDTO> GetOnDate(DateTime date);
    }
}
