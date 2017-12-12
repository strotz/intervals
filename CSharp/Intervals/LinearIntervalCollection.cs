using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Intervals
{
	public class LinearIntervalCollection : IIntervalCollection
	{
		private readonly LinkedList<Interval> _items;

		public LinearIntervalCollection()
		{
			_items = new LinkedList<Interval>();
		}

		// for possible optimization
		private LinkedListNode<Interval> RawAddFirst(int start, int stop)
		{
			return _items.AddFirst(new Interval(start, stop));
		}

		// 
		private LinkedListNode<Interval> RawAddAfter(LinkedListNode<Interval> node, int start, int stop)
		{
			return _items.AddAfter(node, new Interval(start, stop));
		}

		private void RawRemove(LinkedListNode<Interval> node)
		{
			_items.Remove(node);
		}
	
		/// <summary>
		/// Find node that containes interval started the last before given time
		/// </summary>
		/// <param name="start"></param>
		/// <returns></returns>
		internal LinkedListNode<Interval> FindPredecessor(int start)
		{
			var current = _items.First;
			if (current == null)
			{
				return null;
			}

			if (current.Value.Start > start)
			{
				return null;
			}

			while (current.Next != null && current.Next.Value.Start <= start)
			{
				current = current.Next;
			}

			return current;
		}

		public void Add(int start, int stop)
		{
			if (start >= stop)
			{
				throw new ArgumentException($"Incorrect interval from {start} to {stop}");
			}

			if (_items.Count == 0) // optimization, no need to validate
			{
				RawAddFirst(start, stop);
				return; 
			}

			var currentNode = FindPredecessor(start);
			if (currentNode != null && currentNode.Value.Stop >= stop) // optimization, interval contained
			{
				return;
			}

			if (currentNode == null) // first item 
			{
				currentNode = RawAddFirst(start, stop);
			}
			else if (currentNode.Value.Stop < start) // intervals ends before requested start
			{
				currentNode = RawAddAfter(currentNode, start, stop);
			}
			else // extend interval
			{
				currentNode.Value.Stop = stop;
			}

			while (currentNode.Next != null && currentNode.Next.Value.Start <= stop) // clean the tail
			{
				if (currentNode.Next.Value.Stop > stop) // merge and extend
				{
					currentNode.Value.Stop = currentNode.Next.Value.Stop;
				}
				RawRemove(currentNode.Next);
			}
		}

		public void Remove(int start, int stop)
		{
			if (start >= stop)
			{
				throw new ArgumentException($"Incorrect interval from {start} to {stop}");
			}

			if (_items.Count == 0) // optimization
  			{
				return;
			}

			var currentNode = FindPredecessor(start);
			if (currentNode != null && currentNode.Value.Start == start && currentNode.Value.Stop == stop) // optimization, nothing to merge
			{
				RawRemove(currentNode);
				return;
			}

			if (currentNode == null) // first
			{
				currentNode = _items.First;
			}
			else if (currentNode.Value.Stop <= start) // nothing to cut
			{
				currentNode = currentNode.Next;
			}
			else if (currentNode.Value.Stop <= stop) // cut the tail
			{ 	
				currentNode.Value.Stop = start;
				currentNode = currentNode.Next;
			}
			else // finally, split
			{
				var currentStop = currentNode.Value.Stop;
				currentNode.Value.Stop = start;
				currentNode = RawAddAfter(currentNode, stop, currentStop);
			}

			while (currentNode != null && currentNode.Value.Start <= stop) // clean the tail
			{
				if (currentNode.Value.Stop <= stop) // completelly in, remove
				{
					var toRemove = currentNode;
					currentNode = currentNode.Next;
					RawRemove(toRemove);
				}
				else // cut head
				{
					RawAddAfter(currentNode, stop, currentNode.Value.Stop);
					RawRemove(currentNode);
					return;
				}
			}
		}

		public IEnumerable<Interval> Items => _items;
	}
}
