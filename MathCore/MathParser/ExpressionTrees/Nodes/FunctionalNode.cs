using System;
using System.Collections.Generic;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Linq;
using System.Linq.Expressions;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    public class FunctionalNode : ComputedNode
    {
        /// <summary>��� ����</summary>
        public string Name { get; private set; }

        /// <summary>��������� ����������</summary>
        private readonly MathExpression _ParametersExpression = new MathExpression("Param");

        /// <summary>��������� ���� �������</summary>
        private readonly MathExpression _CoreExpression = new MathExpression(nameof(Core));

        /// <summary>��������� ����������</summary>
        public MathExpression Parameters => _ParametersExpression;

        /// <summary>��������� ���� �������</summary>
        public MathExpression Core => _CoreExpression;

        /// <summary>��������</summary>
        public Functional Operator { get; set; }

        [DST]
        public FunctionalNode(string Name) => this.Name = Name;

        internal FunctionalNode(FunctionalTerm term, ExpressionParser Parser, MathExpression Expression)
            : this(term.Name)
        {
            // ����������� ����� ���� �������
            _CoreExpression.Tree = new ExpressionTree(term.Block.GetSubTree(Parser, _CoreExpression));
            // ����������� ����� ����������
            _ParametersExpression.Tree = new ExpressionTree(term.Parameters.GetSubTree(Parser, _ParametersExpression));

            Parser.ProcessVariables(_CoreExpression);
            Parser.ProcessVariables(_ParametersExpression);

            Parser.ProcessFunctions(_CoreExpression);
            Parser.ProcessFunctions(_ParametersExpression);

            // ���������� ���������� � �������� ����� ����������
            _ParametersExpression.Tree
                .Where(n => n is VariableValueNode) // �������� �� ���� ����� � �����������
                .Cast<VariableValueNode>()
                .Where(v => !v.Variable.IsConstant)
                .Foreach(_ParametersExpression.Variable, _CoreExpression.Variable, 
                    (v, expr_vars, core_vars) =>
                    {
                        expr_vars.RemoveFromCollection(v.Variable);
                        expr_vars.Add(v.Variable = core_vars[v.Variable.Name]);
                    });

            //������ � ������� � ���������
            Operator = Parser.GetFunctional(term.Name);
            //������������� ���������
            Operator.Initialize(_ParametersExpression, _CoreExpression, Parser, Expression);
        }

        public override IEnumerable<ExpressionVariabel> GetVariables()
        {
            ExpressionVariabel iterator = null;
            var params_node = _ParametersExpression.Tree.Root;
            if(params_node is EqualityOperatorNode && params_node.Left is VariableValueNode node)
                iterator = node.Variable;
            return _CoreExpression.Tree.Root.GetVariables().Where(v => v != iterator);
        }

        /// <summary>��������� �������� ���������</summary>
        /// <returns>��������� �������� ���������</returns>
        [DST]
        public override double Compute() => Operator.GetValue(_ParametersExpression, _CoreExpression);

        /// <summary>�������������� � ���������</summary>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        [DST]
        public override Expression Compile() => Operator.Compile(_ParametersExpression, _CoreExpression);

        /// <summary>�������������� � ���������</summary>
        /// <param name="Parameters">������ ����������</param>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        [DST]
        public override Expression Compile(params ParameterExpression[] Parameters) => Operator.Compile(_ParametersExpression, _CoreExpression, Parameters);

        /// <summary>�������������� ���� � ������</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{Operator.Name}[{_ParametersExpression.Tree.Root}]{{{_CoreExpression.Tree.Root}}}";

        /// <summary>������������ ���������</summary>
        /// <returns>���� ���������</returns>
        public override ExpressionTreeNode Clone() => throw new NotImplementedException();
    }
}