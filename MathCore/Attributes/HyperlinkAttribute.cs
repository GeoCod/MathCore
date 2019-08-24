// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>������</summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class HyperlinkAttribute : Attribute
    {
        /// <summary>������</summary>
        public string Link { get; set; }

        /// <summary>������������� ������ ���������� <see cref="HyperlinkAttribute"/></summary>
        public HyperlinkAttribute() {  }

        /// <summary>������������� ������ ���������� <see cref="HyperlinkAttribute"/></summary>
        /// <param name="Link">����� ������</param>
        public HyperlinkAttribute(string Link) => this.Link = Link;

        /// <summary>�������� �������� ���������� ���� <see cref="HyperlinkAttribute"/> � <see cref="Uri"/></summary>
        /// <param name="A">������� ������, ������������� � <see cref="Uri"/></param>
        public static implicit operator Uri(HyperlinkAttribute A) => new Uri(A.Link);
    }
}