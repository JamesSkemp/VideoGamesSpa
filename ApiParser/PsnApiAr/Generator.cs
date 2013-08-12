using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using VideoGamesSpa.ApiParser.Models;
using VideoGamesSpa.ApiParser.Utilities;

namespace VideoGamesSpa.ApiParser.PsnApiAr
{
	public class Generator : BasePsnGenerator
	{
		internal XNamespace psnNs = "http://www.psnapi.com.ar/ps3/api";

		public Generator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("psnapiar generator");
				var profileXml = XDocument.Load(this.ProfileFile.FullName);
				var gamesXml = XDocument.Load(this.GamesFile.FullName);

				var psnGames = new List<PsnGame>();
				var psnXml = gamesXml.Descendants(psnNs + "Game");

				#region Games
				foreach (var game in psnXml)
				{
					var psnGame = new PsnGame();
					psnGame.Title = game.Element(psnNs + "Title").Value.Trim();
					psnGame.Id = game.Element(psnNs + "Id").Value;
					psnGame.LastPlayed = Shared.ParseXmlDateTime(game.Element(psnNs + "LastUpdated").Value, 2);
					psnGame.EarnedPoints = int.Parse(game.Element(psnNs + "UserPoints").Value);
					psnGame.PossiblePoints = int.Parse(game.Element(psnNs + "TotalPoints").Value);
					psnGame.EarnedTrophies = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Earned").Value);
					psnGame.PossibleTrophies = int.Parse(game.Element(psnNs + "TotalTrophies").Value);
					psnGame.EarnedBronze = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Bronze").Value);
					psnGame.EarnedSilver = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Silver").Value);
					psnGame.EarnedGold = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Gold").Value);
					psnGame.EarnedPlatinum = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Platinum").Value);
					psnGame.Platform = game.Element(psnNs + "Platform").Value;

					var trophyXml = XDocument.Load(this.GameFiles.SingleOrDefault(f => f.Name.Contains(psnGame.Id)).FullName);
					foreach (var gameTrophy in trophyXml.Descendants(psnNs + "Trophy"))
					{
						var trophy = new BasicTrophy();
						trophy.Id = gameTrophy.Element(psnNs + "IdTrophy").Value;
						trophy.Title = gameTrophy.Element(psnNs + "Title").Value.Trim();
						trophy.Image = gameTrophy.Element(psnNs + "Image").Value;
						trophy.Description = gameTrophy.Element(psnNs + "Description").Value;
						trophy.Earned = Shared.ParseXmlDateTime(gameTrophy.Element(psnNs + "DateEarned").Value, 2);
						trophy.Type = gameTrophy.Element(psnNs + "TrophyType").Value;

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
					var gameTitle = game.Element(psnNs + "Title").Value.Trim();
					var gameId = game.Element(psnNs + "Id").Value;

					var trophyXml = XDocument.Load(this.GameFiles.SingleOrDefault(f => f.Name.Contains(gameId)).FullName);
					foreach (var gameTrophy in trophyXml.Descendants(psnNs + "Trophy"))
					{
						var trophy = new Trophy();
						trophy.Title = gameTrophy.Element(psnNs + "Title").Value.Trim();
						trophy.Id = gameTrophy.Element(psnNs + "IdTrophy").Value;
						trophy.Image = gameTrophy.Element(psnNs + "Image").Value;
						trophy.Description = gameTrophy.Element(psnNs + "Description").Value;
						trophy.Earned = Shared.ParseXmlDateTime(gameTrophy.Element(psnNs + "DateEarned").Value, 2);
						trophy.Type = gameTrophy.Element(psnNs + "TrophyType").Value;
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
					playedGame.Id = game.Element(psnNs + "Id").Value;
					playedGame.Title = game.Element(psnNs + "Title").Value;
					playedGame.Image = game.Element(psnNs + "Image").Value;
					playedGame.LastPlayed = Shared.ParseXmlDateTime(game.Element(psnNs + "LastUpdated").Value, 2).Value;
					playedGame.Progress = double.Parse(game.Element(psnNs + "Progress").Value);
					playedGame.EarnedPoints = int.Parse(game.Element(psnNs + "UserPoints").Value);
					playedGame.EarnedAccomplishments = int.Parse(game.Element(psnNs + "TrophiesCount").Element(psnNs + "Earned").Value);
					playedGame.TotalPoints = int.Parse(game.Element(psnNs + "TotalPoints").Value);
					playedGame.TotalAccomplishments = int.Parse(game.Element(psnNs + "TotalTrophies").Value);
					playedGame.Platform = game.Element(psnNs + "Platform").Value;

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
				psnProfile.Id = profileXml.Element(psnNs + "PSNId").Element(psnNs + "ID").Value;
				psnProfile.Pic = profileXml.Element(psnNs + "PSNId").Element(psnNs + "Avatar").Value;
				psnProfile.Points = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "LevelData").Element(psnNs + "Points").Value);
				psnProfile.Level = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Level").Value);
				psnProfile.LevelProgress = double.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Progress").Value);
				psnProfile.NextLevelPoints = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "LevelData").Element(psnNs + "Ceiling").Value);
				psnProfile.Trophies = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Trophies").Element(psnNs + "Total").Value);
				psnProfile.TrophiesBronze = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Trophies").Element(psnNs + "Bronze").Value);
				psnProfile.TrophiesSilver = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Trophies").Element(psnNs + "Silver").Value);
				psnProfile.TrophiesGold = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Trophies").Element(psnNs + "Gold").Value);
				psnProfile.TrophiesPlatinum = int.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "Trophies").Element(psnNs + "Platinum").Value);
				psnProfile.PossibleTrophies = Trophies.Count;
				psnProfile.PossibleTrophiesBronze = Trophies.Where(t => t.Type == "BRONZE").Count();
				psnProfile.PossibleTrophiesSilver = Trophies.Where(t => t.Type == "SILVER").Count();
				psnProfile.PossibleTrophiesGold = Trophies.Where(t => t.Type == "GOLD").Count();
				psnProfile.PossibleTrophiesPlatinum = Trophies.Where(t => t.Type == "PLATINUM").Count();
				psnProfile.CompletionPercent = double.Parse(profileXml.Element(psnNs + "PSNId").Element(psnNs + "GameCompletion").Value);
				psnProfile.TotalGames = gamesXml.Descendants().Elements(psnNs + "Game").Count();
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
