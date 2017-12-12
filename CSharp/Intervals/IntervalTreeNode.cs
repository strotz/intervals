namespace Intervals
{
	// mostly sintax sugar
	public class IntervalTreeNode : TreeNode<int, Interval>
	{
		public IntervalTreeNode(int start, int stop) : base(new Interval(start, stop), interval => interval.Start)
		{
		}
	}
}