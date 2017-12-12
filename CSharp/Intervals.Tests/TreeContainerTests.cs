using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
	[TestFixture]
	public class TreeContainerTests
	{
		private SortedTreeIntervalCollection _intervals;

		[SetUp]
		public void Setup()
		{
			_intervals = new SortedTreeIntervalCollection();
		}

		[Test]
		public void EmptyTreeTest()
		{
			var result = _intervals.FindPredecessor(123);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void SingleAddTest()
		{
			_intervals.Tree.Add(1, new Interval(1, 2));

			var result = _intervals.Tree.ValuesInOrder.ToList();

			Assert.That(result, Is.Not.Null);
			Assert.That(result[0].Start, Is.EqualTo(1));
		}

		[Test]
		public void SingleExactTest()
		{
			_intervals.Tree.Add(1, new Interval(1, 2));

			var result = _intervals.FindPredecessor(1);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Key, Is.EqualTo(1));
		}

		[Test]
		public void SingleAfterTest()
		{
			_intervals.Add(1, 2);

			var result = _intervals.FindPredecessor(5);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Key, Is.EqualTo(1));
		}

		[Test]
		public void SingleNoPredicessorTest()
		{
			_intervals.Add(2,3);

			var result = _intervals.FindPredecessor(1);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void TreeBestFirstAfterTest()
		{
			_intervals.Add(2, 3);
			_intervals.Tree.Root.Left = new IntervalTreeNode(0, 1);

			var result = _intervals.FindPredecessor(5);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Key, Is.EqualTo(2));
		}

		[Test]
		public void TreeBestLastAfterTest()
		{
			_intervals.Tree.Add(5, new Interval(5, 6));
			var l = _intervals.Tree.Root.Left = new IntervalTreeNode(0, 1);
			l.Right = new IntervalTreeNode(2, 3);

			var e = _intervals.Tree.ValuesInOrder.ToList();
			Assert.That(e[0].Start, Is.EqualTo(0));
			Assert.That(e[1].Start, Is.EqualTo(2));
			Assert.That(e[2].Start, Is.EqualTo(5));

			var result = _intervals.FindPredecessor(4);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Key, Is.EqualTo(2));
		}

	}
}
