using System;
using System.Linq.Expressions;
// ReSharper disable UnusedMember.Global

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, ����������� �������� ������</summary>
    public class SelectorOperatorNode : OperatorNode
    {
        /// <summary>������������� ������ ���� ��������� ������</summary>
        public SelectorOperatorNode() : base("?", -17) { }

        /// <summary>������������� ������ ���� ��������� ������</summary>
        /// <param name="Left">����� ��������� ���������</param>
        /// <param name="Right">������ ��������� ���������</param>
        public SelectorOperatorNode(ExpressionTreeNode Left, ExpressionTreeNode Right)
            : this()
        {
            this.Left = Left;
            this.Right = Right;
        }

        /// <summary>���������� �������� ����</summary>
        /// <returns></returns>
        public override double Compute()
        {
            var variants = (VariantOperatorNode)Right;
            return Math.Abs(((ComputedNode)Left).Compute()) > 0
                        ? ((ComputedNode)variants.Left).Compute()
                        : ((ComputedNode)variants.Right).Compute();
        }

        /// <summary>���������� ����</summary>
        /// <returns>���������������� ��������� ������������ ����� �����������</returns>
        public override Expression Compile()
        {
            var variants = (VariantOperatorNode)Right;
            Expression condition;
            if(Left is LogicOperatorNode node)
                condition = node.LogicCompile();
            else
            {
                var comparer = EqualityOperatorNode.GetAbsMethodCall(((ComputedNode)Left).Compile());
                condition = Expression.GreaterThan(comparer, Expression.Constant(0.0));
            }
            return Expression.Condition(condition, ((ComputedNode)variants.Left).Compile(), ((ComputedNode)variants.Right).Compile());
        }

        /// <summary>���������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ������������ ����� �����������</returns>
        public override Expression Compile(ParameterExpression[] Parameters)
        {
            var variants = (VariantOperatorNode)Right;
            Expression condition;
            if(Left is LogicOperatorNode node)
                condition = node.LogicCompile(Parameters);
            else
            {
                var comparer = EqualityOperatorNode.GetAbsMethodCall(((ComputedNode)Left).Compile(Parameters));
                condition = Expression.GreaterThan(comparer, Expression.Constant(0.0));
            }
            return Expression.Condition(condition, ((ComputedNode)variants.Left).Compile(Parameters), ((ComputedNode)variants.Right).Compile(Parameters));
        }

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => CloneOperatorNode<SelectorOperatorNode>();
    }
}