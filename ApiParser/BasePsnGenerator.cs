﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VideoGamesSpa.ApiParser
{
	public class BasePsnGenerator : BaseGenerator
	{
		public List<Models.PsnGame> Games { get; set; }
		//public List<Models.Trophy> PsnTrophies { get; set; }
		//public List<Models.PlayedGame> PsnGamesBasic { get; set; }
		//public Models.PsnProfile PsnProfile { get; set; }
		//public Models.VideoGameStats PsnStats { get; set; }

		public BasePsnGenerator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("psn generator");
				var directoryFiles = new DirectoryInfo(this.ApiOutputDirectory).GetFiles("*.xml").Where(f => !f.Name.StartsWith("_"));
				this.ProfileFile = directoryFiles.SingleOrDefault(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "profile")));
				this.GamesFile = directoryFiles.SingleOrDefault(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "games")));
				this.GameFiles = directoryFiles.Where(f => f.Name.StartsWith(string.Format(base.XmlNameFormat, "trophies-")));
				return true;
			}
			return false;
		}
	}
}
