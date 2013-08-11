using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// Game for PlayStation Network.
	/// </summary>
	public class PsnGame
	{
		/// <summary>
		/// Title of the game.
		/// </summary>
		public string Title { get; set; }
		public string Id { get; set; }
		public DateTime? LastPlayed { get; set; }
		public int EarnedPoints { get; set; }
		public int PossiblePoints { get; set; }
		public int EarnedTrophies { get; set; }
		public int PossibleTrophies { get; set; }
		public int EarnedBronze { get; set; }
		public int EarnedSilver { get; set; }
		public int EarnedGold { get; set; }
		public int EarnedPlatinum { get; set; }
		public string Platform { get; set; }
		public List<BasicTrophy> Trophies { get; set; }

		/// <summary>
		/// Percentage of possible points earned.
		/// </summary>
		public double PointsPercentage
		{
			get
			{
				return PossiblePoints > 0
					? Math.Round(100d * EarnedPoints / PossiblePoints, Utilities.Shared.PercentRoundingPlaces)
					: 0;
			}
		}
		/// <summary>
		/// Percentage of possible trophies earned.
		/// </summary>
		public double TrophyPercentage
		{
			get
			{
				return PossibleTrophies > 0
					? Math.Round(100d * EarnedTrophies / PossibleTrophies, Utilities.Shared.PercentRoundingPlaces)
					: 0;
			}
		}

		public PsnGame()
		{
			this.Trophies = new List<BasicTrophy>();
		}
	}
}