namespace Carparkr.Domain.Tests;

public class WhenGettingSpaceSummary
{
    private CarPark _carpark = new();

    [Fact]
    public void It_returns_the_default_of_100_total_spaces()
    {
        // Act
        var summary = _carpark.GetSpaceSummary();
        
        // Assert
        Assert.Equal(100, summary.TotalSpaces);
    }
    
    [Fact]
    public void It_returns_the_default_of_0_full_spaces()
    {
        // Act
        var summary = _carpark.GetSpaceSummary();
        
        // Assert
        Assert.Equal(0, summary.FullSpaces);
    }

    [Fact]
    public void It_returns_the_default_of_100_available_spaces()
    {
        // Act
        var summary = _carpark.GetSpaceSummary();
        
        // Assert
        Assert.Equal(100, summary.AvailableSpaces);
    }
}