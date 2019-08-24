using System;

namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������ ���.���������, �������� ��������</summary>
    public abstract class ValueNode : ComputedNode
    {
        /// <summary>�������� ����</summary>
        public abstract double Value { get; set; }

        /// <summary>�������������� � ��������� �����</summary>
        /// <returns>��������� �������������</returns>
        public override string ToString()
        {
            const string format = "{1}{0}{2}";
            string Convert(ExpressionTreeNode n) => n?.ToString() ?? "";
            return string.Format(format, Value, Convert(Left), Convert(Right));
        }
    }
}