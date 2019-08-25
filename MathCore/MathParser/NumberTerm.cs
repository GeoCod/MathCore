using System;
using System.Diagnostics.Contracts;
using System.Linq;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;

namespace MathCore.MathParser
{
    /// <summary>�������� ������� ��������������� ���������</summary>
    sealed class NumberTerm : Term
    {
        /// <summary>��������� �������� ��������</summary>
        private int _IntValue;

        /// <summary>��������� �������� ��������</summary>
        public int Value
        {
            get => _IntValue;
            set
            {
                _IntValue = value;
                _Value = value.ToString();
            }
        }

        /// <summary>����� ��������� ������� ���.���������</summary>
        /// <param name="Str">��������� �������� ��������</param>
        public NumberTerm([NotNull] string Str) : base(Str)
        {
            Contract.Requires(!string.IsNullOrEmpty(Str));
            Contract.Requires(Str.All(char.IsDigit));
            _IntValue = int.Parse(Str);
        }

        public NumberTerm(int Value) : base(Value.ToString()) => _IntValue = Value;

        /// <summary>������ ���������</summary>
        /// <param name="Parser">������</param>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>���� ������������ ��������</returns>
        public override ExpressionTreeNode GetSubTree(ExpressionParser Parser, MathExpression Expression)
            => new ConstValueNode(_IntValue);

        /// <summary>���������� �������� ������� �������� �����</summary>
        /// <param name="node">���� ���������</param>
        /// <param name="SeparatorTerm">���� �����������</param>
        /// <param name="DecimalSeparator">���� � ����� ������ �����</param>
        /// <param name="FrationPartTerm">���� � ������� ������ �����</param>
        /// <returns>������, ���� �������� ��������� �������. ����, ���� � ����������� ������ �� ���������� ������ ����������</returns>
        public static bool TryAddFractionPart(ref ExpressionTreeNode node, Term SeparatorTerm, char DecimalSeparator, Term FrationPartTerm)
        {
            var value = node as ConstValueNode;
            if(value == null) throw new ArgumentException("�������� ��� ���� ������");
            var separator = SeparatorTerm as CharTerm;
            if(separator == null || separator.Value != DecimalSeparator) return false;
            var fraction = FrationPartTerm as NumberTerm;
            if(fraction == null) return false;

            var v_value = fraction.Value;
            if(v_value == 0) return true;
            node = new ConstValueNode(value.Value + v_value / Math.Pow(10, Math.Truncate(Math.Log10(v_value)) + 1));
            return true;
        }
    }
}