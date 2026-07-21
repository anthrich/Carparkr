var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/parking", () =>
    {
        var spaces = new { AvailableSpaces = 10, OccupiedSpaces = 0 };
        return spaces;
    })
    .WithName("GetParking")
    .WithOpenApi();

app.Run();