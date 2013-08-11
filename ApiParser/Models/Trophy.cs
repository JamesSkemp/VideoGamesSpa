using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// PlayStation Network trophy with basic and additional information.
	/// </summary>
	public class Trophy : BasicTrophy
	{
		public string Id { get; set; }
		/// <summary>
		/// Title of the game this trophy is for.
		/// </summary>
		public string GameTitle { get; set; }
		/// <summary>
		/// Id of the game this trophy is for.
		/// </summary>
		public string GameId { get; set; }

		public Trophy()
			: base()
		{
		}

		/// <summary>
		/// Deserialize XML into a collection of Trophy. 
		/// </summary>
		/// <param name="xml">Serialized XDocument.</param>
		public static List<Trophy> Load(XDocument xml)
		{
			XmlSerializer _s = new XmlSerializer(typeof(List<Trophy>));
			return (List<Trophy>)_s.Deserialize(xml.CreateReader());
		}
	}
}
