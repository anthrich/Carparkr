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
    
    [Theory]
    [InlineData(59, 0.10)]
    [InlineData(60, 0.20)]
    [InlineData(119, 0.20)]
    [InlineData(120, 0.30)]
    [InlineData(299, 0.50)]
    public void It_sets_the_charge_for_up_to_5_min_parked(int secondsParked, double charge)
    {
        // Act
        var exitResult = _carPark.ExitVehicle("RA73 XRF", _entryDateTime.AddSeconds(secondsParked));
        
        // Assert
        Assert.Equal((decimal)charge, exitResult.Charge);
    }
}