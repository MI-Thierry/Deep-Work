using System;

namespace DeepWork.Helpers;

public static class DateTimeOffsetExtensions
{
	public static DateTimeOffset FirstDayOfWeek(this DateTimeOffset date, DayOfWeek startOfWeek = DayOfWeek.Monday)
	{
		int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
		return date.AddDays(-diff);
	}

	public static DateTimeOffset LastDayOfWeek(this DateTimeOffset date, DayOfWeek startOfWeek = DayOfWeek.Monday) =>
		date.FirstDayOfWeek(startOfWeek).AddDays(6);

	public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset date) =>
		new DateTime(date.Year, date.Month, 1);

	public static DateTimeOffset LastDayOfMonth(this DateTimeOffset date) =>
		date.FirstDayOfMonth().AddMonths(1).AddDays(-1);

	public static DateTimeOffset FirstDayOfYear(this DateTimeOffset date) =>
		new DateTime(date.Year, 1, 1);

	public static DateTimeOffset LastDayOfYear(this DateTimeOffset date) =>
		date.FirstDayOfYear().AddYears(1).AddDays(-1);
}
