using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// Game for Xbox Live.
	/// </summary>
	public class XblGame
	{
		/// <summary>
		/// Title of the game.
		/// </summary>
		public string Title { get; set; }
		public string Id { get; set; }
		public DateTime? LastPlayed { get; set; }
		public int EarnedGamerScore { get; set; }
		public int PossibleGamerScore { get; set; }
		public int EarnedAchievements { get; set; }
		public int PossibleAchievements { get; set; }
		public List<BasicAchievement> Achievements { get; set; }

		/// <summary>
		/// Percentage of possible gamerscore earned.
		/// </summary>
		public double GamerscorePercentage
		{
			get
			{
				return PossibleGamerScore > 0
					? Math.Round(100d * EarnedGamerScore / PossibleGamerScore, Utilities.Shared.PercentRoundingPlaces)
					: 0;
			}
		}
		/// <summary>
		/// Percentage of possible achievements earned.
		/// </summary>
		public double AchievementPercentage
		{
			get
			{
				return PossibleAchievements > 0
					? Math.Round(100d * EarnedAchievements / PossibleAchievements, Utilities.Shared.PercentRoundingPlaces)
					: 0;
			}
		}

		public XblGame()
		{
			this.Achievements = new List<BasicAchievement>();
		}
	}
}