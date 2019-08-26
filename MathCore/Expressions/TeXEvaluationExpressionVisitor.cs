using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>����� "����������" ��� "�����������" ���������� ������� � ������ ���������</summary>
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
            var member_props = memberObject.GetType().GetProperties();

            // � ������� ������������� ������ ���� �������� � �������� �� ����� ��������
            _MemberProperties = member_props.ToDictionary(pi => pi.Name,
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
            if(_MemberProperties.TryGetValue(memberExpression.Member.Name, out var type_value_pair))
                // � �������� ��� �� ��������������� ����������� ���������
                return Expression.Constant(value: type_value_pair.Value, type: type_value_pair.Type);
            return memberExpression;
        }
    }
}