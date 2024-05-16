namespace DeepWork.Infrastructure.Models;
public static class DateTimeExtensions
{
    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }
}
