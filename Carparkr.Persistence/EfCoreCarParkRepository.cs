using Carparkr.Domain;
using Microsoft.EntityFrameworkCore;

namespace Carparkr.Persistence;

public class EfCoreCarParkRepository : ICarParkRepository
{
    private readonly CarParkContext _context = new();

    public async Task Save(CarPark carPark)
    {
        _context.CarParks.Add(carPark);
        await _context.SaveChangesAsync();
    }

    public Task<List<CarPark>> Get()
    {
        return _context.CarParks.ToListAsync();
    }
}

internal sealed class CarParkContext : DbContext
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
