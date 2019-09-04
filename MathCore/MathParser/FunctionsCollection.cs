using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathCore.Annotations;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace MathCore.MathParser
{
    /// <summary>��������� �������</summary>
    [DebuggerDisplay("���������� ������������������ ������� = {" + nameof(Count) + "}")]
    public class FunctionsCollection : IEnumerable<ExpressionFunction>
    {
        /// <summary>������ �� �������������� ���������</summary>
        [NotNull]
        private readonly MathExpression _MathExpression;

        /// <summary>������ ������� ��������������� ���������</summary>
        [NotNull]
        private readonly List<ExpressionFunction> _Functions = new List<ExpressionFunction>();

        /// <summary>����� �������</summary>
        public IEnumerable<string> Names => _Functions.Select(v => v.Name);

        /// <summary>���������� ������������ �������</summary>
        public int Count => _Functions.Count;

        /// <summary>���������� ������� �� ����� � ������ ����������</summary>
        /// <param name="Name">��� �������</param>
        /// <param name="ArgumentsCount">���������� ����������</param>
        /// <returns>�������, ���������������� ������� ���������</returns>
        [NotNull]
        public ExpressionFunction this[[NotNull] string Name, int ArgumentsCount]
        {
            get
            {
                var function = _Functions.FirstOrDefault(f => f.IsEqualSignature(Name, ArgumentsCount));
                if(function != null) return function;
                function = new ExpressionFunction(Name, new string[ArgumentsCount]);
                _Functions.Add(function);
                return function;
            }
        }

        /// <summary>���������� ������� �� ����� � ������ ����������</summary>
        /// <param name="Name">��� �������</param>
        /// <param name="Arguments">������ ��� ����������</param>
        /// <returns>�������, ���������������� ������� ���������</returns>
        [NotNull]
        public ExpressionFunction this[[NotNull] string Name, [NotNull] params string[] Arguments]
        {
            get
            {
                var function = _Functions.FirstOrDefault(f => f.IsEqualSignature(Name, Arguments));
                if(function != null) return function;
                function = new ExpressionFunction(Name, Arguments);
                _Functions.Add(function);
                return function;
            }
        }

        /// <summary>������������� ����� ��������� ������� ��������������� ���������</summary>
        /// <param name="MathExpression">�������������� ���������, �� ������� ��������� ����������� ���������</param>
        public FunctionsCollection([NotNull] MathExpression MathExpression) => _MathExpression = MathExpression;


        /// <summary>�������� ������� � ���������</summary>
        /// <param name="function">�������</param>
        /// <returns>������, ���� ������� ���� ���������</returns>
        public bool Add([NotNull] ExpressionFunction function)
        {
            var F = _Functions.FirstOrDefault(f => f.IsEqualSignature(function.Name, function.Arguments));
            if(F != null) return false;
            _Functions.Add(function);
            return true;
        }

        /// <summary>���������� �������������, ����������� ������� ��������� � ���������</summary>
        /// <returns>
        /// ��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        IEnumerator<ExpressionFunction> IEnumerable<ExpressionFunction>.GetEnumerator() => _Functions.GetEnumerator();

        /// <summary>���������� �������������, ������� ������������ ������� ��������� ���������</summary>
        /// <returns>
        /// ������ <see cref="T:System.Collections.IEnumerator"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Functions).GetEnumerator();
    }
}