namespace System
{
    /// <summary>��������� ��������� ��� ����������, ����������� � ������, ������� ������ ���� ������� ����� ��������� �������� ��������</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ChangedHandlerAttribute : Attribute
    {
        /// <summary>��� ������-������� �� ��������� �������� ��������</summary>
        public string MethodName { get; set; }

        /// <summary>������������� ������ ���������� <see cref="DependencyOnAttribute"/></summary>
        public ChangedHandlerAttribute() { }

        /// <summary>������������� ������ ���������� <see cref="DependencyOnAttribute"/></summary>
        /// <param name="MethodName">��� ������-������� �� ��������� �������� ��������</param>
        public ChangedHandlerAttribute(string MethodName) => this.MethodName = MethodName;
    }
}