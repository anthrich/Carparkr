namespace Carparkr.Domain.Tests;

public class WhenGettingSpaceSummary
{
    [Fact]
    public void It_returns_the_default_of_100_total_spaces()
    {
        // Arrange
        var carpark = new CarPark();
        
        // Act
        var summary = carpark.GetSpaceSummary();
        
        // Assert
        Assert.Equal(100, summary.TotalSpaces);
    }
    
    [Fact]
    public void It_returns_the_default_of_0_full_spaces()
    {
        // Arrange
        var carpark = new CarPark();
        
        // Act
        var summary = carpark.GetSpaceSummary();
        
        // Assert
        Assert.Equal(0, summary.FullSpaces);
    }
}