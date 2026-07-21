using Carparkr.Domain;

namespace Carparkr.Persistence;

public class EfCoreCarParkRepository : ICarParkRepository
{
    public Task Save(CarPark carPark)
    {
        throw new NotImplementedException();
    }

    public Task<List<CarPark>> Get()
    {
        throw new NotImplementedException();
    }
}