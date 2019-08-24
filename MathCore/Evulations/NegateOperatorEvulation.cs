using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� ��������� ��������</summary>
    /// <typeparam name="T">��� �������� ���������</typeparam>
    public class NegateOperatorEvulation<T> : UnaryOperatorEvulation<T, T>
    {
        /// <summary>������������� ������ ���������� �����������</summary>
        public NegateOperatorEvulation() : base(Ex.Negate) { }

        /// <summary>������������� ������ ���������� ���������</summary>
        /// <param name="value">���������� �������� ��������</param>
        public NegateOperatorEvulation(Evulation<T> value) : base(Ex.Negate, value) { }
    }
}