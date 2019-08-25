using System;
using Ex = System.Linq.Expressions.Expression;

namespace MathCore.Evulations
{
    /// <summary>���������� �������������� �����</summary>
    /// <typeparam name="TInput">��� �������� ��������</typeparam>
    /// <typeparam name="TOutput">��� ��������� ��������</typeparam>
    public class ConvertEvulation<TInput, TOutput> : Evulation<TOutput>
    {
        /// <summary>���������� �������� ��������</summary>
        public Evulation<TInput> InputEvulation { get; set; }

        /// <summary>�������-��������������� ����� �������� � �������� ��������</summary>
        public Func<TInput, TOutput> Converter { get; set; }

        /// <summary>������������� ������ ���������� �������������� �����</summary>
        public ConvertEvulation() { }

        /// <summary>������������� ������ ���������� �������������� �����</summary>
        /// <param name="Converter">����� �������������� �������� �������� � ��������</param>
        public ConvertEvulation(Func<TInput, TOutput> Converter) => this.Converter = Converter;

        /// <summary>������������� ������ ���������� �������������� �����</summary>
        /// <param name="InputEvulation">���������� �������� ��������</param>
        /// <param name="Converter">����� �������������� �������� �������� � ��������</param>
        public ConvertEvulation(Evulation<TInput> InputEvulation, Func<TInput, TOutput> Converter) : this(Converter) => this.InputEvulation = InputEvulation;

        /// <inheritdoc />
        public override TOutput GetValue() => Converter(InputEvulation.GetValue());

        /// <inheritdoc />
        public override Ex GetExpression() => Ex.Call(Converter.Target?.ToExpression(), Converter.Method, InputEvulation.GetExpression());
    }
}