using Hangfire;
using HangfireDemo.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJob;
        private readonly LogJob _logJob;

        public WeatherForecastController(
            IBackgroundJobClient backgroundJob,
            LogJob logJob)
        {
            _backgroundJob = backgroundJob;
            _logJob = logJob;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get([FromHeader] bool? parameter = false)
        {
            try
            {
                if (parameter.Value)
                {
                    throw new Exception("Erro forçado!");
                }
                _backgroundJob.Enqueue(() =>
                    _logJob.LogRequest($"Registro vindo do GET WeatherForecast, Registrado em {DateTime.Now.ToString("dd/MM/yyyy")}", DateTime.Now));

                return Ok(new { Weather = "Sunny 🌞", TemperatureC = 25 });

            }
            catch (Exception ex)
            {
                _backgroundJob.Enqueue(() =>
                    _logJob.LogRequest($"Erro vindo do GET WeatherForecast {ex.Message}, Registrado em {DateTime.Now.ToString("dd/MM/yyyy")}", DateTime.Now));

                return BadRequest(new { Weather = "Erro Bad Request", TemperatureC = 000 });
            }
        }
    }
}
