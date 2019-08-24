namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, ����������� ��������</summary>
    public abstract class OperatorNode : ComputedNode
    {
        /// <summary>�������� �������� �������������� ���� ������������� ��� ������ � ����� ���������</summary>
        public override bool IsPrecomputable
        {
            get
            {
                var is_left_null = ReferenceEquals(Left, null);
                var is_right_null = ReferenceEquals(Right, null);
                return !(is_left_null && is_right_null) && !is_left_null && Left.IsPrecomputable && !is_right_null && Right.IsPrecomputable;
            }
        }

        /// <summary>��������� ���������</summary>
        /// <remarks>
        /// ��� ���� ���������, ��� ������ � ������ ������ ���������� ��������
        /// ������ ������� �����������:
        ///  + - 0
        ///  - - 5
        ///  * - 10
        ///  / - 15
        ///  ^ - 20
        /// </remarks>
        public int Priority { get; protected set; }

        /// <summary>��� ���������</summary>
        public string Name { get; protected set; }

        /// <summary>������������� ���������</summary>
        protected OperatorNode() { }

        /// <summary>������������� ���������</summary>
        /// <param name="Name">��� ���������</param>
        protected OperatorNode(string Name) : this() => this.Name = Name;

        /// <summary>������������� ���������</summary>
        /// <param name="Name">��� ���������</param>
        /// <param name="Priority">��������� ���������</param>
        protected OperatorNode(string Name, int Priority) : this(Name) => this.Priority = Priority;

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => string.Format("{1}{0}{2}", Name, Left?.ToString() ?? "", Right?.ToString() ?? "");

        protected OperatorNode CloneOperatorNode<TOperatorNode>() where TOperatorNode : OperatorNode, new() => 
            new TOperatorNode { Left = Left?.Clone(), Right = Right?.Clone() };
    }
}