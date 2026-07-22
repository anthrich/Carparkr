namespace Carparkr.Domain.Tests;

public class WhenAllocatingASpace
{
    private readonly CarPark _carPark = new();
    
    [Fact]
    public void It_increments_the_number_of_full_spaces()
    {
        // Act
        _carPark.AllocateSpace("RA73 XRF", DateTime.UtcNow);
        
        // Assert
        Assert.Equal(1, _carPark.GetSpaceSummary().FullSpaces);
    }

    [Fact]
    public void It_decrements_the_number_of_available_spaces()
    {
        // Act
        _carPark.AllocateSpace("RA73 XRF", DateTime.UtcNow);
        
        // Assert
        Assert.Equal(99, _carPark.GetSpaceSummary().AvailableSpaces);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99)]
    public void It_returns_the_space_number(int fullSpaces)
    {
        // Arrange
        for (var i = 0; i < fullSpaces; i++)
        {
            _carPark.AllocateSpace("RO29 NU" + i, DateTime.UtcNow);
        }
        
        // Act
        var result = _carPark.AllocateSpace("RA73 XRF", DateTime.UtcNow);
        
        // Assert
        Assert.Equal(fullSpaces, result.Value.SpaceNumber);
    }

    [Fact]
    public void It_fails_when_spaces_are_full()
    {
        // Arrange
        for (var i = 0; i < 100; i++)
        {
            _carPark.AllocateSpace("RO29 NU" + i, DateTime.UtcNow);
        }
        
        // Act
        var result = _carPark.AllocateSpace("RA73 XRF", DateTime.UtcNow);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
}