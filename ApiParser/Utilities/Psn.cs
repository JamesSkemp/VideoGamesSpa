using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Utilities
{
	/// <summary>
	/// Utility methods specific to the PlayStation Network.
	/// </summary>
	public class Psn
	{
		/// <summary>
		/// Number of points each PlayStation Network trophy is worth.
		/// </summary>
		public enum TrophyPoints
		{
			/// <summary>
			/// Bronze trophy.
			/// </summary>
			Bronze = 15,
			/// <summary>
			/// Silver trophy.
			/// </summary>
			Silver = 30,
			/// <summary>
			/// Gold trophy.
			/// </summary>
			Gold = 90,
			/// <summary>
			/// Platinum trophy.
			/// </summary>
			Platinum = 180
		}

		/// <summary>
		/// Converts a trophy type (from PSN web service) to the number of points it's worth.
		/// </summary>
		/// <param name="type">Tyep of trophy.</param>
		public static int TrophyTypeToPoints(string type)
		{
			switch (type.ToLower())
			{
				case "bronze":
					return (int)Psn.TrophyPoints.Bronze;
				case "silver":
					return (int)Psn.TrophyPoints.Silver;
				case "gold":
					return (int)Psn.TrophyPoints.Gold;
				case "platinum":
					return (int)Psn.TrophyPoints.Platinum;
				default:
					break;
			}
			return 0;
		}
	}
}
