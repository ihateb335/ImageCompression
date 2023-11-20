using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.Huffman
{
    /// <summary>
    /// Node parent side
    /// </summary>
    public enum Side
    {
        Left,
        Right
    }

    /// <summary>
    /// Node of binary tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T>
    {
        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="data">Data</param>
        public BinaryTreeNode(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Data in the node
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Left branch
        /// </summary>
        public BinaryTreeNode<T> LeftNode { get; set; }

        /// <summary>
        /// Right branch
        /// </summary>
        public BinaryTreeNode<T> RightNode { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public BinaryTreeNode<T> ParentNode { get; set; }

        /// <summary>
        /// Side
        /// </summary>
        public Side? NodeSide =>
            ParentNode == null
            ? (Side?)null
            : ParentNode.LeftNode == this
                ? Side.Left
                : Side.Right;

        public bool IsLeaf => LeftNode == null && RightNode == null;
        public bool IsOneSideBranch => LeftNode == null || RightNode == null;

        /// <summary>
        /// To string method
        /// </summary>
        /// <returns>Data.ToString()</returns>
        public override string ToString() => Data.ToString();
        public override bool Equals(object obj)
        {
            var other = obj as BinaryTreeNode<T>;
            return (Data == null && other == null ) ||
                Data.Equals(other.Data) &&
                ( ( LeftNode == null && other.LeftNode == null) || LeftNode.Equals(other.LeftNode) ) &&
                ( (RightNode == null && other.RightNode == null) || RightNode.Equals(other.RightNode));
        }
    }
}
