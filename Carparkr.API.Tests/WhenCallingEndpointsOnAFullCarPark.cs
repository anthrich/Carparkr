using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Carparkr.API.Tests;

public class WhenCallingEndpointsOnAFullCarPark(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();
    
    public Task InitializeAsync()
    {
        return FillSpaces();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public async Task POST_parking_returns_422()
    {
        // Arrange
        await FillSpaces();
        var body = GetPostParkingBody("NA74 GGD");
        
        // Act
        var response = await _client.PostAsync("/parking", body);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
    }
    
    private async Task FillSpaces()
    {
        for (var i = 0; i < 100; i++)
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