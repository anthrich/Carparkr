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
    private sealed record ExitInfo(string VehicleReg, double VehicleCharge, DateTime TimeIn, DateTime TimeOut);

    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GET_parking_returns_spaces()
    {
        // Act
        var response = await _client.GetAsync("/parking");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var spaces = await response.Content.ReadFromJsonAsync<ParkingSpaces>();
        Assert.Equal(100, spaces!.OccupiedSpaces + spaces.AvailableSpaces);
    }

    [Fact]
    public async Task POST_parking_integrates()
    {
        // Arrange
        var body = GetPostParkingBody("NA74 GGD");
        
        // Act
        var response = await _client.PostAsync("/parking", body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task POST_parking_returns_parking_info_vehicle_reg()
    {
        // Arrange
        var body = GetPostParkingBody("NA74 GGD");

        // Act
        var response = await _client.PostAsync("/parking", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ParkingInfo>();
        Assert.Equal("NA74 GGD", info?.VehicleReg);
    }

    [Fact]
    public async Task POST_parking_returns_time_in()
    {
        // Arrange
        var body = GetPostParkingBody("NA74 GGD");
        
        // Act
        var response = await _client.PostAsync("/parking", body);

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
        var getResponse = await _client.GetAsync("/parking");
        var preParkingSpaces = await getResponse.Content.ReadFromJsonAsync<ParkingSpaces>();
        var body = GetPostParkingBody("NA74 GGD");
        
        // Act
        var response = await _client.PostAsync("/parking", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ParkingInfo>();
        Assert.Equal(preParkingSpaces!.OccupiedSpaces, info!.SpaceNumber);
    }
    
    [Fact]
    public async Task POST_exit_integrates()
    {
        // Arrange
        var parkingBody = GetPostParkingBody("NA74 GGZ");
        await _client.PostAsync("/parking", parkingBody);
        var body = GetPostExitBody("NA74 GGZ");

        // Act
        var response = await factory.CreateClient().PostAsync("/parking/exit", body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task POST_exit_returns_vehicle_reg()
    {
        // Arrange
        var parkingBody = GetPostParkingBody("NA74 GGZ");
        await _client.PostAsync("/parking", parkingBody);
        var body = GetPostExitBody("NA74 GGZ");

        // Act
        var response = await factory.CreateClient().PostAsync("/parking/exit", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ExitInfo>();
        Assert.Equal("NA74 GGZ", info!.VehicleReg);
    }
    
    [Fact]
    public async Task POST_exit_returns_vehicle_charge()
    {
        // Arrange
        var parkingBody = GetPostParkingBody("NA74 GGA", 2);
        await _client.PostAsync("/parking", parkingBody);
        var body = GetPostExitBody("NA74 GGA");

        // Act
        var response = await factory.CreateClient().PostAsync("/parking/exit", body);

        // Assert
        var info = await response.Content.ReadFromJsonAsync<ExitInfo>();
        Assert.Equal(0.40, info!.VehicleCharge);
    }

    private static StringContent GetPostExitBody(string vehicleReg)
    {
        var model = new { VehicleReg = vehicleReg };
        var content = JsonSerializer.Serialize(model);
        var body = new StringContent(content, Encoding.UTF8, "application/json");
        return body;
    }

    private async Task FillSpaces(int fullSpaces)
    {
        for (var i = 0; i < fullSpaces; i++)
        {
            var fullSpaceBody = GetPostParkingBody("NA74 GG" + i);
            await factory.CreateClient().PostAsync("/parking", fullSpaceBody);
        }
    }

    private static StringContent GetPostParkingBody(string vehicleReg, int type = 1)
    {
        var model = new { VehicleReg = vehicleReg, VehicleType = type };
        var content = JsonSerializer.Serialize(model);
        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}