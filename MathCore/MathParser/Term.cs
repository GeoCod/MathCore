using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>������� ��������������� ���������</summary>
    internal abstract class Term
    {
        /// <summary>��������� ����������</summary>
        protected string _Value;

        /// <summary>����������� �������� ��������������� ���������</summary>
        /// <param name="Value">��������� ����������</param>
        protected Term(string Value) => _Value = Value;

        /// <summary>����� ���������� ��������� ��� ������� �������� ��������������� ���������</summary>
        /// <param name="Parser">������ ��������������� ���������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>���� ������ ���.���������, ���������� ���������� ��� ������� �������� ���.���������</returns>
        [NotNull]
        public abstract ExpressionTreeNode GetSubTree([NotNull] ExpressionParser Parser, [NotNull] MathExpression Expression);

        /// <summary>��������� ������������� �������� ���.���������</summary>
        /// <returns>��������� ���������� ������� ���.���������</returns>
        public override string ToString() => _Value;
    }
}