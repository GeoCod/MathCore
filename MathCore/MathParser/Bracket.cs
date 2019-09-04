using System;
using System.Diagnostics;
using MathCore.Annotations;
// ReSharper disable UnusedMember.Global

namespace MathCore.MathParser
{
    /// <summary>������</summary>
    public class Bracket : IEquatable<Bracket>, ICloneable<Bracket>
    {
        /// <summary>������� ������</summary>
        [NotNull]
        public static Bracket NewRound => new Bracket("(", ")");

        /// <summary>���������� ������</summary>
        [NotNull]
        public static Bracket NewRect => new Bracket("[", "]");

        /// <summary>�������� ������</summary>
        [NotNull]
        public static Bracket NewFigur => new Bracket("{", "}");

        /// <summary>����������� ������</summary>
        [NotNull]
        public string Start { get; }

        /// <summary>����������� ������</summary>
        [NotNull]
        public string Stop { get; }

        /// <summary>������</summary>
        /// <param name="Start">������ ����������� ������</param>
        /// <param name="Stop">������ ����������� ������</param>
        public Bracket([NotNull] string Start, [NotNull] string Stop)
        {
            this.Start = Start;
            this.Stop = Stop;
        }

        /// <summary>�������� �� ��������������� ������ �������</summary>
        /// <param name="other">����������� �� ��������������� ������</param>
        /// <returns>������, ���� ����������� ������ ������������ ������</returns>
        public bool Equals(Bracket other) => other is { } && (ReferenceEquals(this, other) || string.Equals(Start, other.Start) && string.Equals(Stop, other.Stop));

        /// <summary>�������� �� ���������������</summary>
        /// <param name="obj">����������� ������</param>
        /// <returns>������, ���� ������ - ������ � ��� ������ ���������</returns>
        public override bool Equals(object obj) => obj is { } && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Bracket)obj));

        /// <summary>�������� ���-���</summary>
        /// <returns>���-���</returns>
        public override int GetHashCode() { unchecked { return ((Start?.GetHashCode() ?? 0) * 397) ^ (Stop?.GetHashCode() ?? 0); } }

        object ICloneable.Clone() => Clone();

        /// <summary>������������ ������</summary>
        /// <returns>���� ������</returns>
        [NotNull]
        public virtual Bracket Clone() => new Bracket(Start, Stop);

        /// <summary>��������� ������������� ������</summary>
        /// <returns>��������� �������������</returns>
        public override string ToString() => Suround("...");

        /// <summary>���������� ����� � �������</summary>
        /// <param name="str">����������� �����</param>
        /// <returns>����� � �������</returns>
        [NotNull]
        public string Suround([CanBeNull] string str)
        {
            Trace.TraceWarning("� ������ ����� ������ �������� ������ ������");
            return $"{Start}{str}{Stop}";
        }
    }
}