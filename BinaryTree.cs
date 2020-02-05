using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public interface IBinarySearchTree<T>
    {
        void Add(T key);
        bool Contains(T key);
    }

    public class TreeNode<T> where T : IComparable
    {
        public T Value { get; set; }
        public TreeNode<T> Parrent { get; set; }
        public TreeNode<T> Left;
        public TreeNode<T> Right { get; set; }

        public TreeNode() { }
        public TreeNode(T value, TreeNode<T> parrent = null, TreeNode<T> left = null, TreeNode<T> right = null)
        {
            Value = value;
            Parrent = parrent;
            Left = left;
            Right = right;
        }

        public override string ToString() => $"{Value}  P: {Parrent.Value} L: {Left.Value} R{Right.Value}";
    }

    public class BinaryTree<T> : IEnumerable<T>, IBinarySearchTree<T> where T : IComparable
    {
        private TreeNode<T> root;
        private Dictionary<TreeNode<T>, int> branchCount = new Dictionary<TreeNode<T>, int>();

        public T this[int i]
        {
            get
            {
                if (i < 0 || i > branchCount.Count - 1)
                    throw new IndexOutOfRangeException($"BinaryTree dont include index '{i}'.");
                return GetElementAt(root, i);
            }
        }

        private T GetElementAt(TreeNode<T> root, int i)
        {
            var leftBranchCount = 0;
            if (root.Left != null)
                leftBranchCount = branchCount[root.Left];
            if (i == leftBranchCount) 
                return root.Value;
            if (i < leftBranchCount)
                return GetElementAt(root.Left, i);
            else
                return GetElementAt(root.Right, i - leftBranchCount - 1);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            var visited = new HashSet<TreeNode<T>>();
            var t = GetMinElement(root);
            while (t != null)
            {
                if ((t.Left == null || visited.Contains(t.Left)) && !visited.Contains(t))
                {
                    yield return t.Value;
                    visited.Add(t);
                }
                if (t.Left != null && !visited.Contains(t.Left))
                    t = t.Left;
                else if (t.Right != null && !visited.Contains(t.Right))
                    t = t.Right;
                else
                    t = t.Parrent;
            }
        }

        public void Add(T key)
        {
            var element = new TreeNode<T>(key);
            branchCount[element] = 1;
            if (root == null)
            {
                root = element;
                return;
            }
            var next = root;
            branchCount[next]++;
            while (true)
            {
                if (key.CompareTo(next.Value) < 0)
                {
                    if (next.Left != null)
                        next = next.Left;
                    else
                    {
                        next.Left = element;
                        element.Parrent = next;
                        break;
                    }
                }
                else
                {
                    if (next.Right != null)
                        next = next.Right;
                    else
                    {
                        next.Right = element;
                        element.Parrent = next;
                        break;
                    }
                }
                branchCount[next]++;
            }
        }

        public TreeNode<T> GetMinElement(TreeNode<T> root) =>
            root.Left != null ? GetMinElement(root.Left) : root;

        public T GetMinValue(TreeNode<T> root) =>
            root.Left != null ? GetMinValue(root.Left) : root.Value;

        public bool Contains(T key) =>
            Search(key) != null;

        public TreeNode<T> Search(T key) =>
            Search(root, key);
        private TreeNode<T> Search(TreeNode<T> root, T key)
        {
            if (root == null) return null;
            if (key.CompareTo(root.Value) == 0) return root;
            return Search(key.CompareTo(root.Value) < 0 ? root.Left : root.Right, key);
        }
    }
}