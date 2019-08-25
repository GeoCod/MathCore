using System;
using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� ����������� ��������</summary>
    /// <typeparam name="T">��� ������������� ��������</typeparam>
    public class ValueEvulation<T> : Evulation<T>
    {
        /// <summary>������������ ��������</summary>
        public T Value { get; set; }

        /// <summary>������������� ������ ���������� ����������� ��������</summary>
        public ValueEvulation() { }

        /// <summary>������������� ������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        public ValueEvulation(T value) => Value = value;

        /// <inheritdoc />
        public override T GetValue() => Value;

        /// <inheritdoc />
        public override Ex GetExpression() => Value.ToExpression();

        /// <inheritdoc />
        public override string ToString() => Value.ToString();

        /// <summary>�������� �������� �������������� ���� �������� � ��� ���������� ����� ��������</summary>
        /// <param name="Value">������������� ��������</param>
        public static implicit operator ValueEvulation<T>(T Value) => new ValueEvulation<T>(Value);
    }

    /// <summary>����������� ���������� ����������� ��������</summary>
    /// <typeparam name="T">��� ������������� ��������</typeparam>
    public class NamedValueEvulation<T> : ValueEvulation<T>
    {
        /// <summary>��� ����������</summary>
        public string Name { get; set; }

        /// <summary>������� ����, ��� ������ ���������� �������� ����������� ����������</summary>
        public bool IsParameter { get; set; }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        public NamedValueEvulation() { }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        public NamedValueEvulation(T value) : base(value) { }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        /// <param name="name">��� ����������</param>
        public NamedValueEvulation(T value, string name) : base(value) => Name = name;

        /// <summary>���� ���������� �������� ����������, �� ������������ ��������� ���������, ����� ������������ ���������� ��������</summary>
        /// <returns>���������, ��������������� ������� ����������</returns>
        public override Ex GetExpression() => IsParameter
            ? Ex.Parameter(typeof(T), Name)
            : base.GetExpression();

        /// <inheritdoc />
        public override string ToString() => IsParameter
            ? Name.IsNullOrWhiteSpace()
                ? $"({typeof(T)})p"
                : Name
            : Name.IsNullOrWhiteSpace()
                ? Value.ToString()
                : $"{Name}={Value}";
    }
}