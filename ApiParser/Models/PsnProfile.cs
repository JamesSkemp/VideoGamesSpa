namespace VideoGamesSpa.ApiParser.Models
{
	public class PsnProfile
	{
		public string Id { get; set; }
		public string Pic { get; set; }
		public int Points { get; set; }
		public int PossiblePoints { get; set; }
		public int Level { get; set; }
		public double LevelProgress { get; set; }
		public int NextLevelPoints { get; set; }
		public int Trophies { get; set; }
		public int TrophiesBronze { get; set; }
		public int TrophiesSilver { get; set; }
		public int TrophiesGold { get; set; }
		public int TrophiesPlatinum { get; set; }
		public int PossibleTrophies { get; set; }
		public int PossibleTrophiesBronze { get; set; }
		public int PossibleTrophiesSilver { get; set; }
		public int PossibleTrophiesGold { get; set; }
		public int PossibleTrophiesPlatinum { get; set; }
		public double CompletionPercent { get; set; }
		public int TotalGames { get; set; }

		public PsnProfile()
		{
		}

		public void Serialize(System.IO.TextWriter writer)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
			serializer.Serialize(writer, this);
		}
	}
}
