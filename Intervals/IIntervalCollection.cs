using System.Collections.Generic;

namespace Intervals
{
	public interface IIntervalCollection
	{
		void Add(int start, int stop);
		void Remove(int start, int stop);
		IEnumerable<Interval> Items { get; }
	}
}