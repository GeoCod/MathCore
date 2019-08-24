using Ex = System.Linq.Expressions.Expression;
// ReSharper disable UnusedMember.Global

namespace MathCore.Evulations
{
    /// <summary>���������� ������� ���� ����������</summary>
    /// <typeparam name="T">��� �������� ����������</typeparam>
    public class DivideEvulation<T> : BinaryFunctionOperatorEvulation<T>
    {
        /// <summary>������������� ������ ���������� �������</summary>
        public DivideEvulation() : base(Ex.Divide) { }

        /// <summary>������������� ������ ���������� �������</summary>
        /// <param name="a">���������� ��������</param>
        /// <param name="b">���������� ��������</param>
        public DivideEvulation(Evulation<T> a, Evulation<T> b) : base(Ex.Divide, a, b) { }
    }
}