namespace Intervals
{
	public class Interval
	{
		public Interval(int start, int stop)
		{
			Start = start;
			Stop = stop;
		}

		public int Start { get; }
		public int Stop { get; set; }
	}
}