using System;
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
					if (generator.GetType().Namespace.EndsWith("PsnApiAr"))
					{
						#region Games
						var psnXmlSerializerGames = new XmlSerializer(((PsnApiAr.Generator)generator).Games.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_psnGames.xml"))
						{
							psnXmlSerializerGames.Serialize(writer, ((PsnApiAr.Generator)generator).Games);
						}

						JavaScriptSerializer serializerGames = new JavaScriptSerializer();
						var contentsGames = serializerGames.Serialize(new { PsnGames = ((PsnApiAr.Generator)generator).Games });
						File.WriteAllText(this.SpaDirectory + "_psnGames.xml" + ".json", contentsGames);
						#endregion

						#region Trophies
						var psnXmlSerializerTrophies = new XmlSerializer(((PsnApiAr.Generator)generator).Trophies.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_psnTrophies.xml"))
						{
							//StreamWriter writer = new StreamWriter(outputDirectory + "_psnTrophies.xml");
							psnXmlSerializerTrophies.Serialize(writer, ((PsnApiAr.Generator)generator).Trophies);
						}

						JavaScriptSerializer serializerTrophies = new JavaScriptSerializer();
						serializerTrophies.MaxJsonLength = Int32.MaxValue;
						var contentsTrophies = serializerTrophies.Serialize(new { PsnTrophies = ((PsnApiAr.Generator)generator).Trophies });
						File.WriteAllText(this.SpaDirectory + "_psnTrophies.xml" + ".json", contentsTrophies);
						#endregion

						#region Games Basic
						var psnXmlSerializerGamesBasic = new XmlSerializer(((PsnApiAr.Generator)generator).GamesBasic.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_psnGamesBasic.xml"))
						{
							psnXmlSerializerGamesBasic.Serialize(writer, ((PsnApiAr.Generator)generator).GamesBasic);
						}

						JavaScriptSerializer psnSerializerGamesBasic = new JavaScriptSerializer();
						var psnContents = psnSerializerGamesBasic.Serialize(new { PsnGamesBasic = ((PsnApiAr.Generator)generator).GamesBasic });
						File.WriteAllText(this.SpaDirectory + "_psnGamesBasic.xml" + ".json", psnContents);
						#endregion

						#region Profile
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_psnProfile.xml"))
						{
							(((PsnApiAr.Generator)generator).Profile).Serialize(writer);
						}
						JavaScriptSerializer psnSerializer = new JavaScriptSerializer();
						var psnContentsProfile = psnSerializer.Serialize(new { PsnProfile = ((PsnApiAr.Generator)generator).Profile });
						File.WriteAllText(this.SpaDirectory + "_psnProfile.xml" + ".json", psnContentsProfile);
						#endregion

						#region Stats
						using (StreamWriter statsWriter = new StreamWriter(this.SpaDirectory + "_psnStats.xml"))
						{
							((PsnApiAr.Generator)generator).Stats.Serialize(statsWriter);
						}

						JavaScriptSerializer serializer = new JavaScriptSerializer();
						var contents = serializer.Serialize(new { PsnStats = ((PsnApiAr.Generator)generator).Stats });
						File.WriteAllText(this.SpaDirectory + "_psnStats.xml" + ".json", contents);
						#endregion
					}
					else if (generator.GetType().Namespace.EndsWith("XboxApi"))
					{
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
					else if (generator.GetType().Namespace.EndsWith("XboxLeaders"))
					{
						#region Games
						var xblXmlSerializerGames = new XmlSerializer(((XboxLeaders.Generator)generator).Games.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_xblGames.xml"))
						{
							xblXmlSerializerGames.Serialize(writer, ((XboxLeaders.Generator)generator).Games);
						}

						JavaScriptSerializer serializerGames = new JavaScriptSerializer();
						var contentsGames = serializerGames.Serialize(new { XblGames = ((XboxLeaders.Generator)generator).Games });
						File.WriteAllText(this.SpaDirectory + "_xblGames.xml" + ".json", contentsGames);
						#endregion

						#region Achievements
						var xblXmlSerializerAchievements = new XmlSerializer(((XboxLeaders.Generator)generator).Achievements.GetType());
						using (StreamWriter writer = new StreamWriter(this.SpaDirectory + "_xblAchievements.xml"))
						{
							xblXmlSerializerAchievements.Serialize(writer, ((XboxLeaders.Generator)generator).Achievements);
						}

						JavaScriptSerializer serializerAchievements = new JavaScriptSerializer();
						var contentsAchievements = serializerAchievements.Serialize(new { XblAchievements = ((XboxLeaders.Generator)generator).Achievements });
						File.WriteAllText(this.SpaDirectory + "_xblAchievements.xml" + ".json", contentsAchievements);
						#endregion

						// todo
						#region Games Basic
						var xblXmlSerializerGamesBasic = new XmlSerializer(((XboxLeaders.Generator)generator).GamesBasic.GetType());
						using (StreamWriter xblWriter = new StreamWriter(this.SpaDirectory + "_xblGamesBasic.xml"))
						{
							xblXmlSerializerGamesBasic.Serialize(xblWriter, ((XboxLeaders.Generator)generator).GamesBasic);
						}

						JavaScriptSerializer xblSerializer = new JavaScriptSerializer();
						var xblContents = xblSerializer.Serialize(new { XblGamesBasic = ((XboxLeaders.Generator)generator).GamesBasic });
						File.WriteAllText(this.SpaDirectory + "_xblGamesBasic.xml" + ".json", xblContents);
						#endregion

						#region Profile
						using (StreamWriter xblWriter = new StreamWriter(this.SpaDirectory + "_xblProfile.xml"))
						{
							(((XboxLeaders.Generator)generator).Profile).Serialize(xblWriter);
						}
						JavaScriptSerializer xblSerializerProfile = new JavaScriptSerializer();
						var xblContentsProfile = xblSerializerProfile.Serialize(new { XblProfile = ((XboxLeaders.Generator)generator).Profile });
						File.WriteAllText(this.SpaDirectory + "_xblProfile.xml" + ".json", xblContentsProfile);
						#endregion

						#region Stats
						using (StreamWriter statsWriter = new StreamWriter(this.SpaDirectory + "_xblStats.xml"))
						{
							((XboxLeaders.Generator)generator).Stats.Serialize(statsWriter);
						}

						JavaScriptSerializer serializerStats = new JavaScriptSerializer();
						var contents = serializerStats.Serialize(new { XblStats = ((XboxLeaders.Generator)generator).Stats });
						File.WriteAllText(this.SpaDirectory + "_xblStats.xml" + ".json", contents);
						#endregion
					}
				}
			}
			return false;
			//throw new NotImplementedException();
		}
	}
}
