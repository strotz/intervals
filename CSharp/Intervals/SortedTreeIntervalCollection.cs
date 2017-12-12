using System;
using System.Collections.Generic;
using System.Linq;

namespace Intervals
{
	public class SortedTreeIntervalCollection : IIntervalCollection
	{
		private readonly BinaryTree<int, Interval> _tree;

		public SortedTreeIntervalCollection()
		{
			_tree = new BinaryTree<int, Interval>();
		}

		internal BinaryTree<int, Interval> Tree => _tree;

		// find interval that starts earlier or in the same time as requested
		// O(log n) complexity on BST
		internal TreeNode<int, Interval> FindPredecessor(int start)
		{
			int? bestDistance = null;
			var bestPredecessor = _tree.Root;

			var current = _tree.Root;
			while (current != null)
			{
				if (current.Key > start)
				{
					current = current.Left;
				}
				else if (current.Key == start) // found exact match 
				{
					return current;
				}
				else
				{
					var currentDistance = start - current.Key; // 
					if (!bestDistance.HasValue || currentDistance < bestDistance.Value)
					{
						bestPredecessor = current;
						bestDistance = currentDistance;
					}
					current = current.Right;
				}
			}
			if (bestDistance.HasValue)
			{
				return bestPredecessor;
			}
			else
			{
				return null;
			}
		}

		public void Add(int start, int stop)
		{
			if (start >= stop)
			{
				throw new ArgumentException($"Incorrect interval from {start} to {stop}");
			}

			if (_tree.Root == null) // no need to validate
			{
				_tree.Add(start, new Interval(start, stop));
				return;
			}

			var predecessor = FindPredecessor(start);
			if (predecessor != null)
			{
				if (predecessor.Value.Stop >= stop) // optimization, interval completelly contained
				{
					return;
				}
				if (predecessor.Value.Stop >= start) // merge predecessor with new interval
				{
					start = predecessor.Value.Start;
				}
			}

			var last = FindPredecessor(stop);
			if (last != null && last.Value.Stop > stop) // extend requested interval
			{
				stop = last.Value.Stop;
			}

			CleanOverlaps(start, stop);

			_tree.Add(start, new Interval(start, stop));
		}

		private void CleanOverlaps(int start, int stop)
		{
			// TODO: based on implementation of the container, it could be optimized
			var keysToDelete = _tree.KeysInOrder
				.Where(k => k >= start && k <= stop)
				.ToList();

			foreach (var key in keysToDelete)
			{
				_tree.Remove(key);
			}
		}

		public void Remove(int start, int stop)
		{
			if (start >= stop)
			{
				throw new ArgumentException($"Incorrect interval from {start} to {stop}");
			}

			if (_tree.Root == null)
			{
				return;
			}

			Interval head = null;
			Interval tail = null;

			var predecessor = FindPredecessor(start);
			if (predecessor != null && predecessor.Value.Start < start && predecessor.Value.Stop > start)
			{
				head = new Interval(predecessor.Value.Start, start);
				start = predecessor.Value.Start;
			}

			var last = FindPredecessor(stop);
			if (last != null && last.Value.Stop > stop)
			{
				tail = new Interval(stop, last.Value.Stop);
			}

			CleanOverlaps(start, stop);

			if (head != null)
			{
				_tree.Add(head.Start, head);
			}

			if (tail != null)
			{
				_tree.Add(tail.Start, tail);
			}
		}

		public IEnumerable<Interval> Items => _tree.ValuesInOrder;
	}
}
