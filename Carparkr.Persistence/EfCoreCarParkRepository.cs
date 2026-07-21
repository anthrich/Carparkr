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

public sealed class CarParkContext : DbContext
{
    public DbSet<CarPark> CarParks => Set<CarPark>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CarParks");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarPark>().Property<int>("Id");
        modelBuilder.Entity<CarPark>().HasKey("Id");
    }
}
