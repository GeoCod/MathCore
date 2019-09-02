using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���������, ���������� �������� ���������</summary>
    public class SubstractionOperatorNode : OperatorNode
    {
        /// <summary>������������� ������ ��������� ���������</summary>
        public SubstractionOperatorNode() : base("-", 5) { }

        /// <summary>���������� �������� ����</summary>
        /// <returns>�������� �������� �������� ������� � ������ �����������</returns>
        public override double Compute() => (((ComputedNode)Left)?.Compute() ?? 0) - (((ComputedNode)Right)?.Compute() ?? 0);

        /// <summary>���������� ��������� ����</summary>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile()
            => Expression.Subtract
            (
                ((ComputedNode)Left)?.Compile() ?? Expression.Constant(0.0),
                ((ComputedNode)Right)?.Compile() ?? Expression.Constant(0.0)
            );

        /// <summary>���������� ��������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ����</returns>
        /// <returns></returns>
        public override Expression Compile(ParameterExpression[] Parameters) => Left is null
                    ? (Expression)Expression.Negate(((ComputedNode)Right).Compile(Parameters))
                    : Expression.Subtract(((ComputedNode)Left).Compile(Parameters), ((ComputedNode)Right).Compile(Parameters));

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<SubstractionOperatorNode>();
    }
}