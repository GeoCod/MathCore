using System;
using Ex = System.Linq.Expressions.Expression;
// ReSharper disable UnusedMember.Global

namespace MathCore.Evulations
{
    /// <summary>���������� �������� �������� ����� ����� ������������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class BinaryFunctionOperationEvulation<T> : Evulation<T>
    {
        /// <summary>������ ������� ����������</summary>
        public Evulation<T> A { get; set; }

        /// <summary>������ ������� ����������</summary>
        public Evulation<T> B { get; set; }

        /// <summary>����� ���������� �������� ����������</summary>
        public Func<T, T, T> Operation { get; set; }

        /// <summary>������������� ������ ��������� ����������</summary>
        protected BinaryFunctionOperationEvulation() { }

        /// <summary>������������� ������ ��������� ����������</summary>
        /// <param name="Operation">����� ���������� ���������� ���������� �� ������ ����������� ���������� �������� ���������</param>
        protected BinaryFunctionOperationEvulation(Func<T, T, T> Operation) { this.Operation = Operation; }

        /// <summary>������������� ������ ��������� ����������</summary>
        /// <param name="Operation">����� ���������� ���������� ���������� �� ������ ����������� ���������� �������� ���������</param>
        /// <param name="A">������ ������� ����������</param>
        /// <param name="B">������ ������� ����������</param>
        protected BinaryFunctionOperationEvulation(Func<T, T, T> Operation, Evulation<T> A, Evulation<T> B)
            : this(Operation)
        {
            this.A = A;
            this.B = B;
        }

        /// <inheritdoc />
        public override T GetValue() => Operation(A.GetValue(), B.GetValue());

        /// <inheritdoc />
        public override Ex GetExpression() => Ex.Call(Operation.Target?.ToExpression(), Operation.Method, A.GetExpression(), B.GetExpression());
    }
}