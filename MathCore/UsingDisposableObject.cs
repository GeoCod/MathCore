using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace MathCore
{
    /// <summary>�����-������ ��� inline-������� � ��������� ���������� �������, ������������ ��������� IDisposasble</summary>
    /// <typeparam name="T">��� ������������� �������, ������������ ��������� IDIsposable</typeparam>
    public class UsingDisposableObject<T> : UsingObject<T> where T : IDisposable
    {
        /* ------------------------------------------------------------------------------------------ */

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>����� ������ ��� ������������� �������</summary>
        /// <param name="Obj">������������ ������</param>
        [DebuggerStepThrough]
        public UsingDisposableObject(T Obj) : base(Obj, o => o.Dispose()) { }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>�������� �������� �� �������</summary>
        /// <typeparam name="TValue">��� ��������, ����������� �� �������</typeparam>
        /// <param name="f">����� ��������� ��������</param>
        /// <returns>��������, ���������� �� ������� ��������� �������</returns>
        [DebuggerStepThrough]
        public TValue GetValue<TValue>(Func<T, TValue> f) { Contract.Requires(f != null); return f(Object); }

        /* ------------------------------------------------------------------------------------------ */
    }
}