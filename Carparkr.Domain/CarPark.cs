namespace Carparkr.Domain;

public sealed class CarPark
{
    private readonly List<ParkedVehicle> _parkedVehicles = new(100);
    
    public SpaceSummary GetSpaceSummary()
    {
        return new SpaceSummary(
            100,
            _parkedVehicles.Count,
            _parkedVehicles.Capacity - _parkedVehicles.Count
        );
    }

    public void AllocateSpace(string vehicleRegistration, DateTime timestamp, Size size = default)
    {
        _parkedVehicles.Add(new ParkedVehicle(vehicleRegistration, timestamp));
    }

    public ExitResult ExitVehicle(string vehicleRegistration, DateTime timestamp)
    {
        var parkedVehicle = _parkedVehicles.FirstOrDefault(v => v.Registration == vehicleRegistration);
        if (parkedVehicle == default) return new ExitResult(0);
        _parkedVehicles.Remove(parkedVehicle);
        var timeParked = timestamp - parkedVehicle.TimeParked;
        return new ExitResult((timeParked.Minutes + 1) * 0.1m);
    }
}