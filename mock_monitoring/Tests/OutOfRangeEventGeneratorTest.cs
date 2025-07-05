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
using System.Diagnostics.CodeAnalysis;

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
            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId = 1,
                Status = EventStatus.Normal
            };

            // Arrange
            int sensorId = 1;
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.Process(sensorId);

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
                Status = EventStatus.Normal
            };
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.Process(sensorId);

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
                Status = EventStatus.High,
            };
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            // Act
            await _generator.Process(sensorId);

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

        //DONT create a new event when there is already an outofRange open event for the same sensor
        [Fact]
        public async Task CreateEvent_ExistingOutOfRangeEvent_DoesNotAddNewEvent()
        {
            // Arrange
            int sensorId = 1;

            //add sensorLog to be checked for event close
            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId = sensorId,
                Status = EventStatus.High,
                Quality = Quality.Sensor
            };

            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);

            var existingEvent = new OutOfRangeEvent
            {
                SensorId = sensorId,
                SensorLogId = sensorLog.Id,
                Quality = Quality.Sensor,
                Status = sensorLog.Status,
                Type = EventTypes.OutOfRange,
                Current_Level = EventAlarmLevels.Level1,
                Level_1 = 0,
                Level_2 = 0,
                Level_3 = 0,
                Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                End = 0,
            };



            _eventRepositoryMock.Setup(repo => repo.GetOpenEventsAsync<OutOfRangeEvent>(sensorId, EventTypes.OutOfRange))
                .ReturnsAsync([existingEvent]);


            // Act
            await _generator.Process(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddEventsAsync<Event>(It.IsAny<Event>()), Times.Never());
                

        }

        //todo test escalation of events
        [Fact]
        public async Task ProcessExsistingEvent_EventEscalation_UpdatesEvent()
        {
            var sensorId = 1;

            //add sensorLog to be checked for event close
            var sensorLog = new SensorLog
            {
                Id = 1,
                SensorId = sensorId,
                Status = EventStatus.High,
                Quality = Quality.Sensor
            };

            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(sensorLog);
            // Arrange
            var existingEvent = new OutOfRangeEvent
            {
                Id = 1,
                SensorId = 1,
                SensorLogId = 1,
                Quality = Quality.Sensor,
                Status = EventStatus.High,
                Type = EventTypes.OutOfRange,
                Current_Level = EventAlarmLevels.Level0,
                Level_1 = 0,
                Level_2 = 0,
                Level_3 = 0,
                Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 400,
                End = 0,
            };

            _eventRepositoryMock.Setup(repo => repo.GetOpenEventsAsync<OutOfRangeEvent>(existingEvent.SensorId, EventTypes.OutOfRange))
                .ReturnsAsync([existingEvent]);

            // Act
            await _generator.Process(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.EscalateEventAsync<Event>(It.IsAny<Event>()), Times.Once());
        }
        [Fact]
        public async Task ProcessExsistingEvent_CloseEvent()
        {
            var sensorId = 1;
            // Arrange
            var existingEvent = new OutOfRangeEvent
            {
                Id = 1,
                SensorId = 1,
                SensorLogId = 1,
                Quality = Quality.Sensor,
                Status = EventStatus.High,
                Type = EventTypes.OutOfRange,
                Current_Level = EventAlarmLevels.Level1,
                Level_1 = 0,
                Level_2 = 0,
                Level_3 = 0,
                Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 400,
                End = 0,
            };

            _eventRepositoryMock.Setup(repo => repo.GetOpenEventsAsync<OutOfRangeEvent>(existingEvent.SensorId, EventTypes.OutOfRange))
                .ReturnsAsync([existingEvent]);

            // Simulate sensor back in range (Quality.Good, Status.Normal)
            var backInRangeSensorLog = new SensorLog
            {
                Id = 1,
                SensorId = sensorId,
                Status = EventStatus.Normal,
                Quality = Quality.Good
            };
            _sensorRepositoryMock.Setup(repo => repo.GetLatestSensorLogAsync(sensorId))
                .ReturnsAsync(backInRangeSensorLog);

            // Act
            await _generator.Process(sensorId);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.CloseEventAsync<OutOfRangeEvent>(existingEvent), Times.Once());
            _eventRepositoryMock.Verify(repo => repo.EscalateEventAsync<Event>(It.IsAny<Event>()), Times.Never());
        }
        

        //close an open event if the sensor goes back in range
        //   public async Task ProcessExsistingEvent_EventBackInRange_ClosesEvent()



        public void Dispose()
        {
            // Cleanup if needed (e.g., reset mocks)
            _eventRepositoryMock.Reset();
            _sensorRepositoryMock.Reset();
        }
    }
}