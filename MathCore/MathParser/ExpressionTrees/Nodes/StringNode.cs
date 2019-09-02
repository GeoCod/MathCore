namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>��������� ���� ������ ��������������� ���������</summary>
    public class StringNode : ParsedNode
    {
        /// <summary>�������� ����</summary>
        public string Value { get; set; }

        /// <summary>������������� ������ ���������� ����</summary>
        public StringNode() { }

        /// <summary>������������� ������ ���������� ����</summary>
        /// <param name="value">�������� ����</param>
        public StringNode(string value) => Value = value;

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new StringNode(Value)
        {
            Left = Left?.Clone(),
            Right = Right?.Clone()
        };

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{(Left is null ? "" : $"{Left}")}{Value}{(Right is null ? "" : $"{Right}")}";

        /// <summary>�������� �������� �������������� ������ � ���� ���������� ����</summary>
        /// <param name="value">��������� ��������</param>
        /// <returns>��������� ����</returns>
        public static implicit operator StringNode(string value) => new StringNode(value);

        /// <summary>�������� �������� �������������� ���������� ���� � ���������� ����</summary>
        /// <param name="node">��������� ����</param>
        /// <returns>�������� ���������� ����</returns>
        public static implicit operator string(StringNode node) => node.Value;
    }
}