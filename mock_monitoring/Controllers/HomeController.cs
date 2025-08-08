using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mock_monitoring.Models;

namespace mock_monitoring.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MonitoringDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, MonitoringDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Sensors()
    {
        var sensors = _dbContext.Sensor.ToList();
        return View(sensors);
    }

    public IActionResult AddSensor()
    {
        return View();
    }


    public IActionResult AddSensorPost(string name, string macAddress, string samplePeriod,int type)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(macAddress))
        {
            ModelState.AddModelError("", "Name and MAC Address are required.");
            return View("AddSensor");
        }

        //todo update to sensorRepository
        var sensor = new TemperatureSensor
        {
            Name = name,
            MacAddress = macAddress,
            Sample_Period = int.TryParse(samplePeriod, out var period) ? period : 300,
            ProfileId = 1, // Assuming a default profile ID
            Type = type,
            Enable = true,
        };

        _dbContext.Sensor.Add(sensor);
        _dbContext.SaveChanges();

        return RedirectToAction("Sensors");
    }

    public IActionResult SensorLogs(int? sensorId)
    {
        IEnumerable<SensorLog> logs;

        if (sensorId == null || sensorId <= 0)
        {
            logs = _dbContext.SensorLog.ToList();
        }
        else
        {
            // if id given
            logs = _dbContext.SensorLog.ToList()
                .Where(log => log.SensorId == sensorId)
                .OrderByDescending(log => log.Timestamp)
                .ToList();
        }
        return View(logs);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
