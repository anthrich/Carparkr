namespace Carparkr.Domain.Tests;

public class WhenExitingAVehicle
{
    private readonly CarPark _carPark = new();
    private DateTime _entryDateTime = new DateTime(2026, 07, 21, 20, 38, 01);

    public WhenExitingAVehicle()
    {
        _carPark.AllocateSpace("RA73 XRF", _entryDateTime);
    }
    
    [Fact]
    public void It_decrements_the_number_of_full_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF", _entryDateTime);
        
        // Assert
        Assert.Equal(0, _carPark.GetSpaceSummary().FullSpaces);
    }

    [Fact]
    public void It_increments_the_number_of_available_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF", _entryDateTime);
        
        // Assert
        Assert.Equal(100, _carPark.GetSpaceSummary().AvailableSpaces);
    }

    [Fact]
    public void It_sets_the_charge_to_10p_for_up_to_1_min_parked()
    {
        // Act
        var exitResult = _carPark.ExitVehicle("RA73 XRF", _entryDateTime.AddSeconds(59));
        
        // Assert
        Assert.Equal(0.10m, exitResult.Charge);
    }
}