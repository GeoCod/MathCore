using System;
using Ex = System.Linq.Expressions.Expression;
using bEx = System.Linq.Expressions.BinaryExpression;
// ReSharper disable UnusedMember.Global

namespace MathCore.Evulations
{
    /// <summary>���������� ��������������� ��������� ��������� ����� ����� ������������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class BinaryFunctionOperatorEvulation<T> : BinaryFunctionOperationEvulation<T>
    {
        /// <summary>����� ��������� �������, ������������� �������� ��������� ��� ���� ��������� �������� ����������</summary>
        /// <param name="OP">�������, ������������� ��� ��������� � �������� ��������, ���������� ��������� �������� �������� ����� ����� ���������� ���������</param>
        /// <returns>����� ���������� �������� ��������� �� ������ �������� ���� ��� ���������</returns>
        protected static Func<T, T, T> GetOperatorFunction(Func<Ex, Ex, bEx> OP)
        {
            var type = typeof(T);
            var pa = Ex.Parameter(type, "a");
            var pb = Ex.Parameter(type, "b");
            return Ex.Lambda<Func<T, T, T>>(OP(pa, pb), pa, pb).Compile();
        }

        /// <summary>������� ������������ ���� ��������� � �������� �������� ������� ��������</summary>
        private readonly Func<Ex, Ex, bEx> _Operator;

        /// <summary>������������� ������ ��������������� ��������� ��������� ����������</summary>
        /// <param name="Operator">�������, ������������ ��� ����������� ��� ��������� ��������� � �������� ��������</param>
        public BinaryFunctionOperatorEvulation(Func<Ex, Ex, bEx> Operator) : base(GetOperatorFunction(Operator)) { _Operator = Operator; }

        /// <summary>������������� ������ ��������������� ��������� ��������� �� ������ ������� ��������� ��������� ��������� � ���� ���������� ���������</summary>
        /// <param name="Operator">�������, ������������ ��� ����������� ��� ��������� ��������� � �������� ��������</param>
        /// <param name="a">���������� ������� ��������</param>
        /// <param name="b">���������� �������� ��������</param>
        public BinaryFunctionOperatorEvulation(Func<Ex, Ex, bEx> Operator, Evulation<T> a, Evulation<T> b) 
            : base(GetOperatorFunction(Operator), a, b)
        {
            _Operator = Operator;
        }

        /// <inheritdoc />
        public override Ex GetExpression() => _Operator(A.GetExpression(), B.GetExpression());
    }
}