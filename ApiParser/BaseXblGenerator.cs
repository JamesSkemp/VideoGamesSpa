using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser
{
	public class BaseXblGenerator : BaseGenerator
	{
		/// <summary>
		/// Directory that contains XML files with data about hidden achievements.
		/// </summary>
		public string HiddenAchievementsDirectory { get; set; }
		/// <summary>
		/// Full path to the XML file that contains data about achievements earned offline.
		/// </summary>
		public string OfflineAchievementsXmlPath { get; set; }

		public List<Models.XblGame> Games { get; set; }
		public List<Models.Achievement> Achievements { get; set; }
		public List<Models.PlayedGame> GamesBasic { get; set; }
		//public Models.XblProfile XblProfile { get; set; }
		//public Models.VideoGameStats XblStats { get; set; }

		public BaseXblGenerator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("xbl generator");
				var directoryFiles = new DirectoryInfo(this.ApiOutputDirectory).GetFiles("*.xml").Where(f => !f.Name.StartsWith("_"));
				this.ProfileFile = directoryFiles.SingleOrDefault(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "profile")));
				this.GamesFile = directoryFiles.SingleOrDefault(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "games")));
				this.GameFiles = directoryFiles.Where(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "achievements-")));
				return true;
			}
			return false;
		}
	}
}
