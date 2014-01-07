using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoGamesSpa.ApiParser.Models;
using VideoGamesSpa.ApiParser.Utilities;

namespace VideoGamesSpa.ApiParser.XboxLeaders
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
				DebugInfo.Add("xboxleaders generator");
				var profileXml = XDocument.Load(this.ProfileFile.FullName);
				var gamesXml = XDocument.Load(this.GamesFile.FullName);

				var xblGames = new List<XblGame>();
				#region Games
				var xblFiles = this.GameFiles;
				foreach (var xblFile in xblFiles)
				{
					var fileXml = XDocument.Load(xblFile.FullName).Descendants("data").First();
					var game = new XblGame();
					game.Title = fileXml.Element("game").Value
						.Replace("&trade;", "")
						.Replace("&reg;", "")
						.Replace("&#039;", "'")
						.Replace(@"Nightsâ¡/NA", "Nights II/NA");
					// todo Hack until this is once again returned by the XML.
					game.Id = xblFile.Name.Replace("achievements-", "").Replace(".xml", "");
					game.LastPlayed = Shared.ConvertStringToDateTime(fileXml.Element("lastplayed").Value);
					game.EarnedGamerScore = int.Parse(fileXml.Element("gamerscore").Element("current").Value);
					game.PossibleGamerScore = int.Parse(fileXml.Element("gamerscore").Element("total").Value);
					game.EarnedAchievements = int.Parse(fileXml.Element("achievement").Element("current").Value);
					game.PossibleAchievements = int.Parse(fileXml.Element("achievement").Element("total").Value);

					var achievements = fileXml.Elements("achievements");
					if (achievements == null || achievements.Count() == 1)
					{
						achievements = fileXml.Element("achievements").Elements("achievement");
					}

					foreach (var achievement in achievements)
					{
						var gameAchievement = new BasicAchievement();
						gameAchievement.Id = achievement.Element("id").Value;
						gameAchievement.Title = achievement.Element("title").Value
							.Replace("Secret Achievement", "")
							.Replace("&eacute;", "é")
							.Replace("&quot;", "\"")
							.Replace("&rsquo;", "'")
							.Replace("&hellip;", "...")
							.Replace("&amp;", "&")
							.Replace("&auml;", "ä")
							.Replace("&#039;", "'")
							.Trim();
						gameAchievement.Image = (!string.IsNullOrWhiteSpace(achievement.Element("artwork").Element("unlocked").Value) ?
							achievement.Element("artwork").Element("unlocked").Value : achievement.Element("artwork").Element("locked").Value)
							.Replace("https://live.xbox.com/Content/Images/HiddenAchievement.png", "");
						gameAchievement.Description = achievement.Element("description").Value
							.Replace("This is a secret achievement. Unlock it to find out more.", "")
							.Replace("&amp;", "&")
							.Replace("&#039;", "'")
							.Replace("&quot;", "\"")
							.Replace("&hellip;", "...")
							.Replace("&eacute;", "é")
							.Replace("&rsquo;", "'")
							.Trim();
						gameAchievement.Earned =
							achievement.Element("unlocked").Value == "1" ?
								(achievement.Element("unlockdate") != null && !string.IsNullOrWhiteSpace(achievement.Element("unlockdate").Value)
								? Shared.ConvertStringToDateTime(achievement.Element("unlockdate").Value)
								: new DateTime())
							: null;
						if (gameAchievement.Earned.HasValue)
						{
							gameAchievement.Earned = gameAchievement.Earned.Value.AddHours(-1);
							if (gameAchievement.Earned.Value.Year < 1981)
							{
								gameAchievement.Earned = new DateTime();
							}
						}
						gameAchievement.GamerScore = achievement.Element("gamerscore").Value
							.Replace("--", "0");

						if (achievement.Element("secret").Value == "1")
						{
							gameAchievement.UpdateHiddenAchievement(achievement.Element("id").Value, game.Id, this.HiddenAchievementsDirectory);
						}

						if (gameAchievement.Earned == new DateTime())
						{
							gameAchievement.UpdateOfflineAchievement(achievement.Element("id").Value, game.Id, this.OfflineAchievementsXmlPath);
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
					var fileXml = XDocument.Load(xblFile.FullName).Descendants("data").First();
					var gameTitle = fileXml.Element("game").Value
						.Replace("&trade;", "")
						.Replace("&reg;", "")
						.Replace("&#039;", "'")
						.Replace(@"Nightsâ¡/NA", "Nights II/NA");
					// todo Hack until this is once again returned by the XML.
					var gameId = xblFile.Name.Replace("achievements-", "").Replace(".xml", "");

					var achievements = fileXml.Elements("achievements");
					if (achievements == null || achievements.Count() == 1)
					{
						achievements = fileXml.Element("achievements").Elements("achievement");
					}

					foreach (var achievement in achievements)
					{
						var gameAchievement = new Achievement();
						gameAchievement.Title = achievement.Element("title").Value
							.Replace("Secret Achievement", "")
							.Replace("&eacute;", "é")
							.Replace("&quot;", "\"")
							.Replace("&rsquo;", "'")
							.Replace("&hellip;", "...")
							.Replace("&amp;", "&")
							.Replace("&auml;", "ä")
							.Replace("&#039;", "'")
							.Trim();
						gameAchievement.Id = achievement.Element("id").Value;
						gameAchievement.Image = (!string.IsNullOrWhiteSpace(achievement.Element("artwork").Element("unlocked").Value) ?
							achievement.Element("artwork").Element("unlocked").Value : achievement.Element("artwork").Element("locked").Value)
							.Replace("https://live.xbox.com/Content/Images/HiddenAchievement.png", "");
						gameAchievement.Description = achievement.Element("description").Value
							.Replace("This is a secret achievement. Unlock it to find out more.", "")
							.Replace("&amp;", "&")
							.Replace("&#039;", "'")
							.Replace("&quot;", "\"")
							.Replace("&hellip;", "...")
							.Replace("&eacute;", "é")
							.Replace("&rsquo;", "'")
							.Trim();
						gameAchievement.Earned =
							achievement.Element("unlocked").Value == "1" ?
								(achievement.Element("unlockdate") != null && !string.IsNullOrWhiteSpace(achievement.Element("unlockdate").Value)
								? Shared.ConvertStringToDateTime(achievement.Element("unlockdate").Value)
								: new DateTime())
							: null;
						// temporary fix
						if (gameAchievement.Earned.HasValue)
						{
							gameAchievement.Earned = gameAchievement.Earned.Value.AddHours(-1);
							if (gameAchievement.Earned.Value.Year < 1981)
							{
								gameAchievement.Earned = new DateTime();
							}
						}
						gameAchievement.GamerScore = achievement.Element("gamerscore").Value
							.Replace("--", "0");
						gameAchievement.GameTitle = gameTitle;
						gameAchievement.GameId = gameId;

						if (achievement.Element("secret").Value == "1" || achievement.Element("description").Value.StartsWith("This is a secret achievement."))
						{
							gameAchievement.UpdateHiddenAchievement(this.HiddenAchievementsDirectory);
						}

						if (gameAchievement.Earned == new DateTime())
						{
							gameAchievement.UpdateOfflineAchievement(this.OfflineAchievementsXmlPath);
						}

						xblAchievements.Add(gameAchievement);
					}
					xblAchievements = xblAchievements.OrderBy(a => a.GameTitle).ThenByDescending(a => a.Earned).ToList();
					this.Achievements = xblAchievements;
				}
				#endregion

				#region Games Basic
				var xblGamesBasic = new List<PlayedGame>();
				foreach (var game in gamesXml.Descendants("game"))
				{
					var playedGame = new PlayedGame();
					playedGame.Id = game.Element("id").Value;
					playedGame.Title = game.Element("title").Value
						.Replace("&trade;", "")
						.Replace("&reg;", "")
						.Replace("&#039;", "'")
						.Replace(@"Nightsâ¡/NA", "Nights II/NA");
					playedGame.Image = game.Element("artwork").Element("small").Value;
					playedGame.LastPlayed = Shared.ConvertStringToDateTime(game.Element("lastplayed").Value).Value;
					playedGame.Progress = double.Parse(game.Element("progress").Value);
					playedGame.EarnedPoints = int.Parse(game.Element("gamerscore").Element("current").Value);
					playedGame.EarnedAccomplishments = int.Parse(game.Element("achievements").Element("current").Value);
					playedGame.TotalPoints = int.Parse(game.Element("gamerscore").Element("total").Value);
					playedGame.TotalAccomplishments = int.Parse(game.Element("achievements").Element("total").Value);
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
				xblProfile.Id = gamesXml.Element("xbox").Element("data").Element("gamertag").Value;
				xblProfile.Pic = profileXml.Element("xbox").Element("data").Element("avatar").Element("tile").Value;
				xblProfile.GamerScore = int.Parse(gamesXml.Element("xbox").Element("data").Element("gamerscore").Element("current").Value);
				xblProfile.PossibleGamerScore = int.Parse(gamesXml.Element("xbox").Element("data").Element("gamerscore").Element("total").Value);
				xblProfile.Achievements = int.Parse(gamesXml.Element("xbox").Element("data").Element("achievements").Element("current").Value);
				xblProfile.PossibleAchievements = int.Parse(gamesXml.Element("xbox").Element("data").Element("achievements").Element("total").Value);
				xblProfile.CompletionPercent = double.Parse(gamesXml.Element("xbox").Element("data").Element("progress").Value);
				xblProfile.TotalGames = this.Games.Count;
				this.Profile = xblProfile;
				#endregion

				#region Stats
				var accomplishments = VideoGameAccomplishment.ParseAchievements(this.Achievements);
				this.Stats = new VideoGameStats(accomplishments);
				#endregion

				return true;
			}
			return false;
		}
	}
}
