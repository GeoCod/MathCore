using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
// ReSharper disable UnusedMember.Global

namespace MathCore
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _Comparer;
        private readonly Func<T, int> _HashFunction;

        public LambdaEqualityComparer(Func<T, T, bool> Comparer, Func<T, int> HashFunction = null)
        {
            Contract.Requires(Comparer != null);
            Contract.Ensures(_Comparer == Comparer);
            Contract.Ensures(_HashFunction != null);

            _Comparer = Comparer;
            _HashFunction = HashFunction ?? (o => o.GetHashCode());
        }

        /// <summary>����������, ����� �� ��� ��������� �������.</summary>
        /// <returns>�������� true, ���� ��������� ������� �����; � ��������� ������ � �������� false.</returns>
        /// <param name="x">������ ������������ ������ ���� <typeparam name="T"/>.</param>
        /// <param name="y">������ ������������ ������ ���� <typeparam name="T"/>.</param>
        public bool Equals(T x, T y) => _Comparer(x, y);

        /// <summary>���������� ���-��� ���������� �������.</summary>
        /// <returns>���-��� ���������� �������.</returns>
        /// <param name="obj">������ <see cref="T:System.Object"/>, ��� �������� ������ ���� ��������� ���-���.</param>
        /// <exception cref="T:System.ArgumentNullException">��� <paramref name="obj"/> �������� ��������� �����, ��������� <paramref name="obj"/> �������� null.</exception>
        public int GetHashCode(T obj) => _HashFunction(obj);
    }

    public static class LambdaEqualityComparer
    {
        public static LambdaEqualityComparer<T> Create<T>(this Func<T, T, bool> Comparer, Func<T, int> HashFunction = null) 
            => new LambdaEqualityComparer<T>(Comparer, HashFunction);
    }
}