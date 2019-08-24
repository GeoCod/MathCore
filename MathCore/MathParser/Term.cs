using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>������� ��������������� ���������</summary>
    [ContractClass(typeof(TermContract))]
    abstract class Term
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

    [ContractClassFor(typeof(Term)), ExcludeFromCodeCoverage]
    abstract class TermContract : Term
    {
        private TermContract(string Value) : base(Value) { }

        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression)
        {
            Contract.Requires(Parser != null);
            Contract.Requires(Expression != null);
            Contract.Ensures(Contract.Result<ExpressionTreeNode>() != null);
            throw new System.NotImplementedException();
        }
    }
}