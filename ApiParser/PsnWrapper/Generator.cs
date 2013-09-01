using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoGamesSpa.ApiParser.Models;
using VideoGamesSpa.ApiParser.Utilities;

namespace VideoGamesSpa.ApiParser.PsnWrapper
{
	public class Generator : BasePsnGenerator
	{
		public Generator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("psn unofficial generator");
				var profileXml = XDocument.Load(this.ProfileFile.FullName).Root;
				var gamesXml = XDocument.Load(this.GamesFile.FullName);

				var psnGames = new List<PsnGame>();
				var psnXml = gamesXml.Descendants("UserGame");

				#region Games
				foreach (var game in psnXml)
				{
					var psnGame = new PsnGame();
					psnGame.Title = game.Element("Title").Value.Trim();
					psnGame.Id = game.Element("Id").Value;
					psnGame.LastPlayed = Shared.ParseXmlDateTime(game.Element("LastUpdated").Value, 0);
					psnGame.EarnedPoints = int.Parse(game.Element("EarnedPoints").Value);
					psnGame.PossiblePoints = int.Parse(game.Element("TotalPoints").Value);
					psnGame.EarnedTrophies = int.Parse(game.Element("TotalEarned").Value);
					psnGame.PossibleTrophies = int.Parse(game.Element("PossibleTrophies").Value);
					psnGame.EarnedBronze = int.Parse(game.Element("BronzeEarned").Value);
					psnGame.EarnedSilver = int.Parse(game.Element("SilverEarned").Value);
					psnGame.EarnedGold = int.Parse(game.Element("GoldEarned").Value);
					psnGame.EarnedPlatinum = int.Parse(game.Element("PlatinumEarned").Value);
					psnGame.Platform = game.Element("Platform").Value;

					var trophyXml = XDocument.Load(this.GameFiles.SingleOrDefault(f => f.Name.Contains(psnGame.Id)).FullName);
					foreach (var gameTrophy in trophyXml.Descendants("UserTrophy"))
					{
						var trophy = new BasicTrophy();
						trophy.Id = gameTrophy.Element("Id").Value;
						trophy.Title = gameTrophy.Element("Title").Value.Trim();
						trophy.Image = gameTrophy.Element("ImageUrl").Value;
						trophy.Description = gameTrophy.Element("Description").Value;
						trophy.Earned = Shared.ParseXmlDateTime(gameTrophy.Element("Earned").Value, 0);
						trophy.Type = gameTrophy.Element("Type").Value.ToUpper();

						psnGame.Trophies.Add(trophy);
					}
					psnGames.Add(psnGame);
				}
				psnGames = psnGames.OrderBy(g => g.Title).ToList();
				this.Games = psnGames;
				#endregion
				
				#region Trophies
				var psnTrophies = new List<Trophy>();

				foreach (var game in psnXml)
				{
					var gameTitle = game.Element("Title").Value.Trim();
					var gameId = game.Element("Id").Value;

					var trophyXml = XDocument.Load(this.GameFiles.SingleOrDefault(f => f.Name.Contains(gameId)).FullName);
					foreach (var gameTrophy in trophyXml.Descendants("UserTrophy"))
					{
						var trophy = new Trophy();
						trophy.Title = gameTrophy.Element("Title").Value.Trim();
						trophy.Id = gameTrophy.Element("Id").Value;
						trophy.Image = gameTrophy.Element("ImageUrl").Value;
						trophy.Description = gameTrophy.Element("Description").Value;
						trophy.Earned = Shared.ParseXmlDateTime(gameTrophy.Element("Earned").Value, 0);
						trophy.Type = gameTrophy.Element("Type").Value.ToUpper();
						trophy.GameTitle = gameTitle;
						trophy.GameId = gameId;

						psnTrophies.Add(trophy);
					}
				}
				psnTrophies = psnTrophies.OrderBy(t => t.GameTitle).ThenByDescending(t => t.Earned).ThenBy(t => int.Parse(t.Id)).ToList();
				this.Trophies = psnTrophies;
				#endregion
				
				#region Games Basic
				var psnGamesBasic = new List<PlayedGame>();

				foreach (var game in psnXml)
				{
					var playedGame = new PlayedGame();
					playedGame.Id = game.Element("Id").Value;
					playedGame.Title = game.Element("Title").Value;
					playedGame.Image = game.Element("ImageUrl").Value;
					playedGame.LastPlayed = Shared.ParseXmlDateTime(game.Element("LastUpdated").Value, 0).Value;
					playedGame.Progress = double.Parse(game.Element("Progress").Value);
					playedGame.EarnedPoints = int.Parse(game.Element("EarnedPoints").Value);
					playedGame.EarnedAccomplishments = int.Parse(game.Element("TotalEarned").Value);
					playedGame.TotalPoints = int.Parse(game.Element("TotalPoints").Value);
					playedGame.TotalAccomplishments = int.Parse(game.Element("PossibleTrophies").Value);
					playedGame.Platform = game.Element("Platform").Value;

					var gameAccomplishments = psnTrophies.Where(t => t.GameId == playedGame.Id).Select(t => new
					{
						Type = t.Type,
						Earned = t.Earned
					});
					var allTypes = gameAccomplishments.GroupBy(a => a.Type);
					foreach (var type in allTypes)
					{
						playedGame.Accomplishments.Add(new SimpleAccomplishment
						{
							Type = type.Key,
							Value = Psn.TrophyTypeToPoints(type.Key),
							Total = type.Count(),
							Earned = type.Where(t => t.Earned.HasValue).Count()
						});
					}
					playedGame.Accomplishments = playedGame.Accomplishments.OrderBy(a => a.Value).ToList();
					psnGamesBasic.Add(playedGame);
				}
				this.GamesBasic = psnGamesBasic;
				#endregion
				
				#region Profile
				var psnProfile = new PsnProfile();
				psnProfile.Id = profileXml.Element("Id").Value;
				psnProfile.Pic = profileXml.Element("ImageUrl").Value;
				psnProfile.Points = int.Parse(profileXml.Element("EarnedPoints").Value);
				psnProfile.Level = int.Parse(profileXml.Element("Level").Value);
				psnProfile.LevelProgress = double.Parse(profileXml.Element("LevelProgress").Value);
				psnProfile.NextLevelPoints = int.Parse(profileXml.Element("LevelNextPoints").Value);
				psnProfile.Trophies = int.Parse(profileXml.Element("TotalEarned").Value);
				psnProfile.TrophiesBronze = int.Parse(profileXml.Element("BronzeEarned").Value);
				psnProfile.TrophiesSilver = int.Parse(profileXml.Element("SilverEarned").Value);
				psnProfile.TrophiesGold = int.Parse(profileXml.Element("GoldEarned").Value);
				psnProfile.TrophiesPlatinum = int.Parse(profileXml.Element("PlatinumEarned").Value);
				psnProfile.PossibleTrophies = Trophies.Count;
				psnProfile.PossibleTrophiesBronze = Trophies.Where(t => t.Type == "BRONZE").Count();
				psnProfile.PossibleTrophiesSilver = Trophies.Where(t => t.Type == "SILVER").Count();
				psnProfile.PossibleTrophiesGold = Trophies.Where(t => t.Type == "GOLD").Count();
				psnProfile.PossibleTrophiesPlatinum = Trophies.Where(t => t.Type == "PLATINUM").Count();
				psnProfile.CompletionPercent = (100d * psnProfile.Trophies / psnProfile.PossibleTrophies);
				psnProfile.TotalGames = gamesXml.Descendants().Elements("UserGame").Count();
				psnProfile.PossiblePoints = ((psnProfile.PossibleTrophiesBronze * (int)Psn.TrophyPoints.Bronze)
					+ (psnProfile.PossibleTrophiesSilver * (int)Psn.TrophyPoints.Silver)
					+ (psnProfile.PossibleTrophiesGold * (int)Psn.TrophyPoints.Gold)
					+ (psnProfile.PossibleTrophiesPlatinum * (int)Psn.TrophyPoints.Platinum));
				this.Profile = psnProfile;
				#endregion
				
				#region Stats
				var accomplishments = VideoGameAccomplishment.ParseTrophies(this.Trophies);
				this.Stats = new VideoGameStats(accomplishments);
				#endregion

				return true;
			}
			return false;
		}

	}
}
