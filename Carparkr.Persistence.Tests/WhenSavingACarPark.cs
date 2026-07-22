using Carparkr.Domain;

namespace Carparkr.Persistence.Tests;

public class WhenSavingACarPark
{
    private readonly EfCoreCarParkRepository _carparkRepository = new(new CarParkContext());
    
    [Fact]
    public async Task It_is_retrievable()
    {
        // Arrange
        var carPark = new CarPark();
        
        // Act
        await _carparkRepository.Save(carPark);
        
        // Assert
        var carParks = await _carparkRepository.Get();
        Assert.NotEmpty(carParks);
    }

    [Fact]
    public async Task The_parked_vehicles_are_saved()
    {
        // Arrange
        var carPark = new CarPark();
        carPark.AllocateSpace("NU76 JJC", DateTime.UtcNow, Size.Medium);
        
        // Act
        await _carparkRepository.Save(carPark);
        
        // Assert
        var carParks = await _carparkRepository.Get();
        var exitResult = carParks.First().ExitVehicle("NU76 JJC", DateTime.UtcNow.AddMinutes(1));
        Assert.Equal(0.20m, exitResult.Charge);
    }
}