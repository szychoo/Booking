using AutoMapper;
using Booking.DataAcess;
using Booking.DTO;
using Moq;
using System.Linq.Expressions;

namespace Booking.BusinessLogic.UnitTests
{
    public class EventsManagerTests
    {
        [Fact]
        public async Task AddEvent_WithExistingEventName_ReturnsError()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, mockMapper.Object);
            var existingEvent = new Event { Name = "Existing Event" };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingEvent);
            var newEventRequest = new CreateEventRequest { Name = "Existing Event" };

            // Act
            var response = await eventsManager.AddEvent(newEventRequest);

            // Assert
            Assert.True(response.HasError);
            Assert.Equal("Event name already exists", response.ErrorMessage);
        }

        [Fact]
        public async Task AddEvent_WithNewEventName_SuccessfullyAddsEvent()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, mockMapper.Object);
            var newEventRequest = new CreateEventRequest { Name = "New Event", Country = "New Country" };
            var newEvent = new Event { Name = "New Event", Country = "New Country" };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Event)null);
            mockUnitOfWork.Setup(u => u.EventsRepository.AddEventAsync(It.IsAny<Event>())).ReturnsAsync(newEvent);
            mockMapper.Setup(m => m.Map<Event>(It.IsAny<CreateEventRequest>())).Returns(newEvent);
            mockMapper.Setup(m => m.Map<CreateEventResponse>(It.IsAny<Event>())).Returns(new CreateEventResponse { HasError = false });

            // Act
            var response = await eventsManager.AddEvent(newEventRequest);

            // Assert
            Assert.False(response.HasError);
        }

        [Fact]
        public async Task DeleteEvent_WithNonExistentEvent_ReturnsError()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, new Mock<IMapper>().Object);
            mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync((Event)null);

            // Act
            var response = await eventsManager.DeleteEvent(1);

            // Assert
            Assert.True(response.HasError);
            Assert.Equal("Event not found", response.ErrorMessage);
        }

        [Fact]
        public async Task DeleteEvent_WithExistingEvent_SuccessfullyDeletesEvent_AndRegistrations()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, new Mock<IMapper>().Object);
            var existingEvent = new Event { Id = 1, Name = "Existing Event" };
            var existingRegistration = new EventRegistration { Id = 1, EventId = 1 };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(1)).ReturnsAsync(existingEvent);
            mockUnitOfWork.Setup(u => u.RegistrationsRepository.GetByEventIdAsync(1)).ReturnsAsync(new List<EventRegistration> { existingRegistration });

            // Act
            var response = await eventsManager.DeleteEvent(1);

            // Assert
            Assert.False(response.HasError);
            mockUnitOfWork.Verify(u => u.EventsRepository.DeleteEventAsync(It.IsAny<int>()), Times.Once);
            mockUnitOfWork.Verify(u => u.RegistrationsRepository.DeleteAsync(It.IsAny<int>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetEventById_WithValidId_ReturnsEvent()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, mockMapper.Object);
            var @event = new Event { Id = 1, Name = "Test Event" };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(1)).ReturnsAsync(@event);
            mockMapper.Setup(m => m.Map<GetEventResponse>(It.IsAny<Event>())).Returns(new GetEventResponse { Name = "Test Event" });

            // Act
            var response = await eventsManager.GetEventById(1);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Test Event", response.Name);
        }

        [Fact]
        public async Task UpdateEvent_WithNonExistentEvent_ReturnsError()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, new Mock<IMapper>().Object);
            mockUnitOfWork.Setup(u => u.EventsRepository.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync((Event)null);
            var updateEventRequest = new UpdateEventRequest { Id = 1 };

            // Act
            var response = await eventsManager.UpdateEvent(updateEventRequest);

            // Assert
            Assert.True(response.HasError);
            Assert.Equal("Event not found", response.ErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Country1")]
        public async Task GetAllEvents_WithOrWithoutCountry_ReturnsEvents(string country)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, mockMapper.Object);
            var events = new List<Event> { new Event { Country = country } };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetAllEventsAsync(It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(events);
            mockMapper.Setup(m => m.Map<IEnumerable<GetEventBasicResponse>>(It.IsAny<IEnumerable<Event>>())).Returns(new List<GetEventBasicResponse> { new GetEventBasicResponse() });

            // Act
            var response = await eventsManager.GetAllEvents(country);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response);
        }

        [Fact]
        public async Task GetAllEventsAsync_WithSpecificPredicate_ReturnsFilteredEvents()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var eventsManager = new EventsManager(mockUnitOfWork.Object, mockMapper.Object);
            var events = new List<Event> { new Event { Country = "Specific Country" } };
            mockUnitOfWork.Setup(u => u.EventsRepository.GetAllEventsAsync(It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(events);
            mockMapper.Setup(m => m.Map<IEnumerable<GetEventBasicResponse>>(It.IsAny<IEnumerable<Event>>())).Returns(events.Select(x => new GetEventBasicResponse { Country = x.Country }).ToList());

            // Act
            var response = await eventsManager.GetAllEvents("Specific Country");

            // Assert
            Assert.NotNull(response);
            Assert.Single(response);
            Assert.Equal("Specific Country", response.First().Country);
        }
    }
}