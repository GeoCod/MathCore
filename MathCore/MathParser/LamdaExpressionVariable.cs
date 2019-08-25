using System;

namespace MathCore.MathParser
{
    /// <summary>�����-����������</summary>
    /// <remarks>�������� ���������� - ��������� ���������� �����-�������</remarks>
    public class LamdaExpressionVariable : ExpressionVariabel
    {
        /// <summary>������� ���������� �������� ����������</summary>
        private readonly Func<double> _Value;

        /// <summary>������� ���������� ����������� �������������� ��������</summary>
        public override bool IsPrecomputable => false;

        /// <summary>������������� ������ ���������� �����-����������</summary>
        /// <param name="Source">�����-������� ��������� �������� ����������</param>
        public LamdaExpressionVariable(Func<double> Source) : this("", Source) { }

        /// <summary>������������� ������ ���������� �����-����������</summary>
        /// <param name="Name">��� ����������</param>
        /// <param name="Source">�����-������� ��������� �������� ����������</param>
        public LamdaExpressionVariable(string Name, Func<double> Source) : base(Name) => _Value = Source;

        /// <summary>�������� �������� ����������</summary>
        /// <returns>��������� �������� ����������</returns>
        public override double GetValue() => Value = _Value();

        /// <summary>����������� ����������</summary>
        /// <returns>����� ��������� �����-���������� � ��� �� ������ � ������ �������</returns>
        public override ExpressionVariabel Clone() => new LamdaExpressionVariable(Name, (Func<double>)_Value.Clone());
    }
}