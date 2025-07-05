using Microsoft.AspNetCore.Mvc;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;
using mock_monitoring.Repository;

namespace mock_monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly MonitoringDbContext _dbContext;
        private readonly ISensorRepository _sensorRepository;
        public SensorController(MonitoringDbContext dbContext, ISensorRepository sensorRepository)
        {
            _dbContext = dbContext;
            _sensorRepository = sensorRepository;
        }

        [HttpGet]
        public IActionResult test()
        {
            var data = new { Message = "Hello from the backend!", Timestamp = DateTime.UtcNow };
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetLogs(int id, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            var totalResults = _dbContext.SensorLog.Count(log => log.SensorId == id);
            if (totalResults == 0)
            {
                return NotFound(new { Message = "Sensor not found" });
            }

            var sensorLogs = _dbContext.SensorLog
                .Where(log => log.SensorId == id)
                .OrderByDescending(log => log.Timestamp)
                .Skip(offset)
                .Take(limit)
                .ToList();

            if (sensorLogs == null || !sensorLogs.Any())
            {
                return NotFound(new { Message = "Sensor logs not found" });
            }
            //todo make typed api response
            return Ok(new
            {
                Logs = sensorLogs,
                TotalResults = totalResults,
            });
        }

        [HttpPost("data")]
        public async Task<IActionResult> PostSensorData([FromBody] ISensorData data)
        {
            if (data == null || string.IsNullOrEmpty(data.MAC))
            {
                return BadRequest(new { Message = "Invalid sensor data or missing MAC address." });
            }

            //todo get sensor from mac
            var sensor = await _sensorRepository.GetSensorByMacAsync(data.MAC);
            if (sensor == null)
            {
                return NotFound($"Sensor with MAC address {data.MAC} not found.");
            }

            if (!sensor.Enable)
            {
                return BadRequest(new { Message = $"Sensor {sensor.Name} is disabled." });
            }

            if (data.SD1 != null)
            {
                await _sensorRepository.AddReadingAsync<Sensor>(sensor.Id, data.SD1.Value);
            }
            //todo update for two readings should handle temp and humidity
            // await _sensorRepository.AddReadingAsync<Sensor>(sensor.Id, data.SD2);

            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Data received successfully." });
        }
    
    }
}