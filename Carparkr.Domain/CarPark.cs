namespace Carparkr.Domain;

public class CarPark
{
    private readonly List<string> _allocatedVehicleRegistrations = new(100);
    
    public SpaceSummary GetSpaceSummary()
    {
        return new SpaceSummary(
            100,
            _allocatedVehicleRegistrations.Count,
            _allocatedVehicleRegistrations.Capacity - _allocatedVehicleRegistrations.Count
        );
    }

    public void AllocateSpace(string vehicleRegistration)
    {
        _allocatedVehicleRegistrations.Add(vehicleRegistration);
    }

    public void ExitVehicle(string vehicleRegistration)
    {
        _allocatedVehicleRegistrations.Remove(vehicleRegistration);
    }
}