using Carparkr;
using Carparkr.Domain;
using Carparkr.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarParkContext>();
builder.Services.AddTransient<ICarParkRepository, EfCoreCarParkRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var carParkRepository = services.GetRequiredService<ICarParkRepository>();
    var carParks = await carParkRepository.Get();
    if (carParks.Count == 0) await carParkRepository.Save(new CarPark());
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/parking", ([FromBody]PostParkingModel model) => new { model.VehicleReg, TimeIn = DateTime.UtcNow })
    .WithName("PostParking")
    .WithOpenApi();

app.MapGet("/parking", async (ICarParkRepository carParkRepository) =>
    {
        var carParks = await carParkRepository.Get();
        var summary = carParks.First().GetSpaceSummary();
        var spaces = new { summary.AvailableSpaces, OccupiedSpaces = summary.FullSpaces };
        return spaces;
    })
    .WithName("GetParking")
    .WithOpenApi();

app.Run();

public partial class Program;
