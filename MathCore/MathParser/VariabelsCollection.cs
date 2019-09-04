using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathCore.Annotations;
using MathCore.MathParser.ExpressionTrees.Nodes;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace MathCore.MathParser
{
    /// <summary>��������� ����������</summary>
    [DebuggerDisplay("Variabels count = {" + nameof(Count) + "}")]
    public class VariabelsCollection : IEnumerable<ExpressionVariabel>
    {
        /// <summary>�������������� ���������</summary>
        private readonly MathExpression _Expression;

        [NotNull]
        private readonly List<ExpressionVariabel> _Variabels = new List<ExpressionVariabel>();

        /// <summary>���������� ���������� � ���������</summary>
        public int Count => _Variabels.Count;

        /// <summary>�������� ���������� ���������</summary>
        /// <param name="Name">��� ����������</param>
        /// <returns>���������� � ��������� ������</returns>
        [NotNull]
        public ExpressionVariabel this[[NotNull] string Name]
        {
            [DST]
            get
            {
                if (Name is null) throw new ArgumentNullException(nameof(Name));
                if (string.IsNullOrEmpty(Name)) throw new ArgumentOutOfRangeException(nameof(Name));
                return (_Variabels.Find(v => v.Name == Name)
                       ?? new ExpressionVariabel(Name).InitializeObject(this, (v, vc) => vc.Add(v)))
                       ?? throw new InvalidOperationException();
            }
            [DST]
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                if (Name is null) throw new ArgumentNullException(nameof(Name));
                if (string.IsNullOrEmpty(Name)) throw new ArgumentOutOfRangeException(nameof(Name));
                var old_var = _Variabels.Find(v => v.Name == Name);

                if (value is LamdaExpressionVariable || value is EventExpressionVariable)
                {
                    value.Name = Name;
                    _Expression.Tree //������ ��� ���� ������
                                     // ���������� ������ ����������
                                .Where(node => node is VariableValueNode)
                                .Cast<VariableValueNode>()
                                // � ������� ��� ������������� ���������
                                .Where(node => node.Variable.Name == Name)
                                // � ��� ������� ���� �������� ���������� �� ���������
                                .Foreach(value, (node, v) => node.Variable = v);
                    _Variabels.Remove(old_var);
                    Add(value);
                }
                else if (old_var is null)
                    Add(value);
                else
                    old_var.Value = value.GetValue();
            }

        }

        /// <summary>�������� ���������� ���������</summary>
        /// <param name="i">������ ����������</param>
        /// <returns>���������� � ��������� ��������</returns>
        [NotNull]
        public ExpressionVariabel this[int i] => _Variabels[i];

        /// <summary>������������ ���� ��� ���������� ���������</summary>
        [NotNull]
        public IEnumerable<string> Names => _Variabels.Select(v => v.Name);

        /// <summary>������������� ����� ��������� ����������</summary>
        /// <param name="expression">�������������� ���������, �������� ����������� ���������</param>
        public VariabelsCollection([NotNull] MathExpression expression) => _Expression = expression;

        /// <summary>�������� ���������� � ���������</summary>
        /// <param name="Variable">����������</param>
        /// <returns>������, ���� ���������� ���� ���������</returns>
        public bool Add([NotNull] ExpressionVariabel Variable)
        {
            var variable = _Variabels.Find(v => v.Name == Variable.Name);
            if (variable != null) return false;
            Variable.IsConstant = false;
            _Variabels.Add(Variable);
            return true;
        }

        public bool Replace([NotNull] string Name, [NotNull] ExpressionVariabel variable)
        {
            var replaced = false;
            var old_var = _Variabels.Find(v => v.Name == Name);
            if (old_var is null) return false;


            _Expression.Tree //������ ��� ���� ������
                             // ���������� ������ ����������
                                .Where(node => node is VariableValueNode)
                                .Cast<VariableValueNode>()
                                // � ������� ��� ������������� ���������
                                .Where(node => node.Variable.Name == Name)
                                // � ��� ������� ���� �������� ���������� �� ���������
                                // ReSharper disable once HeapView.CanAvoidClosure
                                .Foreach(node =>
                                {
                                    node.Variable = variable;
                                    replaced = true;
                                });
            _Variabels.Remove(old_var);
            Add(variable);
            if (replaced)
                variable.Name = Name;
            return replaced;
        }

        /// <summary>����������� ���������� �� ��������� ���������� � ��������� ��������</summary>
        /// <param name="Variable">������������ ����������</param>
        /// <returns>������, ���� ���������� ���� ���������� �� ��������� ���������� � ��������� ��������</returns>
        public bool MoveToConstCollection([NotNull] string Variable) => MoveToConstCollection(this[Variable]);

        /// <summary>����������� ���������� �� ��������� ���������� � ��������� ��������</summary>
        /// <param name="Variable">������������ ����������</param>
        /// <returns>������, ���� ���������� ���� ���������� �� ��������� ���������� � ��������� ��������</returns>
        public bool MoveToConstCollection([NotNull] ExpressionVariabel Variable) =>
            Exist(v => ReferenceEquals(v, Variable))
            && _Variabels.Remove(Variable) && _Expression.Constants.Add(Variable);

        /// <summary>�������� ���������� �� ���������</summary>
        /// <param name="Variable">��������� ����������</param>
        /// <returns>������, ���� �������� ������ �������</returns>
        public bool Remove([NotNull] ExpressionVariabel Variable) =>
            !_Expression.Tree
               .Where(n => n is VariableValueNode)
               .Any(n => ReferenceEquals(((VariableValueNode)n).Variable, Variable))
            && _Variabels.Remove(Variable);

        /// <summary>������� ���������� �� ���������</summary>
        /// <param name="Variable">��������� ����������</param>
        /// <returns>������, ���� ���������� ������� �������</returns>
        public bool RemoveFromCollection([NotNull] ExpressionVariabel Variable) => _Variabels.Remove(Variable);

        /// <summary>�������� ��������� ����������</summary>
        public void ClearCollection() => _Variabels.Clear();

        /// <summary>���������� �� � ��������� ���������� � ��������� �������</summary>
        /// <param name="Name">������� ��� ����������</param>
        /// <returns>������, ���� � ��������� ������������� ���������� � ��������� ������</returns>
        public bool Exist([NotNull] string Name) => Exist(v => v.Name == Name);

        /// <summary>�������� �� ������������� ���������� � ���������</summary>
        /// <param name="variable">����������� ����������</param>
        /// <returns>������, ���� ��������� ���������� ������ � ���������</returns>
        public bool Exist([NotNull] ExpressionVariabel variable) => _Variabels.Contains(variable);

        /// <summary>���������� �� ���������� � ��������� � �������� ��������� ������</summary>
        /// <param name="exist">�������� ������ ����������</param>
        /// <returns>������, ���� ������� ���������� �� ���������� ��������</returns>
        public bool Exist([NotNull] Predicate<ExpressionVariabel> exist) => _Variabels.Exists(exist);

        /// <summary>���������� �� ���� ���������� � ������ � ��������� ������</summary>
        /// <param name="Name">������� ��� ����������</param>
        /// <returns>������, ���� ��������� ��� ���������� ���������� � ������</returns>
        public bool ExistInTree([NotNull] string Name) => ExistInTree(v => v.Name == Name);

        /// <summary>���������� �� ���� ���������� � ������</summary>
        /// <param name="exist">�������� ������</param>
        /// <returns>������, ���� ������ ���� �� ���������� ��������</returns>
        public bool ExistInTree([NotNull] Func<VariableValueNode, bool> exist) =>
            _Expression.Tree
               .Where(n => n is VariableValueNode)
               .Cast<VariableValueNode>()
               .Any(exist);

        /// <summary>�������� ������������ ����� ���������� � ��������� ������</summary>
        /// <param name="VariableName">������� ��� ����������</param>
        /// <returns>������������ ����� � ����������� � ��������� ������</returns>
        [NotNull]
        public IEnumerable<VariableValueNode> GetTreeNodes([NotNull] string VariableName) => GetTreeNodes(v => v.Name == VariableName);

        /// <summary>�������� ������������ ����� ������ � �����������</summary>
        /// <param name="selector">����� ������� �����</param>
        /// <returns>������������ ����� ����������</returns>
        [NotNull]
        public IEnumerable<VariableValueNode> GetTreeNodes([NotNull] Func<VariableValueNode, bool> selector) =>
            _Expression.Tree
               .Where(n => n is VariableValueNode)
               .Cast<VariableValueNode>()
               .Where(selector);

        /// <summary>�������� ������������ ����� ������ ���������, ���������� ��������� ��� ����������</summary>
        /// <typeparam name="TVariable">��� ����������</typeparam>
        /// <returns>������������ ����� ������ � ��������� ����� ����������</returns>
        [NotNull]
        public IEnumerable<VariableValueNode> GetTreeNodesOf<TVariable>()
            where TVariable : ExpressionVariabel =>
            _Expression.Tree
               .Where(n => n is VariableValueNode)
               .Cast<VariableValueNode>()
               .Where(n => n.Variable is TVariable);

        /// <summary>�������� ������������ ����� ������ ���������, ���������� ��������� ��� ����������</summary>
        /// <typeparam name="TVariable">��� ����������</typeparam>
        /// <param name="selector">����� ������ ����� �� ������������ � ��� ����������</param>
        /// <returns>������������ ����� ������ � ��������� ����� ����������</returns>
        [NotNull]
        public IEnumerable<VariableValueNode> GetTreeNodesVOf<TVariable>([NotNull] Func<TVariable, bool> selector)
            where TVariable : ExpressionVariabel =>
            _Expression.Tree
               .Where(n => n is VariableValueNode)
               .Cast<VariableValueNode>()
               .Where(n => n.Variable is TVariable)
               .Where(n => selector((TVariable)n.Variable));

        /// <summary>�������� ������������ ����� ������ ���������, ���������� ��������� ��� ����������</summary>
        /// <typeparam name="TVariable">��� ����������</typeparam>
        /// <param name="selector">����� ������ �����</param>
        /// <returns>������������ ����� ������ � ��������� ����� ����������</returns>
        [NotNull]
        public IEnumerable<VariableValueNode> GetTreeNodesOf<TVariable>([NotNull] Func<VariableValueNode, bool> selector)
            where TVariable : ExpressionVariabel =>
            _Expression.Tree
               .Where(n => n is VariableValueNode)
               .Cast<VariableValueNode>()
               .Where(n => n.Variable is TVariable)
               .Where(selector);

        /// <summary>���������� �������������, ����������� ������� ��������� � ���������</summary>
        /// <returns>
        /// ��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        IEnumerator<ExpressionVariabel> IEnumerable<ExpressionVariabel>.GetEnumerator() => _Variabels.GetEnumerator();

        /// <summary>���������� �������������, ������� ������������ ������� ��������� ���������</summary>
        /// <returns>
        /// ������ <see cref="T:System.Collections.IEnumerator"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Variabels).GetEnumerator();
    }
}