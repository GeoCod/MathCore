using System.Text;

namespace System.Linq.Expressions
{
    /// <summary>
    /// TeX supports several styles for multiplication sign
    /// </summary>
    public enum MultiplicationSign
    {
        /// <summary>
        /// Without any sign
        /// </summary>
        None,
        /// <summary>
        /// * sign
        /// </summary>
        Asterisk,
        /// <summary>
        /// x sign
        /// </summary>
        Times
    }

    /// <summary>
    /// ����� "������������", ������� "�������" ������ ��������� ����� ��������������� ���������������
    /// ����������� ������� �������� ������ System.Linq.Expressions.ExpressionVisitor
    /// </summary>
    public class TeXExpressionVisitor : ExpressionVisitor
    {
        //----------------------------------------------------------------------------------------//
        // �������� ����
        //----------------------------------------------------------------------------------------//
        // ���� �� ����� ��������� "����������" ����� ���������
        private readonly StringBuilder _Result = new StringBuilder();

        //----------------------------------------------------------------------------------------//
        // ������������
        //----------------------------------------------------------------------------------------//

        // ����������� ��������� ���������, ������� ����� ������������� � ������ TeX-�
        public TeXExpressionVisitor(Expression expression) => Visit(expression);

        // ������-��������� ������������� ��������� ��-�����, ��������� ��� ����� ������ ����
        // ���������, ��� ������� ���������
        public TeXExpressionVisitor(LambdaExpression expression) => Visit(expression.Body);


        //----------------------------------------------------------------------------------------//
        // �������� ���������
        //----------------------------------------------------------------------------------------//
        public string GenerateTeXExpression(string expressionName, MultiplicationSign multiplicationSign =
            MultiplicationSign.Asterisk)
        {
            // �������� ��������������� ������ � ����������� �� ���� ����� "���������"
            return GenerateTeXExpressionImpl(expressionName, multiplicationSign);
        }
        public string GenerateTeXExpression(MultiplicationSign multiplicationSign =
            MultiplicationSign.Asterisk) => GenerateTeXExpressionImpl(null, multiplicationSign);


        //----------------------------------------------------------------------------------------//
        // ���������������� ����������� ������ �������� ������ ExpressionVisitor
        //----------------------------------------------------------------------------------------//
        // ������, ���������������� �� ������ ExpressionVisitor
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if(node.NodeType == ExpressionType.Negate)
                _Result.Append("-");
            return base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node) => IsInfix(node.NodeType) ? VisitInfixBinary(node) : VisitPrefixBinary(node);

        protected override Expression VisitMember(MemberExpression node)
        {
            var name = node.Member.Name.Split('.').Last();
            _Result.Append(name);
            return node;
        }

        public override Expression Visit(Expression node)
        {
            if(node is ConstantExpression constant)
            {
                _Result.Append(constant.Value);
                return Expression.Constant(constant.Value, constant.Type);
            }
            return base.Visit(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _Result.Append(node.Name.Split('.').Last());
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            // ��� ����� ������� ���������� ������ ������������ �������.
            // ��� ������������� ���� �� ����� �������� ����������� ��� ���-��
            // ��������, ����� �� ����� ���������� ����������
            var pow_method = typeof(Math).GetMethod("Pow");

            if(node.Method == pow_method)
            {
                Visit(node.Arguments[0]);
                _Result.AppendFormat("^{{{0}}}", node.Arguments[1]);
                return node;
            }
            return base.Visit(node);
        }

        //----------------------------------------------------------------------------------------//
        // ��������������� �������� ������
        //----------------------------------------------------------------------------------------//

        // ������ ���������� true, ���� ��������� ����� ����������� � ������: ()
        private static bool RequiresPrecedence(ExpressionType nodeType) =>
            nodeType switch
            {
                ExpressionType.Add => true,
                ExpressionType.Subtract => true,
                _ => false
            };

        // �������� ������� ��������� ���������� �� ���� ��������� ���������� � ����� �����������,
        // ��������� �� ������� ������� ������� ����������
        private static bool IsInfix(ExpressionType nodeType) => nodeType != ExpressionType.Divide;

        // ����������� ���������� ������� ��������� � ��������� �������:
        // {arg1} op {arg2}
        private Expression VisitInfixBinary(BinaryExpression node)
        {
            var requires_precedence = RequiresPrecedence(node.NodeType);
            if(requires_precedence) _Result.Append("(");

            Visit(node.Left);

            switch(node.NodeType)
            {
                case ExpressionType.Multiply:
                    _Result.Append("*");
                    break;
                case ExpressionType.Add:
                    _Result.Append("+");
                    break;
                case ExpressionType.Subtract:
                    _Result.Append("-");
                    break;
                default:
                    throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
            }

            Visit(node.Right);

            if(requires_precedence) _Result.Append(")");
            return node;
        }


        /// <summary>
        /// �������� ������� \fract ������� ����� ������� ����������:
        /// \frac{arg1}{arg2} 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Expression VisitPrefixBinary(BinaryExpression node)
        {
            // ��� ������� (x + 2) �� 3, �� ������ �������� ��������� ���������
            // \frac{x + 2}{3}
            switch(node.NodeType)
            {
                case ExpressionType.Divide:
                    _Result.Append(@"\frac");
                    break;
                default:
                    throw new InvalidOperationException($"Unknown prefix BinaryExpression {node.Type}");
            }

            _Result.Append("{");
            Visit(node.Left);
            _Result.Append("}");

            _Result.Append("{");
            Visit(node.Right);
            _Result.Append("}");
            return node;
        }

        // �����, ����������� ��������� ���������� ������������� ����������� ���������
        private string GenerateTeXExpressionImpl(string expressionName, MultiplicationSign multiplicationSign)
        {
            switch(multiplicationSign)
            {
                case MultiplicationSign.Times:
                    _Result.Replace("*", @" \times ");
                    break;
                case MultiplicationSign.None:
                    _Result.Replace("*", "");
                    break;
            }

            var texExpression = _Result.ToString();

            // ���������� ��������� �������� ���������� ������� ������, ������� ������� ������
            if(texExpression.Length > 0)
                if(texExpression[0] == '(' && texExpression[texExpression.Length - 1] == ')')
                    texExpression = texExpression.Substring(1, texExpression.Length - 2);

            // ��������� "��� ���������" ���� ��� �������
            return !string.IsNullOrEmpty(expressionName)
                ? $"{{{expressionName}}}{{=}}{texExpression}"
                : texExpression;
        }
    }
}