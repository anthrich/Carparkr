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
        _parkedVehicles.Add(new ParkedVehicle(vehicleRegistration, timestamp, size));
    }

    public ExitResult ExitVehicle(string vehicleRegistration, DateTime timestamp)
    {
        var parkedVehicle = _parkedVehicles.FirstOrDefault(v => v.Registration == vehicleRegistration);
        if (parkedVehicle == default) return new ExitResult(0);
        _parkedVehicles.Remove(parkedVehicle);
        var charge = CalculateCharge(timestamp, parkedVehicle);
        return new ExitResult(charge);
    }

    private static decimal CalculateCharge(DateTime timestamp, ParkedVehicle parkedVehicle)
    {
        var timeParked = timestamp - parkedVehicle.TimeParked;
        var baseMinutesMultiplier = timeParked.Minutes + 1;
        var sizeExponent = (int)parkedVehicle.Size;
        var baseCharge = (decimal)(baseMinutesMultiplier * 0.1 * Math.Pow(2, sizeExponent));
        var fiveMinuteCharges = timeParked.Minutes / 5;
        return baseCharge + fiveMinuteCharges;
    }
}