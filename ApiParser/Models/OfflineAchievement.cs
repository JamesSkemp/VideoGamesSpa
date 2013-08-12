using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoGamesSpa.ApiParser.Models
{
	public class OfflineAchievement
	{
		public string Id { get; set; }
		public string GameId { get; set; }
		public DateTime Earned { get; set; }

		public OfflineAchievement()
		{
		}

		public OfflineAchievement(string id, string gameId, DateTime earned)
		{
			this.Id = id;
			this.GameId = gameId;
			this.Earned = earned;
		}

		public static List<OfflineAchievement> ParseOfflineAchievementsXml(string xmlPath = null)
		{
			var offlineAchievements = new List<OfflineAchievement>();
			if (string.IsNullOrWhiteSpace(xmlPath))
			{
				return offlineAchievements;
			}
			XNamespace ns = "http://media.jamesrskemp.com/ns/XblOfflineAchievements/201308";

			var offlineData = XDocument.Load(xmlPath).Root.Descendants(ns + "Achievement");
			foreach (var achievement in offlineData)
			{
				var earned = DateTime.Parse(achievement.Attribute("Earned").Value);
				earned = DateTime.SpecifyKind(earned, DateTimeKind.Local);
				offlineAchievements.Add(new OfflineAchievement(
					achievement.Attribute("Id").Value,
					achievement.Attribute("GameId").Value,
					earned));
			}
			return offlineAchievements;
		}
	}
}
