using System;

namespace Intervals
{
	public class TreeNode<TKey, TValue>
		where TKey : IComparable
	{
		public TreeNode(TValue value, Func<TValue, TKey> extracFunc)
		{
			Value = value;
			Key = extracFunc(value);
		}

		internal TreeNode(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public TKey Key { get; }
		public TValue Value { get; } 

		public TreeNode<TKey, TValue> Left { get; internal set; }
		public TreeNode<TKey, TValue> Right { get; internal set; }
	}
}