using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models.Stats
{
	public class AccomplishmentByDate
	{
		public DateTime Date { get; set; }
		public int Count { get; set; }
		public int TotalCount { get; set; }

		public AccomplishmentByDate()
		{
		}
	}
}
