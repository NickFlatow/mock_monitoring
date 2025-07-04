using Microsoft.AspNetCore.Mvc;
using mock_monitoring.Models;

namespace mock_monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly MonitoringDbContext _dbContext;
        public SensorController(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}