using Ex = System.Linq.Expressions.Expression;
// ReSharper disable UnusedMember.Global

namespace MathCore.Evulations
{
    /// <summary>���������� ����� ���� ����������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class AdditionEvulation<T> : BinaryFunctionOperatorEvulation<T>
    {
        /// <summary>������������� ������ ���������� ����� ���� ����������</summary>
        public AdditionEvulation() : base(Ex.Add) { }

        /// <summary>������������� ������ ���������� ����� ���� ����������</summary>
        /// <param name="a">���������� ������� ����������</param>
        /// <param name="b">���������� ������� ����������</param>
        public AdditionEvulation(Evulation<T> a, Evulation<T> b) : base(Ex.Add, a, b) { }
    }
}