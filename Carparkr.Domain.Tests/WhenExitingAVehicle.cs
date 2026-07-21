namespace Carparkr.Domain.Tests;

public class WhenExitingAVehicle
{
    private readonly CarPark _carPark = new();
    private readonly DateTime _entryDateTime = new(2026, 07, 21, 20, 38, 01);

    public WhenExitingAVehicle()
    {
        _carPark.AllocateSpace("RA73 XRF", _entryDateTime, Size.Small);
        _carPark.AllocateSpace("RA74 ACB", _entryDateTime, Size.Medium);
        _carPark.AllocateSpace("RA75 LRG", _entryDateTime, Size.Large);
    }
    
    [Fact]
    public void It_decrements_the_number_of_full_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF", _entryDateTime);
        
        // Assert
        Assert.Equal(2, _carPark.GetSpaceSummary().FullSpaces);
    }

    [Fact]
    public void It_increments_the_number_of_available_spaces()
    {
        // Act
        _carPark.ExitVehicle("RA73 XRF", _entryDateTime);
        
        // Assert
        Assert.Equal(98, _carPark.GetSpaceSummary().AvailableSpaces);
    }
    
    [Theory]
    [InlineData(59, 0.10)]
    [InlineData(60, 0.20)]
    [InlineData(119, 0.20)]
    [InlineData(120, 0.30)]
    [InlineData(299, 0.50)]
    public void It_sets_the_charge_for_up_to_5_min_parked_for_small_cars(int secondsParked, double charge)
    {
        // Act
        var exitResult = _carPark.ExitVehicle("RA73 XRF", _entryDateTime.AddSeconds(secondsParked));
        
        // Assert
        Assert.Equal((decimal)charge, exitResult.Charge);
    }
    
    [Theory]
    [InlineData(59, 0.20)]
    [InlineData(60, 0.40)]
    [InlineData(119, 0.40)]
    [InlineData(120, 0.60)]
    [InlineData(299, 1.00)]
    public void It_sets_the_charge_for_up_to_5_min_parked_for_medium_cars(int secondsParked, double charge)
    {
        // Act
        var exitResult = _carPark.ExitVehicle("RA74 ACB", _entryDateTime.AddSeconds(secondsParked));
        
        // Assert
        Assert.Equal((decimal)charge, exitResult.Charge);
    }
}