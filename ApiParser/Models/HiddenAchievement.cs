using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoGamesSpa.ApiParser.Models
{
	public class HiddenAchievement
	{
		public string Id { get; set; }
		public string GameId { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }

		public HiddenAchievement()
		{
		}

		/// <summary>
		/// Return a listing of basic details for secret/hidden achievements, based upon XML files saved in a set directory.
		/// </summary>
		public static List<HiddenAchievement> ParseHiddenAchievementsXml(string xmlDirectory = null)
		{
			var hiddenAchievements = new List<HiddenAchievement>();
			if (string.IsNullOrWhiteSpace(xmlDirectory))
			{
				return hiddenAchievements;
			}
			var directory = new System.IO.DirectoryInfo(xmlDirectory);
			foreach (var file in directory.GetFiles("*.xml").Where(f => !f.Name.StartsWith("__")))
			{
				try
				{
					var xml = XDocument.Load(file.FullName);
					XNamespace ns = "http://media.jamesrskemp.com/ns/XblAchievements/201307";
					var gameId = xml.Root.Element(ns + "Game").Attribute("id").Value;
					var achievements = xml.Root.Elements(ns + "Achievement");
					foreach (var achievement in achievements)
					{
						var hiddenAchievement = new HiddenAchievement();
						hiddenAchievement.GameId = gameId;
						hiddenAchievement.Id = achievement.Attribute("id").Value;
						hiddenAchievement.Title = achievement.Element(ns + "Title").Value;
						hiddenAchievement.Image = achievement.Element(ns + "Image").Value;
						hiddenAchievement.Description = achievement.Element(ns + "Description").Value;
						hiddenAchievements.Add(hiddenAchievement);
					}
				}
				catch (Exception)
				{
					// todo once I determine how things will be logged
				}
			}
			return hiddenAchievements;
		}
	}
}
