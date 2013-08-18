using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace VideoGamesSpa.ApiParser.Models
{
	public class SpaData
	{
		/// <summary>
		/// Directory spa data was saved to.
		/// </summary>
		public string SpaDirectory { get; set; }
		public List<Models.PsnGame> PsnGames { get; set; }
		public List<Models.Trophy> PsnTrophies { get; set; }
		public List<Models.PlayedGame> PsnGamesBasic { get; set; }
		public Models.PsnProfile PsnProfile { get; set; }
		public Models.VideoGameStats PsnStats { get; set; }

		public List<Models.XblGame> XblGames { get; set; }
		public List<Models.Achievement> XblAchievements { get; set; }
		public List<Models.PlayedGame> XblGamesBasic { get; set; }
		public Models.XblProfile XblProfile { get; set; }
		public Models.VideoGameStats XblStats { get; set; }

		public Models.VideoGameStats CombinedStats { get; set; }

		public SpaData()
		{
		}

		public SpaData(string spaDirectory)
			: this()
		{
			this.SpaDirectory = spaDirectory;
		}

		public bool LoadData()
		{
			if (string.IsNullOrWhiteSpace(this.SpaDirectory))
			{
				throw new NotImplementedException("You must define the directory to grab data.");
			}
			else if (!Directory.Exists(this.SpaDirectory))
			{
				throw new DirectoryNotFoundException();
			}
			#region PlayStation Network
			var path = Path.Combine(this.SpaDirectory, "_psnGames.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<PsnGame>));
					this.PsnGames = (List<PsnGame>)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_psnGamesBasic.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<PlayedGame>));
					this.PsnGamesBasic = (List<PlayedGame>)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_psnProfile.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(PsnProfile));
					this.PsnProfile = (PsnProfile)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_psnStats.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(VideoGameStats));
					this.PsnStats = (VideoGameStats)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_psnTrophies.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<Trophy>));
					this.PsnTrophies = (List<Trophy>)serializer.Deserialize(reader);
				}
			}
			#endregion
			#region Xbox Live
			path = Path.Combine(this.SpaDirectory, "_xblAchievements.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<Achievement>));
					this.XblAchievements = (List<Achievement>)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_xblGames.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<XblGame>));
					this.XblGames = (List<XblGame>)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_xblGamesBasic.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(List<PlayedGame>));
					this.XblGamesBasic = (List<PlayedGame>)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_xblProfile.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(XblProfile));
					this.XblProfile = (XblProfile)serializer.Deserialize(reader);
				}
			}
			path = Path.Combine(this.SpaDirectory, "_xblStats.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(VideoGameStats));
					this.XblStats = (VideoGameStats)serializer.Deserialize(reader);
				}
			}
			#endregion
			#region Combined stats
			path = Path.Combine(this.SpaDirectory, "_combinedStats.xml");
			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					var serializer = new XmlSerializer(typeof(VideoGameStats));
					this.CombinedStats = (VideoGameStats)serializer.Deserialize(reader);
				}
			}
			#endregion
			return true;
		}
	}
}
