using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, ����������� ������ � ������������ ����������</summary>
    public class ComputedBracketNode : ComputedNode
    {
        /// <summary>������</summary>
        private readonly Bracket _Bracket;

        /// <summary>������</summary>
        public Bracket Bracket => _Bracket;

        /// <summary>����������� ������� ���� ������</summary>
        /// <param name="bracket">������</param>
        /// <param name="Node">����-����������</param>
        public ComputedBracketNode(Bracket bracket, ExpressionTreeNode Node = null)
        {
            _Bracket = bracket;
            Left = Node;
        }

        /// <summary>��������� �������� ����</summary>
        /// <returns>�������� ���������� ����</returns>
        public override double Compute() => ((ComputedNode)Left).Compute();

        /// <summary>���������� ����</summary>
        /// <returns>���������� ����������� ����</returns>
        public override Expression Compile() => ((ComputedNode)Left).Compile();

        /// <summary>���������� ���� � �����������</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������� ���������� ����</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => ((ComputedNode)Left).Compile(Parameters);

        /// <summary>���� ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new ComputedBracketNode(_Bracket)
        {
            Left = Left?.Clone(),
            Right = Right?.Clone()
        };

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{_Bracket.Suround(Left.ToString())}{Right?.ToString() ?? ""}";
    }
}