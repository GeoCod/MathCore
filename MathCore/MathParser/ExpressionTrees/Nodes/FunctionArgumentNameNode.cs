using System;
using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���������, ���������� �������� �� ��������� �������</summary>
    public class FunctionArgumentNameNode : OperatorNode
    {
        /// <summary>���� ���������</summary>
        public ExpressionTreeNode ArgumentNode => Right;

        /// <summary>���� ����� ���������</summary>
        public string ArgumentName => ((StringNode)Left)?.Value;

        /// <summary>������������� ���� ������ ���������� �� ��������� �������</summary>
        public FunctionArgumentNameNode() : base(":", -100) { }

        /// <summary>������������� ���� ������ ���������� �� ��������� �������</summary>
        /// <param name="Name">���</param>
        /// <param name="Expression">���� ��������� ���������</param>
        public FunctionArgumentNameNode(string Name, ExpressionTreeNode Expression = null) : this(new StringNode(Name), Expression) { }

        /// <summary>������������� ���� ������ ���������� �� ��������� �������</summary>
        /// <param name="Name">���</param>
        /// <param name="Expression">��������� ����</param>
        public FunctionArgumentNameNode(StringNode Name, ExpressionTreeNode Expression)
            : this()
        {
            if(!Name.Value.IsNullOrEmpty())
                Left = Name;
            Right = Expression;
        }

        /// <summary>����� ���������� �������� ����</summary>
        /// <returns>�������� ���������</returns>
        public override double Compute() => ((ComputedNode)ArgumentNode).Compute();

        /// <summary>���������� ���� ���������</summary>
        /// <returns>���������������� ���������</returns>
        public override Expression Compile() => ((ComputedNode)ArgumentNode).Compile();

        /// <summary>���������� ���� ��������� � ������ ������ ����������</summary>
        /// <param name="Parameters">������ ���������� �������� ����������</param>
        /// <returns>���������������� �������� ���� ��������� ������ ���������</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => ((ComputedNode)ArgumentNode).Compile(Parameters);

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new FunctionArgumentNameNode { Right = Right.Clone(), Left = Left.Clone() };
    }
}