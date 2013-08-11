using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser
{
	public class BaseGenerator
	{
		/*/// <summary>
		/// PlayStation Network id of the user to pull information for.
		/// </summary>
		public string PsnId { get; set; }
		/// <summary>
		/// Xbox Live id of the user to pull information for.
		/// </summary>
		public string Gamertag { get; set; }*/
		/// <summary>
		/// Directory the API wrappers saved data to.
		/// </summary>
		public string ApiOutputDirectory { get; set; }
		/// <summary>
		/// Format of the filename of the API XML output. Example: api-{0}
		/// </summary>
		public string XmlNameFormat { get; set; }
		/// <summary>
		/// File found containing profile information from the API.
		/// </summary>
		public FileInfo ProfileFile { get; set; }
		/// <summary>
		/// File found containing games information from the API.
		/// </summary>
		public FileInfo GamesFile { get; set; }
		/// <summary>
		/// Files found containing individual game information from the API.
		/// </summary>
		public IEnumerable<FileInfo> GameFiles { get; set; }

		public List<object> DebugInfo { get; set; }

		public BaseGenerator()
		{
			this.DebugInfo = new List<object>();
		}

		public virtual bool Run()
		{
			DebugInfo.Add("base generator");
			if (string.IsNullOrWhiteSpace(this.XmlNameFormat))
			{
				return false;
			}

			return true;
		}
	}
}
