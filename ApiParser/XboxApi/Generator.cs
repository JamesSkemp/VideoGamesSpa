using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoGamesSpa.ApiParser.Models;
using VideoGamesSpa.ApiParser.Utilities;

namespace VideoGamesSpa.ApiParser.XboxApi
{
	public class Generator : BaseXblGenerator
	{
		public Generator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("xboxapi generator");
				var profileXml = XDocument.Load(this.ProfileFile.FullName);
				var gamesXml = XDocument.Load(this.GamesFile.FullName);

				var xblGames = new List<XblGame>();
				#region Games
				var xblFiles = this.GameFiles;
				foreach (var xblFile in xblFiles)
				{
					var fileXml = XDocument.Load(xblFile.FullName).Root;
					var game = new XblGame();
					game.Title = fileXml.Element("Game").Element("Name").Value;
					game.Id = fileXml.Element("Game").Element("ID").Value;
					game.LastPlayed = Shared.ConvertStringToDateTime(fileXml.Element("Game").Element("Progress").Element("LastPlayed-UNIX").Value);
					game.EarnedGamerScore = int.Parse(fileXml.Element("Game").Element("Progress").Element("Gamerscore").Value);
					game.PossibleGamerScore = int.Parse(fileXml.Element("Game").Element("PossibleGamerscore").Value);
					game.EarnedAchievements = int.Parse(fileXml.Element("Game").Element("Progress").Element("EarnedAchievements").Value);
					game.PossibleAchievements = int.Parse(fileXml.Element("Game").Element("PossibleAchievements").Value);

					var achievements = fileXml.Elements("Achievements");
					if (achievements == null || achievements.Count() == 1)
					{
						continue;
					}

					foreach (var achievement in achievements)
					{
						var gameAchievement = new BasicAchievement();
						gameAchievement.Id = achievement.Element("ID").Value;
						gameAchievement.Title = achievement.Element("Name").Value;
						gameAchievement.Image = achievement.Element("TileUrl").Value;
						gameAchievement.Description = achievement.Element("Description").Value;
						gameAchievement.Earned =
							!string.IsNullOrWhiteSpace(achievement.Element("EarnedOn-UNIX").Value) ?
								(achievement.Element("IsOffline").Value != "1" || achievement.Element("EarnedOn-UNIX").Value.StartsWith("-")
								? Shared.ConvertStringToDateTime(achievement.Element("EarnedOn-UNIX").Value)
								: new DateTime())
							: null;
						if (gameAchievement.Earned.HasValue)
						{
							if (gameAchievement.Earned.Value.Year < 1981)
							{
								gameAchievement.Earned = new DateTime();
							}
						}
						gameAchievement.GamerScore = achievement.Element("Score").Value
							.Replace("--", "0");

						if (string.IsNullOrWhiteSpace(achievement.Element("Name").Value))
						{
							gameAchievement.UpdateHiddenAchievement(achievement.Element("ID").Value, game.Id, this.HiddenAchievementsDirectory);
						}

						if (gameAchievement.Earned == new DateTime())
						{
							gameAchievement.UpdateOfflineAchievement(achievement.Element("ID").Value, game.Id, this.OfflineAchievementsXmlPath);
						}

						game.Achievements.Add(gameAchievement);
					}
					// We only want to add games that have achievements.
					if (game.Achievements.Count > 0)
					{
						xblGames.Add(game);
					}
				}
				xblGames = xblGames.OrderBy(g => g.Title).ToList();
				this.Games = xblGames;
				#endregion

				// todo
				#region Achievements
				var xblAchievements = new List<Achievement>();

				foreach (var xblFile in this.GameFiles)
				{
					var fileXml = XDocument.Load(xblFile.FullName).Root;
					var gameTitle = fileXml.Element("Game").Element("Name").Value;
					var gameId = fileXml.Element("Game").Element("ID").Value;

					var achievements = fileXml.Elements("Achievements");
					if (achievements == null || achievements.Count() == 1)
					{
						continue;
					}

					foreach (var achievement in achievements)
					{
						var gameAchievement = new Achievement();
						gameAchievement.Title = achievement.Element("Name").Value;
						gameAchievement.Id = achievement.Element("ID").Value;
						gameAchievement.Image = achievement.Element("TileUrl").Value;
						gameAchievement.Description = achievement.Element("Description").Value;
						gameAchievement.Earned =
							!string.IsNullOrWhiteSpace(achievement.Element("EarnedOn-UNIX").Value) ?
								(achievement.Element("IsOffline").Value != "1" || achievement.Element("EarnedOn-UNIX").Value.StartsWith("-")
								? Shared.ConvertStringToDateTime(achievement.Element("EarnedOn-UNIX").Value)
								: new DateTime())
							: null;
						if (gameAchievement.Earned.HasValue)
						{
							if (gameAchievement.Earned.Value.Year < 1981)
							{
								gameAchievement.Earned = new DateTime();
							}
						}
						gameAchievement.GamerScore = achievement.Element("Score").Value
							.Replace("--", "0");
						gameAchievement.GameTitle = gameTitle;
						gameAchievement.GameId = gameId;

						if (string.IsNullOrWhiteSpace(achievement.Element("Name").Value))
						{
							gameAchievement.UpdateHiddenAchievement(achievement.Element("ID").Value, gameId, this.HiddenAchievementsDirectory);
						}

						if (gameAchievement.Earned == new DateTime())
						{
							gameAchievement.UpdateOfflineAchievement(achievement.Element("ID").Value, gameId, this.OfflineAchievementsXmlPath);
						}

						xblAchievements.Add(gameAchievement);
					}
					xblAchievements = xblAchievements.OrderBy(a => a.GameTitle).ThenByDescending(a => a.Earned).ToList();
					this.Achievements = xblAchievements;
				}
				#endregion

				#region Games Basic
				#endregion

				#region Profile
				#endregion

				#region Stats
				#endregion
			}
			return false;
		}
	}
}
