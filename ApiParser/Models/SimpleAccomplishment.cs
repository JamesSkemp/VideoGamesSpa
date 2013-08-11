namespace VideoGamesSpa.ApiParser.Models
{
	/// <summary>
	/// Simple accomplishment meant for grouping by a type of trophy/achievement.
	/// </summary>
	public class SimpleAccomplishment
	{
		public string Type { get; set; }
		public int Value { get; set; }
		public int Earned { get; set; }
		public int Total { get; set; }

		public SimpleAccomplishment()
		{
		}
	}
}
