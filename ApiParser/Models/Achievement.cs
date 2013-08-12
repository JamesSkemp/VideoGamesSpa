using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// Xbox Live achievement with basic and additional information.
	/// </summary>
	public class Achievement : BasicAchievement
	{
		public string Id { get; set; }
		/// <summary>
		/// Title of the game this achievement is for.
		/// </summary>
		public string GameTitle { get; set; }
		/// <summary>
		/// Id of the game this achievement is for.
		/// </summary>
		public string GameId { get; set; }

		public Achievement()
			: base()
		{
		}

		/// <summary>
		/// Checks to see if the achievement can be updated (if it has no title and was earned) by checking against secret achievement listing.
		/// </summary>
		/// <returns>True if the achievement was updated.</returns>
		public bool UpdateHiddenAchievement(string xmlDirectory = null)
		{
			if (string.IsNullOrWhiteSpace(this.Title) && this.Earned.HasValue)
			{
				var hiddenAchievement = HiddenAchievement.ParseHiddenAchievementsXml(xmlDirectory)
					.FirstOrDefault(a => a.GameId == this.GameId && a.Id == this.Id);

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
		/// <returns>True if the achievement was updated.</returns>
		public bool UpdateOfflineAchievement(string xmlPath = null)
		{
			if (this.Earned.HasValue && this.Earned.Value.Year == 1)
			{
				var offlineAchievement = OfflineAchievement.ParseOfflineAchievementsXml(xmlPath)
					.FirstOrDefault(a => a.GameId == this.GameId && a.Id == this.Id);

				if (offlineAchievement != null)
				{
					this.Earned = offlineAchievement.Earned;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Deserialize XML into a collection of Achievement. 
		/// </summary>
		/// <param name="xml">Serialized XDocument.</param>
		public static List<Achievement> Load(XDocument xml)
		{
			XmlSerializer _s = new XmlSerializer(typeof(List<Achievement>));
			return (List<Achievement>)_s.Deserialize(xml.CreateReader());
		}
	}
}
