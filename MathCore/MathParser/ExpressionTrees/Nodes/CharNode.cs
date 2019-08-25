namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���������� ���� ������ ��������������� ���������</summary>
    public class CharNode : ParsedNode
    {
        /// <summary>�������� ������� ����</summary>
        public char Value { get; set; }

        /// <summary>������������� ������ ���������� ����</summary>
        public CharNode() { }

        /// <summary>������������� ������ ���������� ����</summary>
        /// <param name="value">�������� ����</param>
        public CharNode(char value) => Value = value;

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new CharNode(Value)
        {
            Left = Left?.Clone(),
            Right = Right?.Clone()
        };

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{(Left == null ? "" : $"{Left}")}{Value}{(Right == null ? "" : $"{Right}")}";

        /// <summary>�������� �������� �������������� ������ � ���� ���������� ����</summary>
        /// <param name="value">��������� ��������</param>
        /// <returns>���������� ����</returns>
        public static implicit operator CharNode(char value) { return new CharNode(value); }

        /// <summary>�������� �������� �������������� ���������� ���� � ����������� ����</summary>
        /// <param name="node">���������� ����</param>
        /// <returns>�������� ����������� ����</returns>
        public static implicit operator char(CharNode node) { return node.Value; }
    }
}