namespace Carparkr.Domain;

public interface ICarParkRepository
{
    public Task Save(CarPark carPark);
    public Task<List<CarPark>> Get();
}