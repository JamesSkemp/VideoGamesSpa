using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoGamesSpa.ApiParser.Models;

namespace VideoGamesSpa.ApiParser.Combined
{
	public class Generator
	{
		/// <summary>
		/// Generate combined stats for both PSN and XBL accomplishments.
		/// </summary>
		/// <param name="psnFilePath">Full path to the generated trophies output.</param>
		/// <param name="xblFilePath">Full path to the generated achievements output.</param>
		public static VideoGameStats CombinedStats(string psnFilePath, string xblFilePath)
		{
			var psnXml = XDocument.Load(psnFilePath);
			var xblXml = XDocument.Load(xblFilePath);

			var accomplishments = VideoGameAccomplishment.LoadPsnXml(psnXml)
				.Concat(VideoGameAccomplishment.LoadXblXml(xblXml));

			return new VideoGameStats(accomplishments);
		}
	}
}
