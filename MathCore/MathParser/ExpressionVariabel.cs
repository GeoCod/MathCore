using System;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using MathCore.Annotations;

namespace MathCore.MathParser
{
    /// <summary>���������� ��������������� ���������</summary>
    public class ExpressionVariabel : ExpressionItem, ICloneable<ExpressionVariabel>
    {
        /// <summary>�������� ����������</summary>
        private double _Value;
        /// <summary>�������� �� ����������?</summary>
        private bool _IsConstant;

        /// <summary>�������� ����������</summary>
        public virtual double Value { get => _Value; set => Set(ref _Value, value); }

        /// <summary>������� ����������� �������������� ��������</summary>
        public virtual bool IsPrecomputable => true;

        /// <summary>�������� �� ����������?</summary>
        public bool IsConstant { get => _IsConstant; set => Set(ref _IsConstant, value); }

        /// <summary>����� ���������� ��������</summary>
        /// <returns>��������� �������� ����������</returns>
        public override double GetValue() => Value;

        /// <summary>������������� ������ ���������� ����������</summary>
        /// <param name="Name">��� ����������</param>
        public ExpressionVariabel(string Name) : base(Name) { }

        /// <summary>������������ ����������</summary>
        /// <returns>����� ��������� ���������� � ��� �� ������� � ��� �� ���������</returns>
        public virtual ExpressionVariabel Clone() => new ExpressionVariabel(Name) { Value = Value };

        /// <summary>�������������� � ������</summary>
        /// <returns>��������� ������������� ����������</returns>
        public override string ToString() => $"{Name}={_Value}";

        object ICloneable.Clone() => Clone();

        /// <summary>�������� �������� ���������� ������������� ����� � ���� ����������</summary>
        /// <param name="x">������������ �����</param>
        /// <returns>���������� ����������, �������� ��������� �����</returns>
        public static implicit operator ExpressionVariabel(double x) =>
            new ExpressionVariabel("") { _Value = x };

        /// <summary>�������� �������� ���������� � ���� ������������� �����</summary>
        /// <param name="variable">���������� ����������</param>
        /// <returns>�������� ����������</returns>
        public static implicit operator double(ExpressionVariabel variable) =>
            variable.GetValue();
    }
}