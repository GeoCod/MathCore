using System.Diagnostics.Contracts;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>��������� ������� ���������</summary>
    internal class StringTerm : Term
    {
        /// <summary>��� ���������� ��������</summary>
        [NotNull]
        public string Name => _Value;

        /// <summary>����� ��������� �������</summary>
        /// <param name="Name">��� ���������� ��������</param>
        public StringTerm([NotNull] string Name) : base(Name) => Contract.Requires(!string.IsNullOrEmpty(Name));

        /// <summary>��������� ��������, ��������� �� ����-����������</summary>
        /// <param name="Parser">������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>���� ������ � ����������, ���������� �� Expression.Variable[Name]</returns>
        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression)
            => new VariableValueNode(Expression.Variable[Name]);
    }
}