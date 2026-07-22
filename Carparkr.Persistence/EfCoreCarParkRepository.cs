using Carparkr.Domain;
using Microsoft.EntityFrameworkCore;

namespace Carparkr.Persistence;

public class EfCoreCarParkRepository(CarParkContext context) : ICarParkRepository
{
    public async Task Save(CarPark carPark)
    {
        var existing = await context.CarParks.FindAsync(carPark.Id);
        if (existing == null) context.CarParks.Add(carPark);
        else context.Update(carPark);
        await context.SaveChangesAsync();
    }

    public Task<List<CarPark>> Get()
    {
        return context.CarParks.ToListAsync();
    }
}