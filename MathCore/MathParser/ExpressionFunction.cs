using System;
using System.Linq;

namespace MathCore.MathParser
{
    /// <summary>������� � ��������� ��������������� ���������</summary>
    public class ExpressionFunction : ExpressionItem, ICloneable<ExpressionFunction>
    {
        private Delegate _Delegate;

        /// <summary>������� �������</summary>
        public Delegate Delegate { get => _Delegate; set => Set(ref _Delegate, value); }

        /// <summary>������ ��� ����������</summary>
        public string[] Arguments { get; }

        /// <summary>������������� ����� ������� ��������� ��������������� ��������� �� ���������</summary>
        /// <param name="Name">��� �������</param>
        /// <param name="Arguments">������ ��� ����������</param>
        public ExpressionFunction(string Name, string[] Arguments) : base(Name) => this.Arguments = Arguments;

        /// <summary>����� ��������� �������� ������� �� ������� �������� � ����������</summary>
        /// <param name="arguments">������ ���������� �������</param>
        /// <returns>�������� �������</returns>
        public double GetValue(double[] arguments) => (double)Delegate.DynamicInvoke(arguments.Cast<object>().ToArray());

        /// <summary>�������� �� ��������������� ���������</summary>
        /// <param name="sName">��� �������</param>
        /// <param name="ArgumentsCount">���������� ����������</param>
        /// <returns>������, ���� ��������� ������������� �������</returns>
        public bool IsEqualSignature(string sName, int ArgumentsCount) => Name == sName && Arguments.Length == ArgumentsCount;

        /// <summary>�������� �� ��������������� ���������</summary>
        /// <param name="sName">��� �������</param>
        /// <param name="Arguments">������ ��� ����������</param>
        /// <returns>������, ���� ��������� ������������� �������</returns>
        public bool IsEqualSignature(string sName, string[] Arguments)
        {
            if(!string.Equals(Name, sName, StringComparison.CurrentCulture)) return false;
            var args = this.Arguments;
            if(args.Length != Arguments.Length) return false;
            for(int i = 0, N = args.Length; i < N; i++)
            {
                var arg_null = ReferenceEquals(args[i], null);
                var Arg_null = ReferenceEquals(Arguments[i], null);
                if(arg_null != Arg_null) return false;
                if(!arg_null && args[i] != Arguments[i]) return false;
                if(!Arg_null && Arguments[i] != args[i]) return false;
            }

            return true;
        }

        /// <summary>����� ��������� �������� �������. � ����� ���� �� ��������������.</summary>
        /// <returns>�������� �������</returns>
        public override double GetValue() => throw new NotSupportedException();

        /// <summary>������������ �������</summary>
        /// <returns>���� �������</returns>
        public ExpressionFunction Clone() => new ExpressionFunction(Name, Arguments) { Delegate = (Delegate)Delegate.Clone() };

        object ICloneable.Clone() => Clone();
    }
}