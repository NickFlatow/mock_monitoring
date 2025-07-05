using Microsoft.EntityFrameworkCore;

using mock_monitoring.Models;
using mock_monitoring.Mock_Sensor;
using mock_monitoring.Services;
using mock_monitoring.Repository;
using mock_monitoring.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// // Add services to the container.
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Add this line

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin() // Allow any origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


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

builder.Services.AddHostedService<SensorDataGeneratorService>();
builder.Services.AddHostedService<EventGeneratorService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseSwagger(); // Add this line
    //   app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    // {
    //     options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //     options.RoutePrefix = string.Empty;
    // });
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowFrontend"); // Use the CORS policy

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
