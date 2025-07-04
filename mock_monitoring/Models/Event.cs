using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace mock_monitoring.Models;

public abstract class Event
{
    [Key]
    public int Id { get; set; }


    [Column("SensorId")]
    [Required]
    public int SensorId { get; set; }

    [ForeignKey("SensorId")]
    public virtual Sensor Sensor { get; set; } = null!; // Navigation property to Sensor



    [Column("SensorLogId")]
    [Required]
    public int SensorLogId { get; set; } // Foreign key to SensorLog

    [ForeignKey("SensorLogId")]
    public virtual SensorLog SensorLog { get; set; } = null!; // Navigation property to SensorLog

    [Required]
    public int Quality { get; set; }
    [Required]
    public int Status { get; set; }

    [Required]
    public int Type { get; set; } = 0;

    [Required]
    public int Level_1 { get; set; } = 0; // timestamp for first level
    [Required]
    public int Level_2 { get; set; } = 0; // timestamp for second level
    [Required]
    public int Level_3 { get; set; } = 0; // timestamp for third level

    [Required]
    public int Start { get; set; } = 0; // timestamp for start of event

    [Required]
    public int End { get; set; } = 0; // timestamp for end of event


    // abstract public Task GetOpenEventAsync(int sensorId, int sensorLogId);

    // abstract public void CreateEvent(SensorLog sensorLog);



}