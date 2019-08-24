namespace MathCore.MathParser.ExpressionTrees.Nodes
{
    /// <summary>���� ������������� ��������</summary>
    public class IntervalNode : ParsedNode
    {
        /// <summary>����������� ��������</summary>
        public ExpressionTreeNode Min { get { return Left; } set { Left = value; } }

        /// <summary>������������ ��������</summary>
        public ExpressionTreeNode Max { get { return Right; } set { Right = value; } }

        public IntervalNode(double Min, double Max) : this(new ConstValueNode(Min), new ConstValueNode(Max)) { }

        public IntervalNode(ExpressionTreeNode Min, ExpressionTreeNode Max = null) { Left = Min; Right = Max; }

        /// <summary>������������ ���������</summary>
        /// <returns>���� ���������</returns>
        public override ExpressionTreeNode Clone() => new IntervalNode(Min, Max);

        /// <summary>�������������� ���� � ������</summary>
        /// <returns>��������� ������������� ����</returns>
        public override string ToString() => $"{Left}..{Right}";
    }
}