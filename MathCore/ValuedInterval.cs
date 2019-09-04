using System;
using System.Collections.Generic;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Linq;
using MathCore.Annotations;
// ReSharper disable UnusedMember.Global

namespace MathCore
{
    /// <summary>�������� ����������� �������� ������� ��������</summary>
    [Serializable]
    public struct ValuedInterval<TValue> : IComparable<double>, IFormattable
    {
        /* -------------------------------------------------------------------------------------------- */

        /// <summary>����� ��������� ���� ����������</summary>
        /// <param name="a">������ ������������ ��������</param>
        /// <param name="b">������ ������������ ��������</param>
        /// <returns>1 - ���� ������ �������� ������ �������, -1 - ���� ������ �������� ������ �������, 0 - ���� ��������� �����</returns>
        public static int Comparer_Length(ValuedInterval<TValue> a, ValuedInterval<TValue> b)
        {
            var l1 = a.Length;
            var l2 = b.Length;
            return l1 > l2 ? 1 : (l1 < l2 ? -1 : 0);
        }

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>�������� �� ������ ������� ���������?</summary>
        private readonly bool _MinInclude;
        /// <summary>�������� �� ������� ������� ���������?</summary>
        private readonly bool _MaxInclude;
        /// <summary>������ ������� ���������</summary>
        private readonly double _Min;
        /// <summary>������� ������� ���������</summary>
        private readonly double _Max;

        private readonly TValue _Value;

        /* -------------------------------------------------------------------------------------------- */

        #region ��������

        /// <summary>�������� �� ������ ������� ���������?</summary>
        public bool MinInclude => _MinInclude;

        /// <summary>�������� �� ������� ������� ���������?</summary>
        public bool MaxInclude => _MaxInclude;

        /// <summary>������ ������� ���������</summary>
        public double Min => _Min;

        /// <summary>������� ������� ���������</summary>
        public double Max => _Max;

        /// <summary>����� ���������</summary>
        public double Length => _Max - _Min;

        /// <summary>�������� ���������</summary>
        public double Middle => (_Min + _Max) / 2;

        public TValue Value => _Value;

        #endregion

        /* -------------------------------------------------------------------------------------------- */

        #region ������������

        /// <summary>��������</summary>
        /// <param name="Min">������ ������� ���������</param>
        /// <param name="Max">������� ������� ���������</param>
        /// <param name="Value">��������</param>
        public ValuedInterval(double Min, double Max, TValue Value) : this(Min, true, Max, true, Value) { }

        /// <summary>��������</summary>
        /// <param name="Min">������ ������� ���������</param>
        /// <param name="Max">������� ������� ���������</param>
        /// <param name="IncludeLimits">�������� �������?</param>
        /// <param name="Value">��������</param>
        public ValuedInterval(double Min, double Max, bool IncludeLimits, TValue Value) : this(Min, IncludeLimits, Max, IncludeLimits, Value) { }

        /// <summary>��������</summary>
        /// <param name="Min">������ ������� ���������</param>
        /// <param name="MinInclude">�������� �� ������ ������� ���������?</param>
        /// <param name="Max">������� ������� ���������</param>
        /// <param name="MaxInclude">�������� �� ������� ������� ���������</param>
        /// <param name="Value">��������</param>
        public ValuedInterval(double Min, bool MinInclude, double Max, bool MaxInclude, TValue Value)
        {
            _Min = Min;
            _Max = Max;
            _MinInclude = MinInclude;
            _MaxInclude = MaxInclude;
            _Value = Value;
        }

        #endregion

        /* -------------------------------------------------------------------------------------------- */

        #region ������������ �������

        public ValuedInterval<TValue> IncludeMax(bool Include) => new ValuedInterval<TValue>(_Min, _MinInclude, _Max, Include, _Value);
        public ValuedInterval<TValue> IncludeMin(bool Include) => new ValuedInterval<TValue>(_Min, Include, _Max, _MaxInclude, _Value);
        public ValuedInterval<TValue> Include(bool IncludeMin, bool IncludeMax) => new ValuedInterval<TValue>(_Min, IncludeMin, _Max, IncludeMax, _Value);
        public ValuedInterval<TValue> Include(bool Include) => new ValuedInterval<TValue>(_Min, Include, _Max, Include, _Value);

        public ValuedInterval<TValue> SetMin(double value) => new ValuedInterval<TValue>(value, _MinInclude, _Max, _MaxInclude, _Value);
        public ValuedInterval<TValue> SetMin(double value, bool IncludeMin) => new ValuedInterval<TValue>(value, IncludeMin, _Max, _MaxInclude, _Value);
        public ValuedInterval<TValue> SetMax(double value) => new ValuedInterval<TValue>(_Min, _MinInclude, value, _MaxInclude, _Value);
        public ValuedInterval<TValue> SetMax(double value, bool IncludeMax) => new ValuedInterval<TValue>(_Min, _MinInclude, value, IncludeMax, _Value);

        public ValuedInterval<TValue> SetValue(TValue value) => new ValuedInterval<TValue>(_Min, _MinInclude, _Max, _MaxInclude, value);

        public void Deconstruct(out double min, out double max)
        {
            min = _Min;
            max = _Max;
        }

        public void Deconstruct(out double min, out double max, out TValue value)
        {
            min = _Min;
            max = _Max;
            value = _Value;
        }

        ///// <summary>�������� �� ��������� � ��������</summary>
        ///// <param name="X">����������� ��������</param>
        ///// <returns></returns>
        //public bool Check(double X)
        //{
        //    return (MinInclude && Math.Abs(X - Min) < double.Epsilon)
        //        || (MaxInclude && Math.Abs(X - Max) < double.Epsilon)
        //        || (X > Min && X < Max);
        //}

        public bool Check(double X, double MinOffset, double MaxOffset)
        {
            var min = _Min + MinOffset;
            var max = _Max + MaxOffset;

            return (_MinInclude && Math.Abs(X - min) < double.Epsilon)
                || (_MaxInclude && Math.Abs(X - max) < double.Epsilon)
                || (X > min && X < max);
        }

        /// <summary>�������� �� ��������� �������� � ��������</summary>
        /// <param name="value">����������� ��������</param>
        /// <returns>������, ���� �������� ������ � ��������</returns>
        [DST]
        public bool Check(double value) =>
            (_MinInclude && _Min.CompareTo(value) == 0)
            || (_MaxInclude && _Max.CompareTo(value) == 0)
            || (value.CompareTo(_Min) > 0 && value.CompareTo(_Max) < 0);

        public bool Check(double X, double Offset) => Check(X, Offset, -Offset);

        public bool IsExclude(ValuedInterval<TValue> I) => !IsInclude(I);

        public bool IsInclude(ValuedInterval<TValue> I) =>
            Check(I._MinInclude ? I._Min : I._Min + double.Epsilon)
            && Check(I._MaxInclude ? I._Max : I._Max - double.Epsilon);

        public bool IsIntersect(ValuedInterval<TValue> I)
        {
            if (Math.Abs(I.Min - Min) < double.Epsilon && Math.Abs(I._Max - _Max) < double.Epsilon) return true;

            var min_include = Check(I._Min) || Math.Abs(I._Min - _Max) > double.Epsilon;
            var max_include = Check(I._Max) || Math.Abs(I._Max - _Min) > double.Epsilon;

            return min_include || max_include;
        }

        #endregion

        public int CompareTo(double x) =>
            (x > _Min && x < _Max)
            || (_MinInclude && Math.Abs(_Min - x) < double.Epsilon)
            || (_MaxInclude && Math.Abs(_Max - x) < double.Epsilon)
                ? 0
                : (x < _Min ? -1 : 1);

        #region �����

        public void For(int samples, [NotNull] Action<double> Do)
        {
            var len = Length;
            var min = _Min;
            if (!_MaxInclude) len -= double.Epsilon;
            if (!_MinInclude)
            {
                len -= double.Epsilon;
                min += double.Epsilon;
            }
            var dx = len / (samples - 1);
            for (var i = 0; i < samples; i++)
                Do(min + i * dx);
        }

        public void For(int samples, [NotNull] Action<int, double> Do)
        {
            var len = Length;
            var min = _Min;
            if (!_MaxInclude) len -= double.Epsilon;
            if (!_MinInclude)
            {
                len -= double.Epsilon;
                min += double.Epsilon;
            }
            var dx = len / (samples - 1);
            for (var i = 0; i < samples; i++)
                Do(i, min + i * dx);
        }

        public void WhileInInterval(double step, [NotNull] Action<double> Do)
        {
            var min = Math.Min(_Max, _Min);
            step = _Max < _Min && step > 0 ? -step : step;
            var x = min + (_MinInclude ? 0 : double.Epsilon);
            while (Check(x)) Do(x += step);
        }

        #endregion

        //public double[] GetValues(int Count)
        //{
        //    var result = new double[Count];
        //    For(Count, (i, x) => result[i] = x);
        //    return result;
        //}

        public IEnumerable<double> GetValues(int Count)
        {
            var len = Length;
            var min = _Min;
            if (!_MaxInclude) len -= double.Epsilon;
            if (!_MinInclude)
            {
                len -= double.Epsilon;
                min += double.Epsilon;
            }
            var dx = len / (Count - 1);
            for (var i = 0; i < Count; i++)
                yield return min + i * dx;
        }

        public ValuedInterval<TValue> GetInvertedInterval() => new ValuedInterval<TValue>(_Max, _MaxInclude, _Min, _MinInclude, _Value);

        public IEnumerable<double> GetValues(double Step)
        {
            var position = _Min;
            do
            {
                yield return position;
                position += Step;
            } while ((_MaxInclude && position <= _Max) || (!_MaxInclude && position < _Max));
        }

        [NotNull]
        public IEnumerable<ValuedInterval<TValue>> GetSubIntervals(int Count)
        {
            var last = _MinInclude ? _Min : _Min - double.Epsilon;
            var value = _Value;
            return GetValues(Count)
                .Skip(1)
                .ForeachLazyLast(v => last = v)
                .Select(v => new ValuedInterval<TValue>(last, v, true, value));
        }

        public string ToString(string Format) => string.Format(
            "{0}{2};{3}{1}",
            _MinInclude ? "[" : "(",
            _MaxInclude ? "]" : ")",
            _Min.ToString(Format),
            _Max.ToString(Format));

        /// <summary>����������� �������� �������� ���������� � �������������� ��������� �������.</summary>
        /// <returns>������ <see cref="T:System.String"/> �������� �������� �������� ���������� � �������� �������.</returns>
        /// <param name="Format">
        /// ������ <see cref="T:System.String"/>, �������� ������������ ������.� ��� � 
        /// �������� null ��� ������������� ������� �� ���������, ������������� ��� ���� ���������� 
        /// <see cref="T:System.IFormattable"/>. 
        /// </param>
        /// <param name="FormatProvider">
        /// ������ <see cref="T:System.IFormatProvider"/>, ������������ ��� �������������� ��������.� ��� � 
        /// �������� null ��� ��������� �������� � �������������� ����� �� ������ �������� �������� ��������� ��������� 
        /// ��������� ������������ �������. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public string ToString(string Format, IFormatProvider FormatProvider) => string.Format(
            "{0}{2};{3}{1}",
            _MinInclude ? "[" : "(",
            _MaxInclude ? "]" : ")",
            _Min.ToString(Format, FormatProvider),
            _Max.ToString(Format, FormatProvider));

        /* ------------------------------------------------------------------------------------------ */

        public static implicit operator double(ValuedInterval<TValue> I) => I.Length;

        public static explicit operator ValuedInterval<TValue>(double V) => new ValuedInterval<TValue>(0, true, V, true, default);

        public static ValuedInterval<TValue> operator +(ValuedInterval<TValue> I, double x) => new ValuedInterval<TValue>(I._Min + x, I._MinInclude, I._Max + x, I._MaxInclude, I._Value);

        public static ValuedInterval<TValue> operator -(ValuedInterval<TValue> I, double x) => new ValuedInterval<TValue>(I._Min - x, I._MinInclude, I._Max - x, I._MaxInclude, I._Value);

        public static ValuedInterval<TValue> operator *(ValuedInterval<TValue> I, double x) => new ValuedInterval<TValue>(I._Min * x, I._MinInclude, I._Max * x, I._MaxInclude, I._Value);

        public static ValuedInterval<TValue> operator /(ValuedInterval<TValue> I, double x) => new ValuedInterval<TValue>(I._Min / x, I._MinInclude, I._Max / x, I._MaxInclude, I._Value);

        /* ------------------------------------------------------------------------------------------ */
    }
}