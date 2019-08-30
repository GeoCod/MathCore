using System.Diagnostics.Contracts;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>���������� ��� ����� ������� ��������</summary>
    public static class DecimalExtentions
    {
        /// <summary>���������� ����������� ����� ��������� �������� ����������������� �������������</summary>
        /// <param name="x">�����, ���������� ������ �������� ��������� ���������</param>
        /// <param name="epsilon">��������� ��������</param>
        /// <returns>���������� ������ �����</returns>
        [DST]
        public static decimal Sqrt(this decimal x, decimal epsilon = 0.0M)
        {
            var current = (decimal)Math.Sqrt((double)x);
            decimal previous;
            do
            {
                previous = current;
                if(previous == 0.0M) return 0;
                current = (previous + x / previous) / 2;
            }
            while(Math.Abs(previous - current) > epsilon);
            return current;
        }

        /// <summary>�������� �� ����� �����?</summary>
        /// <param name="x">����������� �����</param>
        /// <returns>������, ���� ����� �����</returns>
        [DST]
        public static bool IsInt(this decimal x) => decimal.Round(x) - x == 0;

        [DST]
        public static decimal Round(this decimal x, int n = 0) => decimal.Round(x, n);

        /// <summary>�������� �������� �����</summary>
        /// <param name="x">������������� �����</param>
        /// <returns>�����, �������� � ��������</returns>
        [DST]
        public static decimal GetInverse(this decimal x) => 1 / x;

        //[Diagnostics.DST, Pure]
        //public static double GetAbsMod(this decimal x, decimal mod) { return x % mod + (x < 0 ? mod : 0); }

        [DST, Pure]
        public static decimal GetAbs(this decimal x) => x < 0 ? -x : x;

        //[Diagnostics.DST]
        //public static double GetPower(this decimal x, int n)
        //{
        //    if(n > 1000 || n < -1000) return Math.Pow(x, n);
        //    if(n < 0) return (1 / x).GetPower(-n);
        //    var result = 1.0;
        //    for(var i = 0; i < n; i++)
        //        result *= x;
        //    return result;
        //}

        [DST]
        public static decimal Power(this decimal x, decimal y) => (decimal)Math.Pow((double)x, (double)y);

        //[Diagnostics.DST]
        //public static Complex GetPower(this decimal x, Complex z) { return x ^ z; }

        [DST]
        public static decimal In_dB(this decimal x) => 20 * (decimal)Math.Log10((double)x);
        //[Diagnostics.DST]
        //public static double In_dB_byPower(this decimal x) { return 10 * Math.Log10(x); }

        [DST]
        public static double From_dB(this decimal db) => Math.Pow(10, (double)(db / 20));
        [DST]
        public static double From_dB_byPower(this decimal db) => Math.Pow(10, (double)(db / 10));

        //[Diagnostics.DST]
        //public static double ToRad(this decimal deg) { return deg * Consts.Geometry.ToRad; }
        //[Diagnostics.DST]
        //public static double ToDeg(this decimal rad) { return rad * Consts.Geometry.ToDeg; }
    }
}