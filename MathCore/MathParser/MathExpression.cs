using System;
using System.Collections.Generic;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Linq;
using System.Linq.Expressions;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees;
using MathCore.MathParser.ExpressionTrees.Nodes;
// ReSharper disable ClassCanBeSealed.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace MathCore.MathParser
{
    /// <summary>�������������� ���������</summary>
    public class MathExpression : IDisposable, ICloneable<MathExpression>
    {
        /// <summary>������ ��������������� ���������</summary>
        private ExpressionTree _ExpressionTree;

        /// <summary>��������� ���������� ��������������� ���������</summary>
        [NotNull]
        private readonly VariabelsCollection _Variables;

        /// <summary>��������� �������� ��������������� ���������</summary>
        [NotNull]
        private readonly ConstantsCollection _Constants;

        /// <summary>��������� �������, ����������� � ���������</summary>
        [NotNull]
        private readonly FunctionsCollection _Functions;

        /// <summary>��������� ������������</summary>
        [NotNull]
        private readonly FunctionalsCollection _Functionals;

        /// <summary>��� ���������</summary>
        [NotNull]
        private string _Name;

        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException" accessor="set">������� ������ ��� �������</exception>
        [NotNull]
        public string Name
        {
            get => _Name;
            set
            {
                if(value is null)
                    throw new ArgumentNullException(nameof(value), @"�� ������� ��� �������");
                if(string.IsNullOrEmpty(value))
                    throw new ArgumentException(@"������� ������ ��� �������", nameof(value));
                _Name = value;
            }
        }

        /// <summary>�������� �� ��������� ��������������?</summary>
        public bool IsPrecomputable => _ExpressionTree.Root.IsPrecomputable;

        /// <summary>������ ��������������� ���������</summary>
        [NotNull]
        public ExpressionTree Tree { [DST] get => _ExpressionTree; [DST] set => _ExpressionTree = value; }

        /// <summary>����������, �������� � �������������� ���������</summary>
        [NotNull]
        public VariabelsCollection Variable => _Variables;

        /// <summary>���������, �������� � �������������� ���������</summary>
        [NotNull]
        public ConstantsCollection Constants => _Constants;

        /// <summary>��������� �������, ����������� � ���������</summary>
        [NotNull]
        public FunctionsCollection Functions => _Functions;

        /// <summary>��������� ������������</summary>
        [NotNull]
        public FunctionalsCollection Functionals => _Functionals;

        /// <summary>������������� ������� ��������������� ���������</summary>
        /// <param name="Name">��� �������</param>
        public MathExpression([NotNull] string Name = "f")
        {
            _Name = Name;
            _Variables = new VariabelsCollection(this);     // ��������� ����������
            _Constants = new ConstantsCollection(this);     // ��������� ��������
            _Functions = new FunctionsCollection(this);     // ��������� �������
            _Functionals = new FunctionalsCollection(this); // ��������� ������������
        }

        /// <summary>������������� ������ ��������������� ���������</summary>
        /// <param name="Tree">������ ��������������� ���������</param>
        /// <param name="Name">��� �������</param>
        private MathExpression([NotNull] ExpressionTree Tree, [NotNull] string Name = "f")
            : this(Name)
        {
            _ExpressionTree = Tree; //��������� ������ �� ������ ������

            foreach(var tree_node in Tree) // ������� ��� �������� ������
            {
                switch (tree_node)
                {
                    case VariableValueNode value_node:
                        var variable = value_node.Variable;      // ������� ����������
                        if (variable.IsConstant)                 // ���� ���������� - ���������
                            _Constants.Add(variable);            //   ��������� � ��������� ��������
                        else                                     //  �����...
                            _Variables.Add(variable);            //   ��������� � ��������� ����������
                        break;
                    case FunctionNode function_node:
                        _Functions.Add(function_node.Function);  // �� ��������� ������� � ���������
                        break;
                }
            }
        }

        /// <summary>������������� ������ ��������������� ���������</summary>
        /// <param name="StrExpression">��������� ������������� ���������</param>
        /// <param name="Parser">������ �� ������</param>
        internal MathExpression([NotNull] string StrExpression, [NotNull] ExpressionParser Parser)
            : this()
        {
            var terms = new BlockTerm(StrExpression);    // ������� ������ �� ��������
            var root = terms.GetSubTree(Parser, this);   // �������� ������ ������ �� ������� ��������
            _ExpressionTree = new ExpressionTree(root); // ������� ������ ��������� �� �����
        }

        /// <summary>���������� �������������� ���������</summary>
        void IDisposable.Dispose() => _ExpressionTree.Dispose();

        /// <summary>���������� ��������������� ���������</summary>
        /// <returns>�������� ���������</returns>
        public double Compute() => ((ComputedNode)_ExpressionTree.Root).Compute();

        /// <summary>���������� ��������������� ���������</summary>
        /// <returns>�������� ���������</returns>
        public double Compute([NotNull] params double[] arg)
        {
            for(int i = 0, arg_count = arg.Length, var_count = Variable.Count; i < arg_count && i < var_count; i++)
                Variable[i].Value = arg[i];
            return ((ComputedNode)_ExpressionTree.Root).Compute();
        }

        /// <summary>���������� ��������������� ��������� � ������� ��� ����������</summary>
        /// <returns>������� ���� double func(void) ��� ����������</returns>
        [NotNull]
        public Func<double> Compile() => Compile<Func<double>>();

        /// <summary>���������� ������� ����� ����������</summary>
        /// <returns>������� ������� ����� ����������</returns>
        [NotNull]
        public Func<double, double> Compile1() => Compile<Func<double, double>>();

        /// <summary>���������� ������� ���� ����������</summary>
        /// <returns>������� ������� ���� ����������</returns>
        [NotNull]
        public Func<double, double, double> Compile2() => Compile<Func<double, double, double>>();

        /// <summary>���������� ������� ��� ����������</summary>
        /// <returns>������� ������� ��� ����������</returns>
        [NotNull]
        public Func<double, double, double, double> Compile3() => Compile<Func<double, double, double, double>>();

        /// <summary>���������� ��������������� ��������� � ������� ���������� ����</summary>
        /// <param name="ArgumentName">������ ��� ����������</param>
        /// <returns>������� ����������������� ���������</returns>
        [NotNull]
        public Delegate Compile([NotNull] params string[] ArgumentName) => 
            Expression.Lambda(GetExpression(out var vars, ArgumentName), vars).Compile();

        /// <summary>�������������������� ���������� ���.���������</summary>
        /// <param name="ArgumentName">������ ��� ������������� ����������</param>
        /// <returns>������� �������, ����������� �� ���� ������ �������� ����������</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [NotNull]
        public Func<double[], double> CompileMultyParameters([NotNull] params string[] ArgumentName)
        {
            // ������� �������� ����������
            var var_dictionary = new Dictionary<string, int>();

            // �������� ������ ���������� ���������, ���������� ��� ������ ������
            // ������ ������
            var compilation = GetExpression(out _, ArgumentName);

            // ���� ������ ��� ������������� ������� ���������� �� ����
            if((ArgumentName.Length) > 0)
                ArgumentName.Foreach(var_dictionary.Add); // �� ��������� ������� ��������
            else // ���� ������ ���������� �� ������, �� ������ ������� �� ���� ����������
                Variable.Select(v => v.Name).Foreach(var_dictionary.Add);

            // ������ ������� ���������� ��������� � ����� "������������ ������"
            var array_parameter = Expression.Parameter(typeof(double[]), "args_array");
            // ������ ��� ��������� - ������������ �������� �������
            var array_indexers = new Dictionary<int, Expression>();
            // ��������� ����� ��������� ��������� ����������� �������� ������� �� ���������� �������� �������
            Expression GetIndexedParameter(int index)
            {
                // ���� ��� ��� �������� ��������� ������, �� ������ ��������������� ����������
                if (array_indexers.ContainsKey(index)) return array_indexers[index];
                // ����� ������ ����� ����������
                var indexer = Expression.ArrayIndex(array_parameter, Expression.Constant(index));
                array_indexers.Add(index, indexer); // � ��������� ��� � ����
                return indexer; // ���������� ����� ����������
            }

            // ������ ����������� ������ ��������� Linq.Expression 
            var rebuilder = new ExpressionRebuilder();
            // ��������� ���������� ����� ������ �������
            rebuilder.MethodCallVisited += (s, e) => // ���� ��������� ���� ������ - ����� ������, ��...
            {
                var call = e.Argument; // ��������� ������ �� ����
                //���� ������� ������ ������ - ��(!) ����������� �������� � ��� �� ������������� ���� ���������� ������ MathExpressionTree 
                if(!(call.Object is ConstantExpression constant && constant.Value is ExpressionVariabel))
                    return call; // ���������� ����
                //��������� �� ���� ���������� ������
                var v = (ExpressionVariabel)((ConstantExpression)call.Object).Value;
                //���� ���������� ������ - ���������, ���� ���� � ��� ����������� � ������� ������������� ����������
                if(v.IsConstant || !var_dictionary.ContainsKey(v.Name)) return call; // �� ���������� ����
                var index = var_dictionary[v.Name]; // ����������� ������ ����������
                var indexer = GetIndexedParameter(index); // ��������� ���������� �� ���� �� ���������� �������
                return indexer; // �������� ������� ���� ������������
            };

            compilation = rebuilder.Visit(compilation); // ������������ ������
            // �������� ������-���������
            var lambda = Expression.Lambda<Func<double[], double>>(compilation, array_parameter);
            return lambda.Compile(); // ����������� ������-��������� � ���������� �������
        }

        /// <summary>���������� ��������������� ��������� � ������� ���������� ����</summary>
        /// <typeparam name="TDelegate">��� �������� �������</typeparam>
        /// <param name="ArgumentName">������ ��� ����������</param>
        /// <returns>������� ����������������� ���������</returns>
        [NotNull]
        public TDelegate Compile<TDelegate>([NotNull] params string[] ArgumentName)
        {
            var compilation = GetExpression<TDelegate>(out var vars, ArgumentName);
            return Expression.Lambda<TDelegate>(compilation, vars).Compile();
        }

        /// <summary>�������� Linq.Expression ���������, ����������� �� ������ ������ ���������</summary>
        /// <param name="vars">������ ������� ����������</param>
        /// <param name="ArgumentName">������ ��� ����������</param>
        /// <returns>��������� ���� Linq.Expression</returns>
        [NotNull]
        public Expression GetExpression([NotNull] out ParameterExpression[] vars, [NotNull] params string[] ArgumentName)
        {
            vars = ArgumentName.Select(name => Expression.Parameter(typeof(double), name)).ToArray();
            return ((ComputedNode)_ExpressionTree.Root).Compile(vars);
        }

        /// <summary>�������� Linq.Expression ���������, ����������� �� ������ ������ ���������</summary>
        /// <typeparam name="TDelegate">��� �������� ���������</typeparam>
        /// <param name="vars">������ ������� ����������</param>
        /// <param name="ArgumentName">������ ��� ����������</param>
        /// <returns>��������� ���� Linq.Expression</returns>
        [NotNull]
        public Expression GetExpression<TDelegate>(
            [CanBeNull] out ParameterExpression[] vars,
            [NotNull] params string[] ArgumentName)
        {
            var t = typeof(TDelegate);
            vars = null;
            if(ArgumentName.Length == 0)
            {
                var args = t.GetGenericArguments();
                if(args.Length > 1)
                {
                    vars = new ParameterExpression[Math.Min(args.Length - 1, Variable.Count)];
                    for(var i = 0; i < vars.Length; i++)
                        vars[i] = Expression.Parameter(typeof(double), Variable[i].Name);
                }
            }
            else
                vars = ArgumentName.Select(name => Expression.Parameter(typeof(double), name)).ToArray();
            var compilation = vars is null
                ? ((ComputedNode)_ExpressionTree.Root).Compile()
                : ((ComputedNode)_ExpressionTree.Root).Compile(vars);
            return compilation;
        }

        /// <summary>�������������� � ������</summary>
        /// <returns>��������� �������������</returns>
        [NotNull] public override string ToString() => $"{_Name}({_Variables.Select(v => v.Name).ToSeparatedStr(", ")})={_ExpressionTree.Root}";

        /// <summary>������� �������� �� ��������� ��������� � ��������� �������</summary>
        /// <param name="Source">��������� ��������</param>
        /// <param name="Result">��������� �������</param>
        private static void CheckConstatnsCollection([NotNull] MathExpression Source, [NotNull] MathExpression Result) =>
            Source.Constants
               .Select(constant => Result.Variable[constant.Name])
               .Where(c => Result.Variable.Remove(c))
               .Foreach(Result.Constants, (c, constants) => constants.Add(c));

        /// <summary>������������ ���������</summary>
        /// <returns>����� ��������� ������ ���������</returns>
        [NotNull]
        public MathExpression Clone()
        {
            var result = new MathExpression(_ExpressionTree.Clone());
            CheckConstatnsCollection(this, result);
            return result;
        }

        object ICloneable.Clone() => Clone();

        /// <summary>���������� ���� ��������� � �������������� ����-���������</summary>
        /// <param name="x">������ ���������</param>
        /// <param name="y">������ ���������</param>
        /// <param name="node">���� ��������</param>
        /// <returns>�������������� ���������, � ����� ������ �������� ����� ���� ���������. ���������� - ����� ������� � ������� ���������</returns>
        [NotNull]
        protected static MathExpression CombineExpressions([NotNull] MathExpression x, [NotNull] MathExpression y, [NotNull] OperatorNode node)
        {
            var x_tree = x.Tree.Clone();
            var y_tree = y.Tree.Clone();

            if(x_tree.Root is OperatorNode x_operator_node && x_operator_node.Priority < node.Priority)
                x_tree.Root = new ComputedBracketNode(Bracket.NewRound, x_operator_node);
            if(y_tree.Root is OperatorNode y_operator_node_root && y_operator_node_root.Priority < node.Priority)
                y_tree.Root = new ComputedBracketNode(Bracket.NewRound, y_operator_node_root);

            node.Left = x_tree.Root;
            node.Right = y_tree.Root;

            var z = new MathExpression(new ExpressionTree(node));
            CheckConstatnsCollection(x, z);
            CheckConstatnsCollection(y, z);
            return z;
        }

        /// <summary>�������� �������� ���� ���������</summary>
        /// <param name="x">������ ���������</param>
        /// <param name="y">������ ���������</param>
        /// <returns>���������-�����, ������ �������� - ���� �����. ���������� - ����� ��������� ���������</returns>
        [NotNull]
        public static MathExpression operator +([NotNull] MathExpression x, [NotNull] MathExpression y) => CombineExpressions(x, y, new AdditionOperatorNode());

        /// <summary>�������� ��������� ���� ���������</summary>
        /// <param name="x">�����������</param>
        /// <param name="y">����������</param>
        /// <returns>���������-��������, ������ �������� - ���� ��������. ���������� - ����� ��������� ����������� � ������������</returns>
        [NotNull]
        public static MathExpression operator -([NotNull] MathExpression x, [NotNull] MathExpression y) => CombineExpressions(x, y, new SubstractionOperatorNode());

        /// <summary>�������� ��������� ���� ���������</summary>
        /// <param name="x">������ �����������</param>
        /// <param name="y">������ �����������</param>
        /// <returns>���������-������������, ������ �������� - ���� ������������. ���������� - ����� ��������� ������������</returns>
        [NotNull]
        public static MathExpression operator *([NotNull] MathExpression x, [NotNull] MathExpression y) => CombineExpressions(x, y, new MultiplicationOperatorNode());

        /// <summary>�������� ������� ���� ���������</summary>
        /// <param name="x">�������</param>
        /// <param name="y">��������</param>
        /// <returns>���������-�������, ������ �������� - ���� �������. ���������� - ����� ��������� �������� � ��������</returns>
        [NotNull]
        public static MathExpression operator /([NotNull] MathExpression x, [NotNull] MathExpression y) => CombineExpressions(x, y, new DivisionOperatorNode());

        /// <summary>�������� ���������� � �������</summary>
        /// <param name="x">���������</param>
        /// <param name="y">���������� �������</param>
        /// <returns>���������-�������, ������ �������� - ���� �������. ���������� - ����� ��������� ��������� � ���������� �������</returns>
        [NotNull]
        public static MathExpression operator ^([NotNull] MathExpression x, [NotNull] MathExpression y) => CombineExpressions(x, y, new PowerOperatorNode());

        /// <summary>�������� �������� ���������� ����� ��������������� ��������� � ���� ������ ���������</summary>
        /// <param name="Expression">�������������� ���������</param>
        /// <returns>������ ��������������� ���������</returns>
        [NotNull]
        public static implicit operator ExpressionTree([NotNull] MathExpression Expression) => Expression.Tree;

        /// <summary>�������� �������� ���������� ����� ������ ��������� � ���� ��������������� ���������</summary>
        /// <param name="Tree">������ ��������������� ���������</param>
        /// <returns>�������������� ���������, ���������� ��������� ������</returns>
        [NotNull]
        public static implicit operator MathExpression([NotNull] ExpressionTree Tree) => new MathExpression(Tree);

        /// <summary>�������� �������� ���������� ����� ��������������� ��������� � ���� �������� ������� double Func(void)</summary>
        /// <param name="expr">�������������� ���������</param>
        /// <returns>��������� ���������� ��������������� ���������</returns>
        [NotNull]
        public static implicit operator Func<double>([NotNull] MathExpression expr) => expr.Compile();
    }
}