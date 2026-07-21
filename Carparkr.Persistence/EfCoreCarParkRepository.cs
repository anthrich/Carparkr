using Carparkr.Domain;
using Microsoft.EntityFrameworkCore;

namespace Carparkr.Persistence;

public class EfCoreCarParkRepository(CarParkContext context) : ICarParkRepository
{
    public async Task Save(CarPark carPark)
    {
        context.CarParks.Add(carPark);
        await context.SaveChangesAsync();
    }

    public Task<List<CarPark>> Get()
    {
        return context.CarParks.ToListAsync();
    }
}