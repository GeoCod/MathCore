// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>����������� ��</summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class DependencyOnAttribute : Attribute
    {
        /// <summary>�������, �� �������� ���������� �����������</summary>
        public string Name { get; set; }

        /// <summary>������������� ������ ���������� <see cref="DependencyOnAttribute"/></summary>
        public DependencyOnAttribute() { }

        /// <summary>������������� ������ ���������� <see cref="DependencyOnAttribute"/></summary>
        /// <param name="Name">��� ��������, �� �������� ������� ���������� �������</param>
        public DependencyOnAttribute(string Name) => this.Name = Name;
    }
}