using System;
using MathCore.Annotations;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable UnusedMember.Global

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
        [DST]
        public UsingDisposableObject(T Obj) : base(Obj, o => o.Dispose()) { }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>�������� �������� �� �������</summary>
        /// <typeparam name="TValue">��� ��������, ����������� �� �������</typeparam>
        /// <param name="f">����� ��������� ��������</param>
        /// <returns>��������, ���������� �� ������� ��������� �������</returns>
        [DST]
        public TValue GetValue<TValue>([NotNull] Func<T, TValue> f) => f(Object);

        /* ------------------------------------------------------------------------------------------ */
    }
}