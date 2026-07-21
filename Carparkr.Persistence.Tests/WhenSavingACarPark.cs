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
}