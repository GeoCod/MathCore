using System;
using System.Linq.Expressions;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������, �������� ����������� ��������</summary>
    public class ConstValueNode : ValueNode
    {
        /// <summary>�������� ����</summary>
        private readonly double _Value;

        /// <summary>���� ����������� ��������� �������� ��� �����������. ������ = true</summary>
        public override bool IsPrecomputable => true;

        /// <summary>�������� ����. �� ������������ ����������</summary>
        public override double Value { [DST] get => _Value; [DST] set => throw new NotSupportedException(); }

        /// <summary>������������� ������������ ����</summary>
        [DST]
        public ConstValueNode() { }

        /// <summary>������������� ������������ ����</summary>
        /// <param name="Value">�������� ����</param>
        [DST]
        public ConstValueNode(double Value) => _Value = Value;

        /// <summary>������������� ������������ ����</summary>
        /// <param name="Value">�������� ����</param>
        [DST]
        public ConstValueNode(int Value) : this((double)Value) { }

        /// <summary>��������� �������� ���������</summary>
        /// <returns>��������� �������� ���������</returns>
        [DST]
        public override double Compute() => _Value;

        /// <summary>�������������� � ���������</summary>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        [DST]
        public override Expression Compile() => _Value.ToExpression();

        /// <summary>�������������� � ���������</summary>
        /// <param name="Parameters">������ ����������</param>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        public override Expression Compile(params ParameterExpression[] Parameters) => Compile();

        /// <summary>������������ ���������</summary>
        /// <returns>���� ���������</returns>
        public override ExpressionTreeNode Clone() => new ConstValueNode(Value)
        {
            Left = Left?.Clone(),
            Right = Right?.Clone()
        };
    }
}