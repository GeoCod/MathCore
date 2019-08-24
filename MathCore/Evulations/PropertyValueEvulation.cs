using Ex = System.Linq.Expressions.Expression;
// ReSharper disable UnusedMember.Global

namespace MathCore.Evulations
{
    /// <summary>���������� �������� �������� �������</summary>
    /// <typeparam name="TObject">��� �������, �������� �������� ���� ��������</typeparam>
    /// <typeparam name="TValue">��� �������� ��������</typeparam>
    public class PropertyValueEvulation<TObject, TValue> : UnaryOperatorEvulation<TObject, TValue>
    {
        /// <summary>������������� ������ ���������� �������� �������� �������</summary>
        /// <param name="PropertyName">��� ��������</param>
        public PropertyValueEvulation(string PropertyName) : base(e => Ex.Property(e, PropertyName)) { }

        /// <summary>������������� ������ ���������� �������� �������</summary>
        /// <param name="obj">���������� �������, �������� �������� ���� ��������</param>
        /// <param name="PropertyName">��� �������� �������</param>
        public PropertyValueEvulation(Evulation<TObject> obj, string PropertyName) : base(e => Ex.Property(e, PropertyName), obj) { }
    }
}