using System;
using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ��������������� ���������, ����������� �������� ���������� � �������</summary>
    public class PowerOperatorNode : OperatorNode
    {
        /// <summary>������������� ������ ���� ��������� ���������� � �������</summary>
        public PowerOperatorNode() : base("^", 20) { }

        /// <summary>���������� ���� ���������</summary>
        /// <returns>���������� �������� ����� ������ ��������� � ������� �������� ����� ������� ���������</returns>
        public override double Compute() => Math.Pow(((ComputedNode)Left).Compute(), ((ComputedNode)Right).Compute());

        /// <summary>���������� ��������� ����</summary>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile() => Expression.Power(((ComputedNode)Left).Compile(), ((ComputedNode)Right).Compile());

        /// <summary>���������� ��������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => 
            Expression.Power(((ComputedNode)Left).Compile(Parameters), ((ComputedNode)Right).Compile(Parameters));

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<PowerOperatorNode>();
    }
}