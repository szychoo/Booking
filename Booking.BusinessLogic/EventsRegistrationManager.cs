using AutoMapper;
using Booking.DataAcess;
using Booking.DTO;

namespace Booking.BusinessLogic
{
    public class EventsRegistrationManager : IEventsRegistrationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventsRegistrationManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddRegistrationResponse> AddEventRegistration(AddRegistrationRequest request)
        {
            var eventItem = await _unitOfWork.EventsRepository.GetEventByIdAsync(request.EventId);
            if(eventItem is null)
            {
                return new AddRegistrationResponse { ErrorMessage = "Event not found", HasError = true };
            }

            var registration = _mapper.Map<EventRegistration>(request);
            var existingRegistration = await _unitOfWork.RegistrationsRepository.GetAsync(registration);
            if (existingRegistration != null)
            {
                return new AddRegistrationResponse { ErrorMessage = $"User {request.UserEmail} already registered for event {request.EventId}", HasError = true };
            }

            var response = await _unitOfWork.RegistrationsRepository.AddAsync(registration);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<AddRegistrationResponse>(response);
        }

        public async Task<AddRegistrationResponse?> GetEventRegistration(GetRegistrationRequest request)
        {
            var eventRegistrations = await _unitOfWork.RegistrationsRepository.GetByEventIdAsync(request.EventId);
            if(!eventRegistrations.Any())
            {
                return null;
            }

            var registration = eventRegistrations.FirstOrDefault(x => x.Id == request.Id);

            if(registration is null)
            {
                return null;
            }

            return _mapper.Map<AddRegistrationResponse>(registration);
        }
    }
}
