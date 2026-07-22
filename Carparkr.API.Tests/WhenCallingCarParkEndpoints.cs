using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Carparkr.API.Tests;

public class WhenCallingCarParkEndpoints(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private sealed record ParkingSpaces(int AvailableSpaces, int OccupiedSpaces);
    private sealed record ParkingInfo(string VehicleReg, DateTime TimeIn, int SpaceNumber);

    [Fact]
    public async Task GET_parking_returns_spaces()
    {
        // Act
        var response = await factory.CreateClient().GetAsync("/parking");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var spaces = await response.Content.ReadFromJsonAsync<ParkingSpaces>();
        Assert.Equivalent(new ParkingSpaces(100, 0), spaces);
    }

    [Fact]
    public async Task POST_parking_integrates()
    {
        // Arrange
        var body = GetPostBody("NA74 GGD");
        
        // Act
        var response = await factory.CreateClient().PostAsync("/parking", body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task POST_parking_returns_parking_info_vehicle_reg()
    {
        // Arrange
        var body = GetPostBody("NA74 GGD");

        // Act
        var response = await factory.CreateClient().PostAsync("/parking", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ParkingInfo>();
        Assert.Equal("NA74 GGD", info?.VehicleReg);
    }

    [Fact]
    public async Task POST_parking_returns_time_in()
    {
        // Arrange
        var body = GetPostBody("NA74 GGD");
        
        // Act
        var response = await factory.CreateClient().PostAsync("/parking", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ParkingInfo>();
        Assert.Equal(DateTime.UtcNow, info!.TimeIn, TimeSpan.FromSeconds(1));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(22)]
    public async Task POST_parking_returns_space_number(int fullSpaces)
    {
        // Arrange
        await FillSpaces(fullSpaces);
        var getResponse = await factory.CreateClient().GetAsync("/parking");
        var preParkingSpaces = await getResponse.Content.ReadFromJsonAsync<ParkingSpaces>();
        var body = GetPostBody("NA74 GGD");
        
        // Act
        var response = await factory.CreateClient().PostAsync("/parking", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ParkingInfo>();
        Assert.Equal(preParkingSpaces.OccupiedSpaces, info!.SpaceNumber);
    }

    private async Task FillSpaces(int fullSpaces)
    {
        for (var i = 0; i < fullSpaces; i++)
        {
            var fullSpaceBody = GetPostBody("NA74 GG" + i);
            await factory.CreateClient().PostAsync("/parking", fullSpaceBody);
        }
    }

    private static StringContent GetPostBody(string vehicleReg)
    {
        var model = new { VehicleReg = vehicleReg, VehicleType = 1 };
        var content = JsonSerializer.Serialize(model);
        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}