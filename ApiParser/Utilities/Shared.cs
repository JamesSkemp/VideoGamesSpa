using System;

namespace VideoGamesSpa.ApiParser.Utilities
{
	/// <summary>
	/// Shared utility methods.
	/// </summary>
	public class Shared
	{
		/// <summary>
		/// Number of places to round percents.
		/// </summary>
		public static int PercentRoundingPlaces
		{
			get
			{
				return 3;
			}
		}

		/// <summary>
		/// Tries to convert a JavaScript date and time to a .NET DateTime.
		/// </summary>
		/// <param name="dateTime">String of the JavaScript date and time.</param>
		/// <seealso cref="ConvertDateTimeToDouble"/>
		public static DateTime? ConvertStringToDateTime(string dateTime)
		{
			if (string.IsNullOrWhiteSpace(dateTime))
			{
				return null;
			}
			var d1 = new DateTime(1970, 1, 1);

			long number;
			if (!long.TryParse(dateTime, out number))
			{
				return null;
			}
			number *= (1000L * 10000L);
			number += d1.Ticks;

			return new DateTime(number, DateTimeKind.Utc).ToLocalTime();
		}

		/// <summary>
		/// Tries to convert a .NET DateTime to a JavaScript date and time.
		/// </summary>
		/// <param name="dateTime">Date and time to convert.</param>
		/// <seealso cref="ConvertStringToDateTime"/>
		public static double? ConvertDateTimeToDouble(DateTime? dateTime)
		{
			if (!dateTime.HasValue)
			{
				return null;
			}
			var d1 = new DateTime(1970, 1, 1);
			return (dateTime.Value - d1).TotalMilliseconds;
		}

		/// <summary>
		/// Converts a string datetime? from an XML file to a datetime.
		/// </summary>
		/// <param name="datetime">String, which may be empty.</param>
		/// <param name="offset">Hours by which to offset the time.</param>
		/// <returns>Datetime.</returns>
		public static DateTime? ParseXmlDateTime(string datetime, int offset = 0)
		{
			return !string.IsNullOrWhiteSpace(datetime)
				? (DateTime?)DateTime.Parse(datetime).AddHours(offset)
				: null;
		}
	}
}