using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>���������� ������� ��������������� ���������</summary>
    internal sealed class CharTerm : Term
    {
        /// <summary>���������� �������� ��������</summary>
        public char Value => _Value[0];

        /// <summary>����� ���������� �������</summary>
        /// <param name="c">���������� �������� ��������</param>
        public CharTerm(char c) : base(new string(c, 1)) { }

        /// <summary>�������� ���������</summary>
        /// <param name="Parser">������ ���.���������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>��������� ������ ������ Parser.GetOperatorNode(Value)</returns>
        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression) => Parser.GetOperatorNode(Value);
    }
}