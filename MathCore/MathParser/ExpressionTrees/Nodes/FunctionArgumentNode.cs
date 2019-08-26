using System.Collections.Generic;
using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, �������� ������ �� ��������� �������</summary>
    public class FunctionArgumentNode : OperatorNode
    {
        /// <summary>������������ ���������� ������� � ����������</summary>
        /// <param name="Node">������ ���� ���������</param>
        /// <returns>������������ ��� �����-����� ������ ���������</returns>
        public static IEnumerable<KeyValuePair<string, ExpressionTreeNode>> EnumArguments(FunctionArgumentNode Node)
        {
            while(Node != null)
            {
                yield return new KeyValuePair<string, ExpressionTreeNode>(Node.ArgumentName, Node.ArgumentSubtree);
                Node = Node.Right as FunctionArgumentNode;
            }
        }

        /// <summary>�������� ��������� - ������ ���������</summary>
        public ExpressionTreeNode ArgumentSubtree => Left is FunctionArgumentNameNode ? Left.Right : Left;

        /// <summary>��� ��������� - ����� ���������</summary>
        public string ArgumentName => Left is FunctionArgumentNameNode node ? node.ArgumentName : "";

        /// <summary>������������� ����-���������</summary>
        public FunctionArgumentNode() : base(",", -20) { }

        /// <summary>������������� ����-���������</summary>
        /// <param name="Name">��� ���������</param>
        /// <param name="Node">���� ��������� ���������</param>
        public FunctionArgumentNode(string Name, ExpressionTreeNode Node) : this(new FunctionArgumentNameNode(Name, Node)) { }

        /// <summary>������������� ����-���������</summary>
        /// <param name="Node">���� ��������� ���������</param>
        public FunctionArgumentNode(FunctionArgumentNameNode Node) : this() => Left = Node;

        /// <summary>���������� �������� ����</summary>
        /// <returns>�������� ����</returns>
        public override double Compute() => ((ComputedNode)ArgumentSubtree).Compute();

        /// <summary>���������� ���� ���������</summary>
        /// <returns>���������������� ��������� ����� ��������� ���������</returns>
        public override Expression Compile() => ((ComputedNode)ArgumentSubtree).Compile();

        /// <summary>���������� ���� ���������</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ����� ��������� ���������</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => ((ComputedNode)ArgumentSubtree).Compile(Parameters);

        /// <summary>������������ ����</summary>
        /// <returns>������������ ����</returns>
        public override ExpressionTreeNode Clone() => new FunctionArgumentNode { Left = Left.Clone(), Right = Right?.Clone() };
    }
}