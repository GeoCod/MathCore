using System.Diagnostics.Contracts;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>�������������� ������� ���������</summary>
    internal class FunctionTerm : StringTerm
    {
        /// <summary>���� �� ��������</summary>
        [NotNull]
        public BlockTerm Block { get; set; }

        /// <summary>����� �������������� ������� ���������</summary>
        /// <param name="StrTerm">��������� ������� ���������</param>
        /// <param name="Block">���� ���������</param>
        public FunctionTerm([NotNull] StringTerm StrTerm, [NotNull] BlockTerm Block) : this(StrTerm.Name, Block)
        {
            Contract.Requires(StrTerm != null);
            Contract.Requires(Block != null);
        }

        public FunctionTerm([NotNull] string Name, [NotNull] BlockTerm Block) : base(Name)
        {
            Contract.Requires(!string.IsNullOrEmpty(Name));
            Contract.Requires(Block != null);
            this.Block = Block;
        }

        /// <summary>�������� ���������</summary>
        /// <param name="Parser">������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>���� �������</returns>
        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression)
            => new FunctionNode(this, Parser, Expression);

        /// <summary>�������������� � ��������� �����</summary>
        /// <returns>��������� ������������� ��������</returns>
        public override string ToString() => $"{Name}{Block?.ToString() ?? ""}";
    }
}