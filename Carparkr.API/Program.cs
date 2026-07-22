using Carparkr;
using Carparkr.Domain;
using Carparkr.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarParkContext>(
    contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton
);
builder.Services.AddTransient<ICarParkRepository, EfCoreCarParkRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create a car park if there are none.
// This is a temporary measure based on the assumption that car parks would be manually created later.
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

app.MapPost("/parking", async (PostParkingModel model, ICarParkRepository carParkRepository) =>
    {
        var carParks = await carParkRepository.Get();
        var carPark = carParks.First();
        var timeStamp = DateTime.UtcNow;
        var entryResult = carPark.AllocateSpace(model.VehicleReg, timeStamp, model.VehicleType);
        return new { model.VehicleReg, TimeIn = timeStamp, entryResult.SpaceNumber };
    })
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
