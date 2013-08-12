using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.Models.Stats
{
	public class AccomplishmentByDayOfWeek
	{
		public int DayOfWeek { get; set; }
		public string DayOfWeekString { get; set; }
		public int Count { get; set; }

		public AccomplishmentByDayOfWeek()
		{
		}
	}
}
