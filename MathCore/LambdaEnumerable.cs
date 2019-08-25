using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathCore.Annotations;

namespace MathCore
{
    public class LambdaEnumerable<T> : Factory<IEnumerator<T>>, IEnumerable<T>
    {  
        /* ------------------------------------------------------------------------------------------ */

        public LambdaEnumerable([CanBeNull] Func<IEnumerable<T>> Generator) : base(() => (Generator?.Invoke() ?? Enumerable.Empty<T>()).GetEnumerator()) { }

        /* ------------------------------------------------------------------------------------------ */

        #region Implementation of IEnumerable

        /// <summary>���������� �������������, ����������� ������� ��������� � ���������.</summary>
        /// <returns>
        /// ��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator() => Create();

        /// <summary>���������� �������������, ������� ������������ ������� ��������� ���������.</summary>
        /// <returns>
        /// ������ <see cref="T:System.Collections.IEnumerator"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /* ------------------------------------------------------------------------------------------ */
    }
}