using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace JamesRSkemp.VideoGamesSpa.Web.Models
{
	public class VideoGame
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string System { get; set; }
		public string Notes { get; set; }
		public string Own { get; set; }
		public string Date { get; set; }
		public string Price { get; set; }
		public string Place { get; set; }
		public string SellDate { get; set; }
		public string SellPrice { get; set; }
		public string SellPlace { get; set; }
		public string AddOn { get; set; }
		public string Electronic { get; set; }
		public bool Beat { get; set; }

		public static VideoGame[] GetGames()
		{
			var xml = XDocument.Load("http://media.jamesrskemp.com/xml/video_games.xml");
			var games = from game in xml.Descendants("game")
						select new VideoGame
						{
							Id = int.Parse(game.Attribute("id").Value),
							Title = game.Element("title").Value,
							System = string.Concat(game.Element("system").Element("console").Value, " ", game.Element("system").Element("version").Value).Trim(),
							Notes = game.Element("notes").Value,
							Own = game.Element("own").Value,
							Date = game.Element("purchase").Element("date").Value,
							Price = game.Element("purchase").Element("price").Value,
							Place = game.Element("purchase").Element("place").Value,
							SellDate = game.Element("sell") == null || game.Element("sell").Element("date") == null ? "" : game.Element("sell").Element("date").Value,
							SellPrice = game.Element("sell") == null || game.Element("sell").Element("price") == null ? "" : game.Element("sell").Element("price").Value,
							SellPlace = game.Element("sell") == null || game.Element("sell").Element("place") == null ? "" : game.Element("sell").Element("place").Value,
							AddOn = game.Attribute("addOn") != null ? game.Attribute("addOn").Value : "",
							Electronic = game.Attribute("electronic") != null ? game.Attribute("electronic").Value : "",
							Beat = game.Attribute("beat") != null ? Boolean.Parse(game.Attribute("beat").Value) : false,
						};
			return games.ToArray();
		}
	}
}