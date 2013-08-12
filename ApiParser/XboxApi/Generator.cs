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
				// todo
				#region Games
				#endregion

				#region Achievements
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
