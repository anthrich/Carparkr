using Carparkr.Domain;
using Microsoft.EntityFrameworkCore;

namespace Carparkr.Persistence;

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