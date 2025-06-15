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
    public class OutOfRangeEventGeneratorTests : IDisposable
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ISensorRepository> _sensorRepositoryMock;
        private readonly OutOfRangeEventGenerator _generator;

        public OutOfRangeEventGeneratorTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _sensorRepositoryMock = new Mock<ISensorRepository>();
            _generator = new OutOfRangeEventGenerator(_eventRepositoryMock.Object, _sensorRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateEvent_NoSensorLog_DoesNotAddEvent()
        {
            // Arrange
            int sensorId = 1;
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync((SensorLog)null);

            // Act
            await _generator.CreateEvent(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.IsAny<Event>()), Times.Never());
        }

        [Fact]
        public async Task CreateEvent_SensorLogStatusNormal_DoesNotAddEvent()
        {
            // Arrange
            int sensorId = 1;
            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId = sensorId,
                Status = Status.Normal
            };
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.CreateEvent(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.IsAny<Event>()), Times.Never());
        }

        [Fact]
        public async Task CreateEvent_SensorLogStatusError_AddsOutOfRangeEvent()
        {
            // Arrange
            int sensorId = 1;
            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId = sensorId,
                Status = Status.High,
            };
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.CreateEvent(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.Is<OutOfRangeEvent>(e =>
                e.SensorId == sensorLog.SensorId &&
                e.SensorLogId == sensorLog.Id &&
                e.Status == sensorLog.Status &&
                e.Type == EventTypes.OutOfRange &&
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