using Microsoft.EntityFrameworkCore;
using mock_monitoring.Lib.Events;

namespace mock_monitoring.Models;

public class MonitoringDbContext : DbContext
{

    public DbSet<Sensor> Sensor { get; set; } = null!;
    public DbSet<SensorLog> SensorLog { get; set; } = null!;

    public DbSet<Event> Event { get; set; } = null!;

    public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options)
        : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // TPH Sensor inheritance mapping
        modelBuilder.Entity<Sensor>()
                .ToTable("Sensor")
                .HasDiscriminator<int>("Type")
                .HasValue<TemperatureSensor>(104);
        // .HasValue<TempHumiditySensor>(113);

        modelBuilder.Entity<Event>()
                .ToTable("Event")
                .HasDiscriminator<int>("Type")
                .HasValue<OutOfRangeEvent>(1);



        // Define foreign key relationships
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Sensor)
            .WithMany()
            .HasForeignKey(e => e.SensorId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.SensorLog)
            .WithMany()
            .HasForeignKey(e => e.SensorLogId)
            .OnDelete(DeleteBehavior.ClientNoAction);


        base.OnModelCreating(modelBuilder);
        //seed mock temperature sensor
        modelBuilder.Entity<TemperatureSensor>().HasData(
            new TemperatureSensor()
            {
                Id = 1,
                Name = "Mock Sensor 1",
                MacAddress = "00:11:22:33:44:55",
                ProfileId = 1,
                Sample_Period = 900,
                Enable = true,
                Type = 104,
                CreatedAt = DateTime.UtcNow,
                Alarmen = 0
            }
        );
    }
}