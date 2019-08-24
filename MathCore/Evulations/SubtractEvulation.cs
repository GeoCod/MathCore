using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� �������� ���� ����������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class SubtractEvulation<T> : BinaryFunctionOperatorEvulation<T>
    {
        /// <summary>������������� ������ ���������� ��������</summary>
        public SubtractEvulation() : base(Ex.Subtract) { }

        /// <summary>������������� ������ ���������� ��������</summary>
        /// <param name="a">���������� ������� �����������</param>
        /// <param name="b">���������� ������� �����������</param>
        public SubtractEvulation(Evulation<T> a, Evulation<T> b) : base(Ex.Subtract, a, b) { }
    }
}