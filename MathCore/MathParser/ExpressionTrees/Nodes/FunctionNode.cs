using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���������, ���������� �������</summary>
    public class FunctionNode : ComputedNode
    {
        /// <summary>��� �������</summary>
        public string Name { get; }

        /// <summary>������ ��� ���������� �������</summary>
        public string[] ArgumentsNames => Arguments.Select(a => a.Key).ToArray();

        /// <summary>������������ ���������� �������</summary>
        public IEnumerable<KeyValuePair<string, ExpressionTreeNode>> Arguments => GetFunctionArgumentNodes(this);

        /// <summary>������� ����</summary>
        public ExpressionFunction Function { get; set; }

        /// <summary>������������� ������ ��������������� ����</summary>
        internal FunctionNode() { }

        /// <summary>������������� ������ ��������������� ����</summary>
        /// <param name="Name">��� �������</param>
        internal FunctionNode(string Name) { this.Name = Name; }

        /// <summary>������������� ������ ��������������� ����</summary>
        /// <param name="Name">��� �������</param>
        internal FunctionNode(StringNode Name) : this(Name.Value) { }

        /// <summary>������������� ������ ��������������� ����</summary>
        /// <param name="Term">��������� �������</param>
        /// <param name="Parser">������ ���������</param>
        /// <param name="Expression">�������������� ���������</param>
        internal FunctionNode(FunctionTerm Term, ExpressionParser Parser, MathExpression Expression)
            : this(Term.Name)
        {

            var arg = Term.Block.GetSubTree(Parser, Expression);
            if (!(arg is FunctionArgumentNode))
                switch (arg)
                {
                    case FunctionArgumentNameNode name:
                        arg = new FunctionArgumentNode(name);
                        break;
                    case VariableValueNode _:
                        arg = new FunctionArgumentNode(null, arg);
                        break;
                    case VariantOperatorNode _ when arg.Left is VariableValueNode:
                        arg = new FunctionArgumentNode(((VariableValueNode)arg.Left).Name, arg.Right);
                        break;
                    default:
                        arg = new FunctionArgumentNode(null, arg);
                        break;
                }
            Right = arg;
            Function = Expression.Functions[Name, ArgumentsNames];
        }

        /// <summary>���������� �������� ����</summary>
        /// <returns>�������� �������</returns>
        public override double Compute() => Function.GetValue(Arguments.Select(k => ((ComputedNode)k.Value).Compute()).ToArray());

        /// <summary>�������� ������������ ���������� �������</summary>
        /// <param name="FunctionNode">���� �������</param>
        /// <returns>������������ ���������� �������</returns>
        private static IEnumerable<KeyValuePair<string, ExpressionTreeNode>> GetFunctionArgumentNodes(ExpressionTreeNode FunctionNode) => 
            FunctionNode.Right is FunctionArgumentNode node
                ? FunctionArgumentNode.EnumArguments(node)
                : throw new FormatException();

        /// <summary>���������� ����</summary>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile() =>
            Expression.Call(Function.Delegate.Target != null ? Expression.Constant(Function.Delegate.Target) : null,
                    Function.Delegate.Method,
                    Arguments.Select(a => ((ComputedNode)a.Value).Compile()));

        /// <summary>���������� ����</summary>
        /// <param name="Parameters">������ ���������� ���������</param>
        /// <returns>���������������� ��������� ����</returns>
        public override Expression Compile(ParameterExpression[] Parameters) =>
            Expression.Call(Function.Delegate.Target != null ? Expression.Constant(Function.Delegate.Target) : null,
                    Function.Delegate.Method,
                    Arguments.Select(a => ((ComputedNode)a.Value).Compile(Parameters)));

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new FunctionNode(Name)
        {
            Left = Left?.Clone(),
            Right = Right?.Clone(),
            Function = Function.Clone()
        };

        /// <summary>��������� ������������� ����</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{Name}({Arguments.Select(v => string.IsNullOrEmpty(v.Key) ? v.Value.ToString() : $"{v.Key}:{v.Value.ToString()}").ToSeparatedStr(", ")})";
    }
}