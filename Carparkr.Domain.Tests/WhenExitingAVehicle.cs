namespace Carparkr.Domain.Tests;

public class WhenExitingAVehicle
{
    private readonly CarPark _carPark = new();

    public WhenExitingAVehicle()
    {
        _carPark.AllocateSpace("RA73 XRF");
    }
    
    [Fact]
    public void It_decrements_the_number_of_full_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF");
        
        // Assert
        Assert.Equal(0, _carPark.GetSpaceSummary().FullSpaces);
    }

    [Fact]
    public void It_increments_the_number_of_available_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF");
        
        // Assert
        Assert.Equal(100, _carPark.GetSpaceSummary().AvailableSpaces);
    }
}