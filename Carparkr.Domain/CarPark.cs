namespace Carparkr.Domain;

public class CarPark
{
    private List<string> _allocatedVehicleRegistrations = new();
    
    public SpaceSummary GetSpaceSummary()
    {
        return new SpaceSummary(100, _allocatedVehicleRegistrations.Count, 100);
    }

    public void AllocateSpace(string vehicleRegistration)
    {
        _allocatedVehicleRegistrations.Add(vehicleRegistration);
    }
}