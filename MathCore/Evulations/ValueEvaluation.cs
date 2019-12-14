using System;
using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� ����������� ��������</summary>
    /// <typeparam name="T">��� ������������� ��������</typeparam>
    public class ValueEvaluation<T> : Evulation<T>
    {
        /// <summary>������������ ��������</summary>
        public T Value { get; set; }

        /// <summary>������������� ������ ���������� ����������� ��������</summary>
        public ValueEvaluation() { }

        /// <summary>������������� ������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        public ValueEvaluation(T value) => Value = value;

        /// <inheritdoc />
        public override T GetValue() => Value;

        /// <inheritdoc />
        public override Ex GetExpression() => Value.ToExpression();

        /// <inheritdoc />
        public override string ToString() => Value.ToString();

        /// <summary>�������� �������� �������������� ���� �������� � ��� ���������� ����� ��������</summary>
        /// <param name="Value">������������� ��������</param>
        public static implicit operator ValueEvaluation<T>(T Value) => new ValueEvaluation<T>(Value);
    }

    /// <summary>����������� ���������� ����������� ��������</summary>
    /// <typeparam name="T">��� ������������� ��������</typeparam>
    public class NamedValueEvaluation<T> : ValueEvaluation<T>
    {
        /// <summary>��� ����������</summary>
        public string Name { get; set; }

        /// <summary>������� ����, ��� ������ ���������� �������� ����������� ����������</summary>
        public bool IsParameter { get; set; }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        public NamedValueEvaluation() { }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        public NamedValueEvaluation(T value) : base(value) { }

        /// <summary>������������� ������ ������������ ���������� ����������� ��������</summary>
        /// <param name="value">������������ ��������</param>
        /// <param name="name">��� ����������</param>
        public NamedValueEvaluation(T value, string name) : base(value) => Name = name;

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