using AutoMapper;
using Booking.DataAcess;
using Booking.DTO;

namespace Booking.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, CreateEventRequest>();
            CreateMap<CreateEventRequest, Event>();
            CreateMap<Event, CreateEventResponse>();
            CreateMap<Event, GetEventBasicResponse>();
            CreateMap<Event, GetEventResponse>();
            CreateMap<UpdateEventRequest, Event>();
            CreateMap<Event, UpdateEventResponse>();
            CreateMap<AddRegistrationRequest, EventRegistration>();
            CreateMap<EventRegistration, AddRegistrationResponse>();
        }
    }
}
