using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� �������� ���� �������</summary>
    /// <typeparam name="TObject">��� �������, ���� �������� ���� ��������</typeparam>
    /// <typeparam name="TValue">��� �������� ����</typeparam>
    public class FieldValueEvulation<TObject, TValue> : UnaryOperatorEvulation<TObject, TValue>
    {
        /// <summary>������������� ������ ���������� �������� ���� �������</summary>
        /// <param name="PropertyName">��� ����</param>
        public FieldValueEvulation(string PropertyName) : base(e => Ex.Field(e, PropertyName)) { }

        /// <summary>������������� ������ ���������� ���� �������</summary>
        /// <param name="obj">���������� �������, ���� �������� ���� ��������</param>
        /// <param name="FieldName">��� ���� �������</param>
        public FieldValueEvulation(Evulation<TObject> obj, string FieldName) : base(e => Ex.Field(e, FieldName), obj) { }
    }
}