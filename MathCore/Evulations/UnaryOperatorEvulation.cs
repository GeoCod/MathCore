using System;
using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� �������� � ����� ���������</summary>
    /// <typeparam name="TObject">��� �������� ��������</typeparam>
    /// <typeparam name="TValue">��� �������� ����������</typeparam>
    public class UnaryOperatorEvulation<TObject, TValue> : ConvertEvulation<TObject, TValue>
    {
        /// <summary>�����, ������������ ������� �������������� �������� �������� � �������� ����������</summary>
        /// <param name="OP">������� �������������� ���������, ������������ �������� ��������, � ���������, ������������ �������� ����������</param>
        /// <returns></returns>
        protected static Func<TObject, TValue> GetOperation(Func<Ex, Ex> OP)
        {
            var p = Ex.Parameter(typeof(TObject), "p");
            return Ex.Lambda<Func<TObject, TValue>>(OP(p), p).Compile();
        }

        /// <summary>������� �������������� ��������� �������� � ��������� ���������� ��� ���� ���������</summary>
        private readonly Func<Ex, Ex> _Operator;

        /// <summary>������������� ������ �������� ����������</summary>
        /// <param name="Operator">�������� �������������� ��������� �������� � ��������� ����������</param>
        public UnaryOperatorEvulation(Func<Ex, Ex> Operator)
            : base(GetOperation(Operator)) => _Operator = Operator;

        /// <summary>������������� ������ �������� ����������</summary>
        /// <param name="Operator">�������� �������������� ��������� �������� � ��������� ����������</param>
        /// <param name="value">���������� ��������</param>
        public UnaryOperatorEvulation(Func<Ex, Ex> Operator, Evulation<TObject> value)
            : base(value, GetOperation(Operator)) => _Operator = Operator;

        /// <inheritdoc />
        public override Ex GetExpression() => _Operator(InputEvulation.GetExpression());
    }
}