using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// ����� "����������" ��� "�����������" ���������� ������� � ������ ���������
    /// </summary>
    public class TeXEvaluationExpressionVisitor : ExpressionVisitor
    {
        // ��������������� ����� ��� �������� �������� � ���� �������
        private sealed class TypeValuePair
        {
            public object Value { get; set; }
            public Type Type { get; set; }
        }

        // ���� ��� �������� �������� � ���� ������� �� ����� ��������
        private readonly Dictionary<string, TypeValuePair> _MemberProperties;

        // ����������� ��������� ��������� � ������, �������� ������� �������� ����� �����������
        // � �������� ���������
        public TeXEvaluationExpressionVisitor(Expression expression, object memberObject)
        {
            // ������� ��� �������� ����������� �������
            var memberProps = memberObject.GetType().GetProperties();

            // � ������� ������������� ������ ���� �������� � �������� �� ����� ��������
            _MemberProperties = memberProps.ToDictionary(pi => pi.Name,
                pi => new TypeValuePair
                {
                    Value = pi.GetValue(memberObject, null),
                    Type = pi.PropertyType
                });

            ConvertedExpression = Visit(expression);
        }

        // "�����������" ��������� � "��������������" ���������� �������
        public Expression ConvertedExpression { get; private set; }

        // �������� ��������� � ����� �� ��������������� ��������
        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            // ������� ����� �������� ����� � ��������� ������
            TypeValuePair typeValuePair;
            if(_MemberProperties.TryGetValue(memberExpression.Member.Name, out typeValuePair))
            {
                // � �������� ��� �� ��������������� ����������� ���������
                return Expression.Constant(value: typeValuePair.Value, type: typeValuePair.Type);
            }
            return memberExpression;
        }
    }
}