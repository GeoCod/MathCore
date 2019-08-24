using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� ������������ ���� ����������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class MultiplyEvulation<T> : BinaryFunctionOperatorEvulation<T>
    {
        /// <summary>������������� ������ ���������� ������������</summary>
        public MultiplyEvulation() : base(Ex.Multiply) { }

        /// <summary>������������� ������ ���������� ������������</summary>
        /// <param name="a">���������� ������� �����������</param>
        /// <param name="b">���������� ������� �����������</param>
        public MultiplyEvulation(Evulation<T> a, Evulation<T> b) : base(Ex.Multiply, a, b) { }
    }
}