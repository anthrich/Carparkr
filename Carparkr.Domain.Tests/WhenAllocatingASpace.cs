namespace Carparkr.Domain.Tests;

public class WhenAllocatingASpace
{
    private readonly CarPark _carPark = new();
    
    [Fact]
    public void It_increments_the_number_of_full_spaces()
    {
        // Act
        _carPark.AllocateSpace("RA73 XRF");
        
        // Assert
        Assert.Equal(1, _carPark.GetSpaceSummary().FullSpaces);
    }

    [Fact]
    public void It_decrements_the_number_of_available_spaces()
    {
        // Act
        _carPark.AllocateSpace("RA73 XRF");
        
        // Assert
        Assert.Equal(99, _carPark.GetSpaceSummary().AvailableSpaces);
    }
}