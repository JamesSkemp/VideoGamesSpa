using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models.Stats
{
	public class AccomplishmentByYearMonth
	{
		public DateTime YearMonth { get; set; }
		public int Count { get; set; }

		public AccomplishmentByYearMonth()
		{
		}
	}
}
