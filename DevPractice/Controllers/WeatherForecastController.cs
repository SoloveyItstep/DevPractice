using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DevPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private SqliteConnection connection;
    

    public WeatherForecastController()
    {
        connection = new SqliteConnection("Data Source=identifier.sqlite");
        connection.Open();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> SelectFromDBAndShow()
    {
        List<dynamic> Summaries = new List<dynamic>();
        Console.WriteLine("Get started");
        SqliteCommand command = new SqliteCommand("select * from table_name", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            dynamic name = reader["name"];
            Summaries.Add(name);
            Console.WriteLine($"Verbose:{name}");
        }
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.ToArray().Length)]
            })
            .ToArray();
        Console.WriteLine("Get ended");
    }
}