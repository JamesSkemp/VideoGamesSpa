using System;
using System.Linq;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// Xbox Live achievement, with basic information.
	/// </summary>
	public class BasicAchievement
	{
		public string Id { get; set; }
		/// <summary>
		/// Title of the achievement.
		/// </summary>
		public string Title { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public DateTime? Earned { get; set; }
		public string GamerScore { get; set; }

		public BasicAchievement()
		{
		}

		/// <summary>
		/// Checks to see if the achievement can be updated (if it has no title and was earned) by checking against secret achievement listing.
		/// </summary>
		/// <param name="achievementId">Id of the achievement.</param>
		/// <param name="gameId">Id of the game.</param>
		/// <returns>True if the achievement was updated.</returns>
		public bool UpdateHiddenAchievement(string achievementId, string gameId, string xmlDirectory = null)
		{
			if ((string.IsNullOrWhiteSpace(this.Title) || this.Description.StartsWith("This is a secret achievement.")) && this.Earned.HasValue)
			{
				var hiddenAchievement = HiddenAchievement.ParseHiddenAchievementsXml(xmlDirectory)
					.FirstOrDefault(a => a.GameId == gameId && a.Id == achievementId);

				if (hiddenAchievement != null)
				{
					this.Title = hiddenAchievement.Title;
					this.Image = hiddenAchievement.Image;
					this.Description = hiddenAchievement.Description;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if the achievement can be updated (if it was earned, but doesn't have a date) by checking against offline achievement listing.
		/// </summary>
		/// <param name="achievementId">Id of the achievement.</param>
		/// <param name="gameId">Id of the game.</param>
		/// <returns>True if the achievement was updated.</returns>
		public bool UpdateOfflineAchievement(string achievementId, string gameId, string xmlPath = null)
		{
			if (this.Earned.HasValue && this.Earned.Value.Year == 1)
			{
				var offlineAchievement = OfflineAchievement.ParseOfflineAchievementsXml(xmlPath)
					.FirstOrDefault(a => a.GameId == gameId && a.Id == achievementId);

				if (offlineAchievement != null)
				{
					this.Earned = offlineAchievement.Earned;
					return true;
				}
			}
			return false;
		}
	}
}
