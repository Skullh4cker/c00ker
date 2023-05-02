namespace Dishcovery.Model;

public class Time
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public Time(int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }
}
