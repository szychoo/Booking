using AutoMapper;
using Booking.DataAcess;
using Booking.DTO;
using Moq;

namespace Booking.BusinessLogic.UnitTests
{
    public class EventsRegistrationManagerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EventsRegistrationManager _eventsRegistrationManager;

        public EventsRegistrationManagerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _eventsRegistrationManager = new EventsRegistrationManager(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddEventRegistration_EventNotFound_ReturnsError()
        {
            // Arrange
            var request = new AddRegistrationRequest { EventId = 1 };
            _mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(request.EventId)).ReturnsAsync((Event)null);

            // Act
            var result = await _eventsRegistrationManager.AddEventRegistration(request);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal("Event not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddEventRegistration_ExistingRegistration_ReturnsError()
        {
            // Arrange
            var request = new AddRegistrationRequest { EventId = 1, UserEmail = "test@example.com" };
            var eventItem = new Event { Id = 1 };
            var registration = new EventRegistration { UserEmail = "test@example.com" };
            _mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(request.EventId)).ReturnsAsync(eventItem);
            _mockUnitOfWork.Setup(u => u.RegistrationsRepository.GetAsync(It.IsAny<EventRegistration>())).ReturnsAsync(registration);

            // Act
            var result = await _eventsRegistrationManager.AddEventRegistration(request);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"User {request.UserEmail} already registered for event {request.EventId}", result.ErrorMessage);
        }

        [Fact]
        public async Task GetEventRegistration_EventRegistrationNotFound_ReturnsNull()
        {
            // Arrange
            var request = new GetRegistrationRequest { EventId = 1, Id = 1 };
            _mockUnitOfWork.Setup(u => u.RegistrationsRepository.GetByEventIdAsync(request.EventId)).ReturnsAsync(Enumerable.Empty<EventRegistration>());

            // Act
            var result = await _eventsRegistrationManager.GetEventRegistration(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEventRegistration_ValidRequest_ReturnsRegistration()
        {
            // Arrange
            var request = new GetRegistrationRequest { EventId = 1, Id = 1 };
            var registrations = new List<EventRegistration> { new EventRegistration { Id = 1, UserEmail = "test@example.com" } };
            _mockUnitOfWork.Setup(u => u.RegistrationsRepository.GetByEventIdAsync(request.EventId)).ReturnsAsync(registrations);
            _mockMapper.Setup(m => m.Map<AddRegistrationResponse>(It.IsAny<EventRegistration>())).Returns(new AddRegistrationResponse { UserEmail = "test@example.com" });

            // Act
            var result = await _eventsRegistrationManager.GetEventRegistration(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.UserEmail);
        }
    }
}
