using DevPractice.Domain.Core.Response;
using System.Diagnostics.CodeAnalysis;

namespace DevPractice.Domain.Core
{
    [ExcludeFromCodeCoverage]
    public class WeatherForecastDTO
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}