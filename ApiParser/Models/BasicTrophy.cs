using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// PlayStation Network trophy, with basic information.
	/// </summary>
	public class BasicTrophy
	{
		public string Id { get; set; }
		/// <summary>
		/// Title of the trophy.
		/// </summary>
		public string Title { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public DateTime? Earned { get; set; }
		public string Type { get; set; }

		public BasicTrophy()
		{
		}
	}
}
