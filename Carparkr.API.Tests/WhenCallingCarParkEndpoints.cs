using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Carparkr.API.Tests;

public class WhenCallingCarParkEndpoints(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
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

    private sealed record ParkingSpaces(int AvailableSpaces, int OccupiedSpaces);
    
    [Fact]
    public async Task POST_parking_integrates()
    {
        // Arrange
        var model = new { VehicleReg = "NA74 GGD", VehicleType = 1 };
        var content = JsonSerializer.Serialize(model);
        var body = new StringContent(content, Encoding.UTF8, "application/json");
        
        // Act
        var response = await factory.CreateClient().PostAsync("/parking", body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}