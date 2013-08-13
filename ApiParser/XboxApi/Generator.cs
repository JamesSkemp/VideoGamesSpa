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
				var xblGamesBasic = new List<PlayedGame>();
				foreach (var game in gamesXml.Descendants("Games"))
				{
					var playedGame = new PlayedGame();
					playedGame.Id = game.Element("ID").Value;
					playedGame.Title = game.Element("Name").Value;
					playedGame.Image = game.Element("BoxArt").Element("Small").Value;
					playedGame.LastPlayed = Shared.ConvertStringToDateTime(game.Element("Progress").Element("LastPlayed-UNIX").Value).Value;
					playedGame.EarnedPoints = int.Parse(game.Element("Progress").Element("Score").Value);
					playedGame.EarnedAccomplishments = int.Parse(game.Element("Progress").Element("Achievements").Value);
					playedGame.TotalPoints = int.Parse(game.Element("PossibleGamerscore").Value);
					playedGame.TotalAccomplishments = int.Parse(game.Element("PossibleAchievements").Value);
					playedGame.Progress = Math.Round((double)playedGame.EarnedAccomplishments / playedGame.TotalAccomplishments * 100, 1);
					playedGame.Platform = "xbox";

					var gameAccomplishments = xblAchievements.Where(a => a.GameId == playedGame.Id).Select(a => new
					{
						Type = a.GamerScore,
						Earned = a.Earned
					});

					var allTypes = gameAccomplishments.GroupBy(a => a.Type);
					foreach (var type in allTypes)
					{
						playedGame.Accomplishments.Add(new SimpleAccomplishment
						{
							Type = type.Key,
							Value = int.Parse(type.Key),
							Total = type.Count(),
							Earned = type.Where(t => t.Earned.HasValue).Count()
						});
					}
					playedGame.Accomplishments = playedGame.Accomplishments.OrderBy(a => a.Value).ToList();
					if (playedGame.Accomplishments.Count > 0)
					{
						xblGamesBasic.Add(playedGame);
					}
				}
				this.GamesBasic = xblGamesBasic;
				#endregion

				#region Profile
				var xblProfile = new XblProfile();
				xblProfile.Id = gamesXml.Element("Data").Element("Player").Element("Gamertag").Value;
				xblProfile.Pic = profileXml.Element("Data").Element("Player").Element("Avatar").Element("Gamertile").Element("Large").Value;
				xblProfile.GamerScore = int.Parse(gamesXml.Element("Data").Element("Player").Element("Gamerscore").Value);
				xblProfile.PossibleGamerScore = xblGamesBasic.Sum(g => g.TotalPoints);
				xblProfile.Achievements = xblGamesBasic.Sum(g => g.EarnedAccomplishments);
				xblProfile.PossibleAchievements = xblGamesBasic.Sum(g => g.TotalAccomplishments);
				xblProfile.CompletionPercent = double.Parse(gamesXml.Element("Data").Element("Player").Element("PercentComplete").Value);
				xblProfile.TotalGames = this.Games.Count;
				this.Profile = xblProfile;
				#endregion

				// todo
				#region Stats
				#endregion
			}
			return false;
		}
	}
}
