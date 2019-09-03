using System.Diagnostics.Contracts;
using MathCore.Annotations;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>����������� ����� ������ � ��������� ����������� � �����</summary>
    public static class StringExtentionsTrim
    {
        /// <summary>����������� ����� ������ � ��������� ����������� � �����</summary>
        /// <param name="Str">���������� ������</param>
        /// <param name="Length">��������� �����</param>
        /// <param name="ReplacementPattern">������ ������</param>
        /// <returns>������ � �������� ���������� ������</returns>
        [NotNull]
        public static string TrimByLength([NotNull] this string Str, int Length, [NotNull] string ReplacementPattern = "..")
        {
            if (Str is null) throw new ArgumentNullException(nameof(Str));
            Contract.Requires(Length >= 0);
            Contract.Ensures((Str is null && Contract.Result<string>() is null)
                || (Str != null && Contract.Result<string>() != null));

            if(Str.Length <= Length) return Str;
            if(Length == 0) return "";

            var dL1 = Str.Length - Length + ReplacementPattern.Length;
            var dL2 = dL1 / 2;
            dL1 -= dL2;

            var s1 = Str.Substring(0, Str.Length / 2 - dL1);
            var start = Str.Length/2 + dL2;
            var len = Str.Length - Str.Length / 2 - dL2;
            var s2 = Str.Substring(start, len);

            return string.Format("{0}{2}{1}", s1, s2, ReplacementPattern);
        }
    }
}