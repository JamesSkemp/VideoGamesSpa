namespace VideoGamesSpa.ApiParser.Models
{
	public class XblProfile
	{
		public string Id { get; set; }
		public string Pic { get; set; }
		public int GamerScore { get; set; }
		public int PossibleGamerScore { get; set; }
		public int Achievements { get; set; }
		public int PossibleAchievements { get; set; }
		public double CompletionPercent { get; set; }
		public int TotalGames { get; set; }

		public XblProfile()
		{
		}

		public void Serialize(System.IO.TextWriter writer)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
			serializer.Serialize(writer, this);
		}
	}
}
