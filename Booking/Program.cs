using Booking.BusinessLogic;
using Booking.DataAcess;
using Booking.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IEventsRepository, EventsRepository>();
builder.Services.AddTransient<IEventsManager, EventsManager>();
builder.Services.AddTransient<IEventsRegistrationManager, EventsRegistrationManager>();
builder.Services.AddTransient<IRegistrationsRepository, RegistrationsRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<BookingContext>(options =>
{
    options.UseInMemoryDatabase("BookingDb");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
