using System;
using System.Collections.Generic;
using System.Linq;

namespace Intervals
{
	// TODO: it is not balanced, AVL or simular will do it 
	public class BinaryTree<TKey, TValue>
		where TKey : IComparable
	{
		private readonly IComparer<TKey> _comparer;

		public BinaryTree()
		{
			Root = null;
			_comparer = Comparer<TKey>.Default;
		}

		public TreeNode<TKey, TValue> Root { get; private set; }

		public TreeNode<TKey, TValue> FindNode(TKey key)
		{
			var current = Root;

			while (current != null)
			{
				var compare = _comparer.Compare(current.Key, key);
				if (compare == 0)
				{
					return current;
				}

				if (compare > 0) // left
				{
					current = current.Left;
				}
				else // right
				{
					current = current.Right;
				}
			}

			return null;
		}

		private TreeNode<TKey, TValue> InsertOrReplaceNode(TreeNode<TKey, TValue> root, TKey key, TValue value)
		{
			if (root == null)
			{
				return new TreeNode<TKey, TValue>(key, value);
			}

			var compare = _comparer.Compare(root.Key, key);
			if (compare == 0)
			{
				var newRoot = new TreeNode<TKey, TValue>(key, value)
				{
					Left = root.Left,
					Right = root.Right
				};
				root = newRoot;
			}
			else if (compare > 0) // left
			{
				root.Left = InsertOrReplaceNode(root.Left, key, value);
			}
			else // right
			{
				root.Right = InsertOrReplaceNode(root.Right, key, value);
			}
			return root;
		}

		private TreeNode<TKey, TValue> RemoveNode(TreeNode<TKey, TValue> root, TKey key)
		{
			if (root == null)
			{
				return null;
			}

			var compare = _comparer.Compare(root.Key, key);
			if (compare > 0) // left
			{
				root.Left = RemoveNode(root.Left, key);
				return root;
			}
			else if (compare < 0) // right
			{
				root.Right = RemoveNode(root.Right, key);
				return root;
			}
			else // remove itself
			{
				if (root.Left == null)
				{
					return root.Right;
				}
				if (root.Right == null)
				{
					return root.Left;
				}

				var replace = FindMinNode(root.Right);

				var newRoot = new TreeNode<TKey, TValue>(replace.Key, replace.Value)
				{
					Left = root.Left,
					Right = RemoveNode(root.Right, replace.Key)
				};

				return newRoot;
			}
		}

		private TreeNode<TKey, TValue> FindMinNode(TreeNode<TKey, TValue> root)
		{
			var current = root;
			while (current.Left != null)
			{
				current = current.Left;
			}
			return current;
		}


		private void ListNodesInOrder(TreeNode<TKey, TValue> root, List<TreeNode<TKey, TValue>> result)
		{
			if (root != null)
			{
				ListNodesInOrder(root.Left, result);
				result.Add(root);
				ListNodesInOrder(root.Right, result);
			}
		}

		public IEnumerable<TValue> ValuesInOrder
		{
			get
			{
				var result = new List<TreeNode<TKey, TValue>>();
				ListNodesInOrder(Root, result);
				return result.Select(item => item.Value);
			}
		}

		public IEnumerable<TKey> KeysInOrder
		{
			get
			{
				var result = new List<TreeNode<TKey, TValue>>();
				ListNodesInOrder(Root, result);
				return result.Select(item => item.Key);
			}
		}

		public void Add(TKey key, TValue value)
		{
			Root = InsertOrReplaceNode(Root, key, value);
		}

		public void Remove(TKey key)
		{
			Root = RemoveNode(Root, key);
		}
	}
}