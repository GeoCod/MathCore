using System;
using System.Linq.Expressions;
using MathCore.Extensions.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���������, ����������� �������� ��������</summary>
    public class AdditionOperatorNode : OperatorNode
    {
        public const string NodeName = "+";

        /// <summary>����� �������� ��������</summary>
        public AdditionOperatorNode() : base(NodeName, 0) { }

        /// <summary>���������� ����</summary>
        /// <returns>����� �����������</returns>
        public override double Compute() => (((ComputedNode)Left)?.Compute() ?? 0) + ((ComputedNode)Right)?.Compute() ?? 0;

        /// <summary>���������� ����</summary>
        /// <returns>Linq.Expression.Add()</returns>
        public override Expression Compile() =>
            (((ComputedNode)Left)?.Compile() ?? 0d.ToExpression())
                .Add(((ComputedNode)Right)?.Compile() ?? 0d.ToExpression());

        /// <summary>���������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>Linq.Expression.Add()</returns>
        public override Expression Compile(params ParameterExpression[] Parameters) =>
            (((ComputedNode)Left)?.Compile(Parameters) ?? 0d.ToExpression())
                .Add(((ComputedNode)Right)?.Compile(Parameters) ?? 0d.ToExpression());

        /// <summary>������������ ����</summary>
        /// <returns>������ ���� ���� � ������� �����������</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<AdditionOperatorNode>();
    }
}