using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models
{
	public class PlayedGame
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public DateTime LastPlayed { get; set; }
		public double Progress { get; set; }
		public int EarnedPoints { get; set; }
		public int EarnedAccomplishments { get; set; }
		public int TotalPoints { get; set; }
		public int TotalAccomplishments { get; set; }
		public string Platform { get; set; }
		public List<SimpleAccomplishment> Accomplishments { get; set; }

		public PlayedGame()
		{
			this.Accomplishments = new List<SimpleAccomplishment>();
		}
	}
}
