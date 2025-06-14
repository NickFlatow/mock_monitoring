using Microsoft.EntityFrameworkCore;

using mock_monitoring.Models;
using mock_monitoring.Mock_Sensor;
using mock_monitoring.Services;
using mock_monitoring.Repository;
using mock_monitoring.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MonitoringDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IEventRepository, EventRepository>(); // Register IEventRepository
builder.Services.AddScoped<ISensorRepository, SensorRepository>(); // Register ISensorRepository
// builder.Services.AddScoped<ISensorService, SensorService>();

builder.Services.AddHostedService<EventGeneratorService>();
builder.Services.AddHostedService<SensorDataGeneratorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
