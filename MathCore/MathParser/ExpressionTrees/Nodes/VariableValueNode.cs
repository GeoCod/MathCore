using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using MathCore.Annotations;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������, �������� ����������</summary>
    public class VariableValueNode : ValueNode
    {
        private ExpressionVariabel _Variable;

        /// <summary>������� ����������� ��������� ������������ ��������</summary>
        public override bool IsPrecomputable => _Variable.IsPrecomputable;

        /// <summary>������ �� ����������</summary>
        [NotNull]
        public ExpressionVariabel Variable { get => _Variable; set => _Variable = value; }

        /// <summary>�������� ����</summary>
        public override double Value { get => _Variable.Value; set => _Variable.Value = value; }

        /// <summary>��� ����������</summary> 
        public string Name { [DST] get => _Variable.Name; }

        /// <summary>����� ���� ����������</summary>
        /// <param name="Variable">����������</param>
        public VariableValueNode([NotNull] ExpressionVariabel Variable)
        {
            Contract.Requires(Variable != null);
            _Variable = Variable;
        }

        /// <summary>�������������� � ��������� �����</summary>
        /// <returns>��������� �������������</returns>
        public override string ToString() => $"{Left?.ToString() ?? ""}{Name}{Right?.ToString() ?? ""}";

        /// <summary>��������� �������� ���������</summary>
        /// <returns>��������� �������� ���������</returns>
        [DST]
        public override double Compute() => _Variable.GetValue();

        /// <summary>�������������� � ���������</summary>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        public override Expression Compile() => Expression.Call
        (
            Expression.Constant(_Variable),
            Variable.GetType().GetMethod("GetValue", new Type[0]) ?? throw new InvalidOperationException("����� GetValue �� ������")
        );

        /// <summary>�������������� � ���������</summary>
        /// <param name="Parameters">������ ����������</param>
        /// <returns>���������������� ��������� System.Linq.Expressions</returns>
        public override Expression Compile(ParameterExpression[] Parameters) => Parameters.Find(p => p.Name == Name) ?? Compile();

        public override IEnumerable<ExpressionVariabel> GetVariables() => base.GetVariables().AppendFirst(_Variable);

        /// <summary>������������ ����</summary>
        /// <returns>���� ����</returns>
        public override ExpressionTreeNode Clone() => new VariableValueNode(_Variable.Clone())
        {
            Left = Left?.Clone(),
            Right = Right?.Clone()
        };
    }
}