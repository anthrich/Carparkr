using System.Net;
using System.Net.Http.Json;
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
}