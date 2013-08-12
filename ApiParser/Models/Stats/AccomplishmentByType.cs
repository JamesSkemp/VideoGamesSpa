using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models.Stats
{
	/// <summary>
	/// Type of accomplishment, with the number earned and unearned.
	/// </summary>
	public class AccomplishmentByType
	{
		public string Type { get; set; }
		public int Earned { get; set; }
		public int Unearned { get; set; }
		public int Total { get; set; }

		public AccomplishmentByType()
		{
		}
	}
}
