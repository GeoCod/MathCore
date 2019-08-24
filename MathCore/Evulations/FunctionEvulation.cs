using System;
using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� ������� ��� ����������</summary>
    /// <typeparam name="T"></typeparam>
    public class FunctionEvulation<T> : Evulation<T>
    {
        /// <summary>����������� �������</summary>
        public Func<T> Function { get; set; }

        /// <summary>������������� ������ ���������� �������</summary>
        public FunctionEvulation() { }

        /// <summary>������������� ������ ���������� �������</summary>
        /// <param name="Function">����������� �������</param>
        public FunctionEvulation(Func<T> Function) { this.Function = Function; }

        /// <inheritdoc />
        public override T GetValue() => Function();

        /// <inheritdoc />
        public override Ex GetExpression() => Ex.Call(
            Function.Target == null ? null : Ex.Constant(Function.Target),
            Function.Method);

        /// <inheritdoc />
        public override string ToString() => "f()";

        /// <summary>�������� �������� �������������� ���� ������� � ���� ���������� �������</summary>
        /// <param name="Function">������������� �������</param>
        public static implicit operator FunctionEvulation<T>(Func<T> Function) => new FunctionEvulation<T>(Function);
    }

    /// <summary>����������� ���������� �������</summary>
    /// <typeparam name="T">��� �������� �������</typeparam>
    public class NamedFunctionEvulation<T> : FunctionEvulation<T>
    {
        /// <summary>��� �������</summary>
        public string Name { get; set; }

        /// <summary>������������� ������ ���������� �������� �������</summary>
        public NamedFunctionEvulation() { }

        /// <summary>������������� ������ ���������� �������� �������</summary>
        /// <param name="Function">����������� �������</param>
        public NamedFunctionEvulation(Func<T> Function) : base(Function) { }

        /// <summary>������������� ������ ���������� �������� �������</summary>
        /// <param name="Function">����������� �������</param>
        /// <param name="Name">��� �������</param>
        public NamedFunctionEvulation(Func<T> Function, string Name) : base(Function) { this.Name = Name; }

        /// <inheritdoc />
        public override string ToString() => $"{(Name.IsNullOrWhiteSpace() ? "_" : Name)}()";
    }
}