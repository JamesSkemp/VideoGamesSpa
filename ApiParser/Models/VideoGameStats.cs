using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using VideoGamesSpa.ApiParser.Models.Stats;

namespace VideoGamesSpa.ApiParser.Models
{
	public class VideoGameStats
	{
		public List<AccomplishmentByDate> AccomplishmentsByDate { get; set; }
		public List<AccomplishmentByYearMonth> AccomplishmentsByYearMonth { get; set; }
		public List<AccomplishmentByDayOfWeek> AccomplishmentsByDayOfWeek { get; set; }
		public List<AccomplishmentByHour> AccomplishmentsByHour { get; set; }
		public List<AccomplishmentByDayOfWeekAndHour> AccomplishmentsByDayOfWeekAndHour { get; set; }
		public List<AccomplishmentByType> AccomplishmentsByType { get; set; }

		public VideoGameStats()
		{
			this.AccomplishmentsByDate = new List<AccomplishmentByDate>();
			this.AccomplishmentsByYearMonth = new List<AccomplishmentByYearMonth>();
			this.AccomplishmentsByDayOfWeek = new List<AccomplishmentByDayOfWeek>();
			this.AccomplishmentsByHour = new List<AccomplishmentByHour>();
			this.AccomplishmentsByDayOfWeekAndHour = new List<AccomplishmentByDayOfWeekAndHour>();
		}

		public VideoGameStats(IEnumerable<VideoGameAccomplishment> accomplishments)
			: this()
		{
			var earnedAccomplishments = accomplishments.Where(a => a.Earned != null);

			int currentCount = 0;
			this.AccomplishmentsByDate = earnedAccomplishments
				.Where(a => a.Earned.Value.Date != new DateTime().Date)
				.GroupBy(a => a.Earned.Value.Date)
				.OrderBy(a => a.Key)
				.Select(a =>
				{
					currentCount += a.Count();
					return new AccomplishmentByDate
					{
						Date = a.Key,
						Count = a.Count(),
						TotalCount = currentCount
					};
				})
				.ToList();

			this.AccomplishmentsByYearMonth = earnedAccomplishments
				.Where(a => a.Earned.Value.Date != new DateTime().Date)
				.GroupBy(a => new DateTime(a.Earned.Value.Year, a.Earned.Value.Month, 1))
				.Select(a => new AccomplishmentByYearMonth { YearMonth = a.Key, Count = a.Count() })
				.OrderBy(a => a.YearMonth)
				.ToList();

			this.AccomplishmentsByDayOfWeek = earnedAccomplishments
				.Where(a => a.Earned.Value.Date != new DateTime().Date)
				.GroupBy(a => a.Earned.Value.DayOfWeek)
				.Select(a => new AccomplishmentByDayOfWeek { DayOfWeek = (int)a.Key, DayOfWeekString = a.Key.ToString(), Count = a.Count() })
				.OrderBy(a => a.DayOfWeek)
				.ToList();

			this.AccomplishmentsByHour = earnedAccomplishments
				.Where(a => a.Earned.Value.Date != new DateTime().Date)
				.GroupBy(a => a.Earned.Value.TimeOfDay.Hours)
				.Select(a => new AccomplishmentByHour { Hour = a.Key, Count = a.Count() })
				.OrderBy(a => a.Hour)
				.ToList();

			this.AccomplishmentsByDayOfWeekAndHour = earnedAccomplishments
				.Where(a => a.Earned.HasValue)
				.GroupBy(a => new { a.Earned.Value.DayOfWeek, a.Earned.Value.Hour })
				.Select(a => new AccomplishmentByDayOfWeekAndHour { DayOfWeek = (int)a.Key.DayOfWeek, DayOfWeekString = a.Key.DayOfWeek.ToString(), Hour = a.Key.Hour, Count = a.Count() })
				.OrderBy(a => a.DayOfWeek).ThenBy(a => a.Hour)
				.ToList();

			this.AccomplishmentsByType = (from a in accomplishments
										  group a by a.Type into typeGroup
										  select new AccomplishmentByType
										  {
											  Type = typeGroup.Key,
											  Earned = typeGroup.Where(a => a.Earned.HasValue).Count(),
											  Unearned = typeGroup.Where(a => !a.Earned.HasValue).Count(),
											  Total = typeGroup.Count()
										  })
										  .OrderBy(a => a.Type)
										  .ToList();
		}

		/// <summary>
		/// Serialize an instance to XML, using a particular writer.
		/// </summary>
		/// <param name="statsWriter">Writer to use to serialize the object.</param>
		public void Serialize(System.IO.TextWriter statsWriter)
		{
			var statsSerializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
			statsSerializer.Serialize(statsWriter, this);
		}

		/// <summary>
		/// Create a new VideoGameStats from a serialized XDocument.
		/// </summary>
		/// <param name="xml">XDocument of a Serialized VideoGameStats.</param>
		public static VideoGameStats Load(XDocument xml)
		{
			XmlSerializer _s = new XmlSerializer(typeof(VideoGameStats));
			return (VideoGameStats)_s.Deserialize(xml.CreateReader());
		}
	}
}