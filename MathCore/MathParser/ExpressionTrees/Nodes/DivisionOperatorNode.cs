using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, ����������� �������� �������</summary>
    public class DivisionOperatorNode : OperatorNode
    {
        public const string NodeName = "/";

        /// <summary>������������� ���� ��������� �������</summary>
        public DivisionOperatorNode() : base(NodeName, 15) { }

        /// <summary>���������� �������� ����</summary>
        /// <returns>�������� ����</returns>
        public override double Compute() => (((ComputedNode) Left)?.Compute() ?? 1) / ((ComputedNode)Right).Compute();

        /// <summary>���������� ����</summary>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile() => Expression.Divide(((ComputedNode) Left)?.Compile() ?? Expression.Constant(1.0), ((ComputedNode)Right).Compile());

        /// <summary>���������� ���� � �����������</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile(params ParameterExpression[] Parameters) => Expression.Divide(((ComputedNode) Left)?.Compile(Parameters) ?? Expression.Constant(1.0), ((ComputedNode)Right).Compile(Parameters));

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� �������������</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<DivisionOperatorNode>();
    }
}