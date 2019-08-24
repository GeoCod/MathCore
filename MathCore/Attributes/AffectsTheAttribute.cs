// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>������� ��</summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AffectsTheAttribute : Attribute
    {
        /// <summary>�������, �� ������� ����������� �������</summary>
        public string Name { get; set; }

        /// <summary>������������� ������ ���������� <see cref="AffectsTheAttribute"/></summary>
        public AffectsTheAttribute() { }

        /// <summary>������������� ������ ���������� <see cref="AffectsTheAttribute"/></summary>
        /// <param name="Name">��� ��������, �� ��� ���������� ������� ��������� �������</param>
        public AffectsTheAttribute(string Name) => this.Name = Name;
    }
}