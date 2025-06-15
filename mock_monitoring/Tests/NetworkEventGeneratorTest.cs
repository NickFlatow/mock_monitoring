using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using mock_monitoring.Lib.EventGenerators;
using mock_monitoring.Lib.Events;
using mock_monitoring.Repository;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;
using mock_monitoring.Types;

namespace mock_monitoring.Tests
{
    public class NetworkEventGeneratorTests : IDisposable
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ISensorRepository> _sensorRepositoryMock;
        private readonly NetworkEventGenerator _generator;

        public NetworkEventGeneratorTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _sensorRepositoryMock = new Mock<ISensorRepository>();
            _generator = new NetworkEventGenerator(_eventRepositoryMock.Object, _sensorRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateEvent_NoNetworkEvent()
        {
            // Arrange

            var sensor = new TemperatureSensor
            {
                Id = 1,
                Enable = true,
                Sample_Period = 600 // Sample period in seconds
            };


            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId =  sensor.Id,
                Status = Status.Normal,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 300
            };

            
            _sensorRepositoryMock.Setup(repo => repo.GetSensorAsync<Sensor>(sensor.Id))
                .ReturnsAsync(sensor);
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensor.Id))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.CreateEvent(sensor.Id);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.IsAny<Event>()), Times.Never());
        }

        [Fact]
        public async Task CreateEvent_NetworkEvent_NoResponseFromNode()
        {
            // Arrange
            var sensor = new TemperatureSensor
            {
                Id = 1,
                Enable = true,
                Sample_Period = 600 // Sample period in seconds
            };


            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId =  sensor.Id,
                Status = Status.Normal,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1200
            };
            _sensorRepositoryMock.Setup(repo => repo.GetSensorAsync<Sensor>(sensor.Id))
                .ReturnsAsync(sensor);
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensor.Id))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.CreateEvent(sensor.Id);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.Is<NetworkEvent>(e =>
                e.SensorId == sensorLog.SensorId &&
                e.SensorLogId == sensorLog.Id &&
                e.Status == sensorLog.Status &&
                e.Type == EventTypes.NetworkEvent &&
                e.Quality == 0 &&
                e.Level_1 == 0 &&
                e.Level_2 == 0 &&
                e.Level_3 == 0 &&
                e.End == 0
            )), Times.Once());
        }

        //todo don't create a new evnet when there is already an outofRange open event for the same sensor




        public void Dispose()
        {
            // Cleanup if needed (e.g., reset mocks)
        }
    }
}