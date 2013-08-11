﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace VideoGamesSpa.ApiParser.Models
{
	public class Spa
	{
		/// <summary>
		/// Directory to save data generated.
		/// </summary>
		public string SpaDirectory { get; set; }
		/// <summary>
		/// One or more network generators to use for data.
		/// </summary>
		public List<BaseGenerator> Generators { get; set; }
		/// <summary>
		/// Return whether everything necessary has been provided.
		/// </summary>
		public bool Ready
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.SpaDirectory) && this.Generators != null && this.Generators.Count > 0;
			}
		}

		public Spa()
		{
			this.Generators = new List<BaseGenerator>();
		}

		/// <summary>
		/// Generate all files for the Video Games Spa site.
		/// </summary>
		/// <returns>True if all processes run successfully.</returns>
		public bool GenerateAll()
		{
			if (this.Ready)
			{
				foreach (var generator in this.Generators)
				{
					generator.Run();
					// todo, make this pretty
					if (generator.GetType().Namespace.Contains("PsnApiAr"))
					{
						var psnXmlSerializer = new XmlSerializer(((PsnApiAr.Generator)generator).Games.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_psnGames.xml"))
						{
							psnXmlSerializer.Serialize(writer, ((PsnApiAr.Generator)generator).Games);
						}

						JavaScriptSerializer serializer = new JavaScriptSerializer();
						var contents = serializer.Serialize(new { PsnGames = ((PsnApiAr.Generator)generator).Games });
						File.WriteAllText(this.SpaDirectory + "_psnGames.xml" + ".json", contents);
					}
				}
			}
			return false;
			//throw new NotImplementedException();
		}
	}
}
