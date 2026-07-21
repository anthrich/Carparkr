namespace Carparkr.Domain;

public class CarPark
{
    public SpaceSummary GetSpaceSummary()
    {
        return new SpaceSummary(100, 0);
    }
}