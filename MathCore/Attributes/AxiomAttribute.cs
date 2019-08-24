// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>��������� ������ ��������� �� ������� �������������, ��� ��������</summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AxiomAttribute : Attribute
    {
        /// <summary>�������������� �����������</summary>
        public string Comment { get; set; }

        /// <summary>������������� ������ ���������� <see cref="AxiomAttribute"/></summary>
        public AxiomAttribute() { }

        /// <summary>������������� ������ ���������� <see cref="AxiomAttribute"/></summary>
        /// <param name="Comment">�������������� �����������</param>
        public AxiomAttribute(string Comment) => this.Comment = Comment;

        /// <inheritdoc />
        public override string ToString() => $"Comment: {Comment}";
    }
}