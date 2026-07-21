namespace Carparkr.Domain.Tests;

public class WhenAllocatingASpace
{
    private CarPark _carpark = new();
    
    [Fact]
    public void It_increments_the_number_of_full_spaces()
    {
        // Act
        _carpark.AllocateSpace("RA73 XRF");
        
        // Assert
        Assert.Equal(1, _carpark.GetSpaceSummary().FullSpaces);
    }
}