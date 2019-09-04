using System.Linq.Expressions;
using MathCore.Annotations;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���������� ���� ������ ��������������� ���������</summary>
    public abstract class ComputedNode : ExpressionTreeNode
    {
        /// <summary>��������� �������� ���������</summary>
        /// <returns>��������� �������� ���������</returns>
        public abstract double Compute();

        /// <summary>�������������� � ���������</summary>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        [NotNull]
        public abstract Expression Compile();

        /// <summary>�������������� � ���������</summary>
        /// <param name="Parameters">������ ����������</param>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        [NotNull]
        public abstract Expression Compile([NotNull] params ParameterExpression[] Parameters);
    }
}