// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>�������� ������ ���� ������, ���� �����</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class LessOrEqualAttribute : Attribute
    {
        /// <summary>��������� ��������</summary>
        private object _Value;

        /// <summary>��������� ��������</summary>
        public object Value
        {
            get => _Value;
            set
            {
                if(!(value is IComparable)) throw new ArgumentException("�������� ������ ������������ ��������� IComparable", nameof(value));
                _Value = value;
            }
        }

        /// <summary>������������� ������ ���������� <see cref="LessOrEqualAttribute"/></summary>
        public LessOrEqualAttribute() { }

        /// <summary>������������� ������ ���������� <see cref="LessOrEqualAttribute"/></summary>
        /// <param name="Value">��������� ��������</param>
        public LessOrEqualAttribute(object Value)
        {
            if(!(Value is IComparable)) throw new ArgumentException("�������� ������ ������������ ��������� IComparable", nameof(Value));
            this.Value = Value;
        }

        /// <inheritdoc />
        public override string ToString() => $"value <= {Value}";
    }
}