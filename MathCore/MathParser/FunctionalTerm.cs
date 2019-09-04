using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>���� ����������� �������</summary>
    internal sealed class FunctionalTerm : FunctionTerm
    {
        /// <summary>��������� ���������</summary>
        [NotNull]
        public BlockTerm Parameters { get; set; }

        /// <summary>������������� ����� ������������ ���������</summary>
        /// <param name="Header">��������� �����</param>
        /// <param name="Body">���� �����</param>
        public FunctionalTerm([NotNull] FunctionTerm Header, [NotNull] BlockTerm Body) : base(Header.Name, Body) => Parameters = Header.Block;

        /// <summary>�������� ��������� ������������ ���������</summary>
        /// <param name="Parser">������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>���� ������������ ���������</returns>
        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression)
            => new FunctionalNode(this, Parser, Expression);

        /// <summary>�������������� � ��������� �����</summary>
        /// <returns>��������� ������������� ��������</returns>
        public override string ToString() => $"{Name}{Parameters}{Block}";
    }
}