using System.Collections;
using System.Collections.Generic;
using MathCore.Annotations;

// ReSharper disable ClassCanBeSealed.Global

namespace MathCore.MathParser
{
    /// <summary>��������� ������������</summary>
    public class FunctionalsCollection : IEnumerable<Functional>
    {
        /// <summary>������ ������������</summary>
        [NotNull]
        private readonly List<Functional> _Operators = new List<Functional>();

        /// <summary>������ �� �������������� ���������, � ������� ������� ���������</summary>
        [NotNull]
        private readonly MathExpression _Expression;

        /// <summary>���������� ������������ � ���������</summary>
        public int Count => _Operators.Count;

        /// <summary>������������� ����� ��������� ������������</summary>
        /// <param name="Expression">�������������� ���������, �� ������� ��������� ���������</param>
        public FunctionalsCollection([NotNull] MathExpression Expression) => _Expression = Expression;

        /// <summary>�������� ���������� � ���������</summary>
        /// <param name="Operator">����������� ����������</param>
        /// <returns>������, ���� ���������� ������ �������</returns>
        public bool Add([NotNull] Functional Operator)
        {
            if(_Operators.Exists(o => o.Name == Operator.Name)) return false;
            _Operators.Add(Operator);
            return true;
        }

        IEnumerator<Functional> IEnumerable<Functional>.GetEnumerator() => _Operators.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Operators).GetEnumerator();

        public override string ToString() => $"Complex operator collection > count = {Count}";
    }
}