using AutoMapper;
using Booking.DataAcess;
using Booking.DTO;

namespace Booking.BusinessLogic
{
    public class EventsManager : IEventsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventsManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateEventResponse> AddEvent(CreateEventRequest eventItem)
        {
            var @event = await _unitOfWork.EventsRepository.GetByNameAsync(eventItem.Name);
            if (@event != null)
            {
                return new CreateEventResponse { ErrorMessage = "Event name already exists", HasError = true };
            }

            @event = _mapper.Map<Event>(eventItem);
            var response = await _unitOfWork.EventsRepository.AddEventAsync(@event);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CreateEventResponse>(response);
        }

        public async Task<BaseResponse> DeleteEvent(int id)
        {
            var @event = await _unitOfWork.EventsRepository.GetEventByIdAsync(id);
            if (@event is null)
            {
                return new BaseResponse { ErrorMessage = "Event not found", HasError = true };
            }

            await _unitOfWork.EventsRepository.DeleteEventAsync(id);
            var registrations = await _unitOfWork.RegistrationsRepository.GetByEventIdAsync(id);
            foreach (var registration in registrations)
            {
                await _unitOfWork.RegistrationsRepository.DeleteAsync(registration.Id);
            }
            await _unitOfWork.SaveAsync();
            return new BaseResponse(); 
        }

        public async Task<GetEventResponse?> GetEventById(int id)
        {
            var @event = await _unitOfWork.EventsRepository.GetEventByIdAsync(id);
            if(@event is null) return null;

            return _mapper.Map<GetEventResponse>(@event);
        }

        public async Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest updateEventRequest)
        {
            var @event = await _unitOfWork.EventsRepository.GetEventByIdAsync(updateEventRequest.Id);
            if (@event is null)
            {
                return new UpdateEventResponse { ErrorMessage = "Event not found", HasError = true };
            }

            @event = _mapper.Map<Event>(updateEventRequest);
            _unitOfWork.EventsRepository.UpdateEventAsync(@event);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UpdateEventResponse>(@event);
        }

        public async Task<IEnumerable<GetEventBasicResponse>> GetAllEvents(string? country)
        {
            List<Event> events = null;

            if(country is null)
            {
                events = await _unitOfWork.EventsRepository.GetAllEventsAsync();
                return _mapper.Map<IEnumerable<GetEventBasicResponse>>(events);
            }

            events = await _unitOfWork.EventsRepository.GetAllEventsAsync(x => x.Country == country);
            return _mapper.Map<IEnumerable<GetEventBasicResponse>>(events);
            
        }
    }
}
