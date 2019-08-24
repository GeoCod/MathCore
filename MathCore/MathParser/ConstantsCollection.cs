using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MathCore.Annotations;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable NotAccessedField.Local

namespace MathCore.MathParser
{
    /// <summary>��������� ��������</summary>
    [System.Diagnostics.DebuggerDisplay("����������� ��������������� �������� = {" + nameof(Count) + "}"), DST]
    public sealed class ConstantsCollection : IEnumerable<ExpressionVariabel>
    {
        /// <summary>������ �� ���������</summary>
        [NotNull]
        private readonly MathExpression _Expression;

        /// <summary>�������� ��������</summary>
        [NotNull]
        private readonly List<ExpressionVariabel> _Items = new List<ExpressionVariabel>();

        /// <summary>���������� ��������� ���������</summary>
        public int Count => _Items.Count;

        /// <summary>�������� �������� �� �����</summary>
        /// <param name="Name">��� ���������</param>
        /// <returns>��������� � ��������� ������</returns>
        [NotNull]
        public ExpressionVariabel this[[NotNull] string Name]
        {
            get
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(Name));
                Contract.Ensures(Contract.Result<ExpressionVariabel>() != null);
                if(Name == null) throw new ArgumentNullException(nameof(Name));
                if(string.IsNullOrEmpty(Name)) throw new ArgumentOutOfRangeException(nameof(Name));
                var c = _Items.Find(v => v.Name == Name);
                if(c == null) throw new ArgumentException($"��������� � ������ {Name} �� �������");
                return c;
            }
        }

        /// <summary>������������� ����� ��������� ��������</summary>
        /// <param name="Expression">�������������� ���������, �������� ����������� ���������</param>
        public ConstantsCollection([NotNull] MathExpression Expression)
        {
            Contract.Requires(Expression != null);
            _Expression = Expression;
        }

        /// <summary>�������� ������� � ���������</summary>
        /// <param name="Constant">����������� ��������, ��� ���������</param>
        public bool Add([NotNull] ExpressionVariabel Constant)
        {
            Contract.Requires(Constant != null);
            if(_Items.Contains(v => v.Name == Constant.Name)) return false;
            Constant.IsConstant = true;
            _Items.Add(Constant);
            return true;
        }

        /// <summary>�������� ����� �������� ���������</summary>
        /// <returns>������������ ��� �������� ���������</returns>
        [NotNull]
        public IEnumerable<string> GetNames()
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            return _Items.Select(v => v.Name);
        }

        /// <summary>�������� ������������ �������� ��������</summary>
        /// <returns>������������� ��������</returns>
        [NotNull]
        IEnumerator<ExpressionVariabel> IEnumerable<ExpressionVariabel>.GetEnumerator() => _Items.GetEnumerator();

        /// <summary>�������� ������������ �������� ��������</summary>
        /// <returns>������������� ��������</returns>
        [NotNull]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ExpressionVariabel>)this).GetEnumerator();
    }
}