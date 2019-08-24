// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
namespace System
{
    /// <summary>������� �������� ������������������</summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NotSupportedAttribute : Attribute
    {
        /// <summary>���������</summary>
        public string Message { get; set; }

        /// <summary>����� ������� ������������������</summary>
        public NotSupportedAttribute() { }

        /// <summary>����� ������� ������������������</summary>
        /// <param name="Message">��������� (������ �� ��������������?)</param>
        public NotSupportedAttribute(string Message) => this.Message = Message;

        /// <summary>������� �������� "�� ���������"</summary>
        /// <returns>������, ���� ������ ������������� �������</returns>
        public override bool IsDefaultAttribute() => string.IsNullOrEmpty(Message);
    }
}