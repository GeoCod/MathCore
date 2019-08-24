using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser.ExpressionTrees
{
    /// <summary>������ ���������</summary>
    public sealed class ExpressionTree : IDisposable, ICloneable<ExpressionTree>, IEnumerable<ExpressionTreeNode>
    {
        /* --------------------------------------------------------------------------------------------- */

        /// <summary>����� ������ ������</summary>
        public enum BypassingType
        {
            /// <summary>����� ���������, ������ ��������, ������</summary>
            LeftRightRoot,
            /// <summary>����� ���������, ������, ������ ���������</summary>
            LeftRootRight,
            /// <summary>������, ����� ���������, ������ ���������</summary>
            RootLeftRight,
            /// <summary>������ ���������, ����� ���������, ������</summary>
            RightLeftRoot,
            /// <summary>������ ���������, ������, ����� ���������</summary>
            RightRootLeft,
            /// <summary>������, ������ ���������, ����� ���������</summary>
            RootRightLeft
        }

        /* --------------------------------------------------------------------------------------------- */

        /// <summary>������</summary>
        public ExpressionTreeNode Root { get; set; }

        /* --------------------------------------------------------------------------------------------- */

        /// <summary>������������� ������ ������ ��������������� ���������</summary>
        public ExpressionTree() { }

        /// <summary>������������� ������ ������ ��������������� ���������</summary><param name="Root">���� - ������ ������</param>
        public ExpressionTree(ExpressionTreeNode Root) { this.Root = Root; }

        /* --------------------------------------------------------------------------------------------- */

        /// <summary>�������� ������</summary>
        public void Clear()
        {
            var root = Root;
            Root = null;
            root?.Dispose();
        }

        /// <summary>������� ����</summary><param name="Node">��������� ����</param>
        public void Remove([NotNull] ExpressionTreeNode Node)
        {
            Contract.Requires(Node != null);
            // ��������� ������ �� ������ ����
            var parent = Node.Parent;
            // ��������� ������ �� ����������
            var right = Node.Right;
            var left = Node.Left;

            Node.Parent = null;
            Node.Left = null;
            Node.Right = null;

            if(parent == null) // ���� � ���� ��� ������������� ����
                if(Node == Root)    // � ��� ���� �� �������� ������
                {
                    if(left == null)
                    {
                        if(right == null) return;
                        right.Parent = null; // �������� ������ �� ������
                        Root = right;
                        return;
                    }
                    if(right == null) // ���� ��� ������� ���������
                    {
                        left.Parent = null; // �������� ������ � ������ ��������� �� ������
                        Root = left;
                        return;
                    }

                    Root = left;    // ������ ������ ���������� ����� ��������� ���������� ����
                    //�������� � ����� ��������� ����� ������ ����
                    left.LastRightChild
                        .Right = right; // � � ��� ������ ��������� ���������� ������ ��������� ���������� ����
                    return;
                }
                else //���� ���� �� �������� ������ � �� ����� ������, �� ��� ������ - ���� �� ����������� ������
                    throw new ArgumentException("��������� ���� �� ����������� ������");

            // ���� �� �������� ��������.
            if(Node.IsLeftSubtree) // ���� ���� �������� ����� ����������
            {
                if(left == null) // ���� ������ ��������� ���
                    parent.Left = right; // �� ����� ���������� ������������� ���� ����� ������ ���������
                else
                {   //����� - ����� ���������
                    parent.Left = left;
                    // � ����� ������ �������� ���� ������ ��������� �������� ������
                    left.LastRightChild.Right = right;
                }
            }
            else // ����� ���� �������� ������ ����������
            {
                if(right == null) // ���� ������� ��������� ���
                    parent.Right = left; // �� ������ ���������� ������������� ���� ����� ����� ���������
                else
                {   //����� - ������ ���������
                    parent.Right = right;
                    // � ����� ����� �������� ���� ������� ��������� �������� �����
                    right.LastLeftChild.Left = left;
                }
            }
        }

        /// <summary>�������� ����</summary><param name="OldNode">�������� ����</param><param name="NewNode">����� ����</param>
        public void Swap([NotNull] ExpressionTreeNode OldNode, [NotNull] ExpressionTreeNode NewNode)
        {
            Contract.Requires(OldNode != null);
            Contract.Requires(NewNode != null);
            OldNode.SwapTo(NewNode);
            if(Root == OldNode) Root = NewNode;
        }

        /// <summary>����������� ���� ����</summary><param name="Node">������������ ����</param>
        public void MoveParentDown([NotNull] ExpressionTreeNode Node)
        {
            Contract.Requires(Node != null);
            var parent = Node.Parent;
            var is_left_subtree = Node.IsLeftSubtree;

            if(is_left_subtree)
            {
                parent.Left = null;

                if(parent.IsLeftSubtree)
                    parent.Parent.Left = Node;
                else if(parent.IsRightSubtree)
                    parent.Parent.Right = Node;
                else
                    Node.Parent = null;

                var right = Node.Right;
                Node.Right = null;
                if(right != null)
                    right.Parent = null;

                Node.Right = parent;
                parent.Left = right;
            }
            else
            {
                parent.Right = null;

                if(parent.IsLeftSubtree)
                    parent.Parent.Left = Node;
                else if(parent.IsRightSubtree)
                    parent.Parent.Right = Node;
                else
                    Node.Parent = null;

                var left = Node.Left;
                Node.Left = null;
                if(left != null)
                    left.Parent = null;

                Node.Left = parent;
                parent.Right = left;
            }

            if(Root == parent) Root = Node;
        }

        /// <summary>������ ������</summary><param name="type">������ ������</param><returns>������������ ����� ������ �� ���������� ������� ������</returns>
        [NotNull]
        public IEnumerable<ExpressionTreeNode> Bypass(BypassingType type = BypassingType.LeftRootRight)
        {
            Contract.Ensures(Contract.Result<IEnumerable<ExpressionTreeNode>>() != null);
            return Root.Bypassing(type);
        }

        /* --------------------------------------------------------------------------------------------- */

        /// <summary>���������� ������</summary>
        public void Dispose() => Clear();

        /* --------------------------------------------------------------------------------------------- */

        /// <summary>���������� ������ <see cref="T:System.String"/>, ������� ������������ ������� ������ <see cref="T:System.Object"/>.</summary>
        /// <returns>������ <see cref="T:System.String"/>, �������������� ������� ������ <see cref="T:System.Object"/>.</returns>
        [NotNull]
        public override string ToString() => Root?.ToString() ?? "ExpressionTree";

        /// <summary>����������� ������</summary><returns>���� ������</returns>
        [NotNull]
        public ExpressionTree Clone()
        {
            Contract.Ensures(Contract.Result<ExpressionTree>() != null);
            return new ExpressionTree(Root.Clone());
        }

        /// <summary>����������� ������</summary><returns>���� ������</returns>
        [NotNull]
        object ICloneable.Clone()
        {
            Contract.Ensures(Contract.Result<object>() != null);
            return Clone();
        }

        /// <summary>�������� ������������� ����� ������ �� ������ ���</summary><returns>������������� ����� ������ �� ������ ���</returns>
        [NotNull]
        public IEnumerator<ExpressionTreeNode> GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<ExpressionTreeNode>>() != null);
            return Bypass().GetEnumerator();
        }

        [NotNull]
        IEnumerator IEnumerable.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator>() != null);
            return GetEnumerator();
        }
    }
}