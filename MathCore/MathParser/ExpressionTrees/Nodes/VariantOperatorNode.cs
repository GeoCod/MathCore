using System.Linq.Expressions;
// ReSharper disable UnusedMember.Global

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, ����������� �������� ����������� ���������</summary>
    public class VariantOperatorNode : OperatorNode
    {
        /// <summary>������������� ������ ���� ��������� ����������� ���������</summary>
        public VariantOperatorNode() : base(":", -16) { }

        /// <summary>������������� ������ ���� ��������� ����������� ���������</summary>
        /// <param name="Left">����� ��������� ���������</param>
        /// <param name="Right">������ ��������� ���������</param>
        public VariantOperatorNode(ExpressionTreeNode Left, ExpressionTreeNode Right)
            : this()
        {
            this.Left = Left;
            this.Right = Right;
        }

        /// <summary>���������� �������� ����</summary>
        /// <returns></returns>
        public override double Compute() => ((ComputedNode)Left).Compute();

        /// <summary>���������� ����</summary>
        /// <returns>���������������� ��������� ������������ ����� �����������</returns>
        public override Expression Compile() => ((ComputedNode)Left).Compile();

        /// <summary>���������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ������������ ����� �����������</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => ((ComputedNode)Left).Compile(Parameters);

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<VariantOperatorNode>();
    }
}