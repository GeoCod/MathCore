using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MathCore.Values
{
    /// <summary>����������� ��������� ���������</summary>
    /// <typeparam name="T">��� ��������� ���������</typeparam>
    public abstract class AbstractSetOf<T> : IEnumerable<T>
    {
        /// <summary>�������� ���������</summary>
        public abstract int Power { get; }

        /// <summary>������� ��������� �������� � ���������</summary>
        /// <param name="value">����������� �������</param>
        /// <returns>������, ���� ������� ����������� ���������</returns>
        public virtual bool Contains(T value) => ((IEnumerable<T>)this).Contains(value);

        /// <summary>������� ����, ��� ������� �� ������ � ���������</summary>
        /// <param name="value">����������� �������</param>
        /// <returns>������, ���� ������� �� ����������� ���������</returns>
        public virtual bool NotContains(T value) => !Contains(value);

        /// <summary>�������� ������������� ���������</summary>
        /// <returns>������������� ���������</returns>
        public abstract IEnumerator<T> GetEnumerator();

        /// <summary>�������� ������������� ���������</summary>
        /// <returns>������������� ���������</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}