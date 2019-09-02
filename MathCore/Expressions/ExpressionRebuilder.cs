using System.Collections.Generic;
using System.Collections.ObjectModel;

// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions
{
    /// <summary>����������� ������ ��������� Linq.Expression</summary>
    //[Diagnostics.DST]
    public class ExpressionRebuilder : ExpressionVisitorEx
    {
        /// <summary>����� ��������� ������� ��������� ���� ���� Expression</summary>
        /// <typeparam name="TExpressionNode">��� ���� ������</typeparam>
        /// <param name="Handlers">�������</param>
        /// <param name="Node">���� ������</param>
        /// <param name="Base">������� ����� ��������� ����</param>
        /// <returns>����, ������� ���� ��������� ���������� ���� ������</returns>
        private Expression InvokeEvent<TExpressionNode>(EventHandlerReturn<EventArgs<TExpressionNode>, Expression> Handlers, TExpressionNode Node, Func<TExpressionNode, Expression> Base)
            where TExpressionNode : Expression
        {
            // ���� ������������ ������� ���, �� �������� ������� ����� � ���������� ���������
            var element = Base(Node); // �������� ������� ����� ��� ��������� ������
            if(Handlers is null) return element;
            var node = element as TExpressionNode;
            return node != null
                ? Handlers(this, new EventArgs<TExpressionNode>(node))
                : element; // ����� ���������� �������, �� �������� ������
        }

        /// <summary>����� ��������� ������� ��������� ���� ������������� ����</summary>
        /// <typeparam name="TElement">��� ���� ������</typeparam><typeparam name="TOut">��� ��������� ����</typeparam>
        /// <param name="Handlers">�������</param>
        /// <param name="Node">���������� ���� ������</param>
        /// <param name="Base">������� ����� ��������� ����</param>
        /// <returns>����, ������� ���� ��������� ���������� ���� ������</returns>
        private TOut InvokeEvent<TElement, TOut>(EventHandlerReturn<EventArgs<TOut>, TOut> Handlers, TElement Node, Func<TElement, TOut> Base)
        {
            // ���� ������������ ������� ���, �� �������� ������� ����� � ���������� ���������
            var element = Base(Node); // �������� ������� ����� ��� ��������� ������
            if(Handlers is null) return element;
            // ���������� ������� � ��������� � ���� ����, ����������� �� �������� ������
            return Handlers(this, new EventArgs<TOut>(element));
        }

        /// <summary>������� ��������� ��� ��������� ������ ���� ������</summary>
        public event EventHandlerReturn<EventArgs<Expression>, Expression> Visited;

        /// <summary>�������� ���� ������</summary><param name="Node">���� ������</param><returns>����� ���� ������</returns>
        public override Expression Visit(Expression Node) => InvokeEvent(Visited, Node, base.Visit);

        /// <summary>������� ��������� ��� ��������� ���� ������ ��������� ���������</summary>
        public event EventHandlerReturn<EventArgs<BinaryExpression>, Expression> BinaryVisited;
        protected override Expression VisitBinary(BinaryExpression b) => InvokeEvent(BinaryVisited, b, base.VisitBinary);

        /// <summary>������� ��������� ��� ��������� ���� ��������</summary>
        public event EventHandlerReturn<EventArgs<MemberBinding>, MemberBinding> BindingVisited;
        protected override MemberBinding VisitBinding(MemberBinding binding) => InvokeEvent(BindingVisited, binding, base.VisitBinding);

        /// <summary>������� ��������� ��� ��������� ��������� ��������</summary>
        public event EventHandlerReturn<EventArgs<IEnumerable<MemberBinding>>, IEnumerable<MemberBinding>> BindingListVisited;
        protected override IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original) => InvokeEvent(BindingListVisited, original, base.VisitBindingList);

        /// <summary>������� ��������� ��� ��������� ���� ��������� ���������</summary>
        public event EventHandlerReturn<EventArgs<ConditionalExpression>, Expression> ConditionalVisited;
        protected override Expression VisitConditional(ConditionalExpression c) => InvokeEvent(ConditionalVisited, c, base.VisitConditional);

        /// <summary>������� ��������� ��� ��������� ���� ���������</summary>
        public event EventHandlerReturn<EventArgs<ConstantExpression>, Expression> ConstantlVisited;
        protected override Expression VisitConstant(ConstantExpression c) => InvokeEvent(ConstantlVisited, c, base.VisitConstant);

        /// <summary>������� ��������� ��� ��������� ���� �������������� �������</summary>
        public event EventHandlerReturn<EventArgs<ElementInit>, ElementInit> ElementInitializerVisited;
        protected override ElementInit VisitElementInitializer(ElementInit initializer) => InvokeEvent(ElementInitializerVisited, initializer, base.VisitElementInitializer);

        /// <summary>������� ��������� ��� ��������� ��������� ��������������� �������</summary>
        public event EventHandlerReturn<EventArgs<IEnumerable<ElementInit>>, IEnumerable<ElementInit>> ElementInitializerListVisited;
        protected override IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original) => InvokeEvent(ElementInitializerListVisited, original, base.VisitElementInitializerList);

        /// <summary>������� ��������� ��� ��������� ��������� ���������</summary>
        public event EventHandlerReturn<EventArgs<ReadOnlyCollection<Expression>>, ReadOnlyCollection<Expression>> ExpressionListVisited;
        protected override ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original) => InvokeEvent(ExpressionListVisited, original, base.VisitExpressionList);

        /// <summary>������� ��������� ��� ��������� �����-���������</summary>
        public event EventHandlerReturn<EventArgs<LambdaExpression>, Expression> LambdaVisited;
        protected override Expression VisitLambda(LambdaExpression lambda) => InvokeEvent(LambdaVisited, lambda, base.VisitLambda);

        /// <summary>������� ��������� ��� ��������� ���� �������������� ���������</summary>
        public event EventHandlerReturn<EventArgs<ListInitExpression>, Expression> ListInitVisited;
        protected override Expression VisitListInit(ListInitExpression init) => InvokeEvent(ListInitVisited, init, base.VisitListInit);

        /// <summary>������� ��������� ��� ��������� ���� ������ �������</summary>
        public event EventHandlerReturn<EventArgs<InvocationExpression>, Expression> InvocationVisited;
        protected override Expression VisitInvocation(InvocationExpression iv) => InvokeEvent(InvocationVisited, iv, base.VisitInvocation);

        /// <summary>������� ��������� ��� ��������� ���� ������� � ����� �������</summary>
        public event EventHandlerReturn<EventArgs<MemberExpression>, Expression> MemberAccessVisited;
        protected override Expression VisitMemberAccess(MemberExpression m) => InvokeEvent(MemberAccessVisited, m, base.VisitMemberAccess);

        /// <summary>������� ��������� ��� ��������� ���� ���������� ����� ������� ��������</summary>
        public event EventHandlerReturn<EventArgs<MemberAssignment>, MemberAssignment> MemberAssignmentVisited;
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment) => InvokeEvent(MemberAssignmentVisited, assignment, base.VisitMemberAssignment);

        /// <summary>������� ��������� ��� ��������� ���� �������������� �������� �������</summary>
        public event EventHandlerReturn<EventArgs<MemberInitExpression>, Expression> MemberInitVisited;
        protected override Expression VisitMemberInit(MemberInitExpression init) => InvokeEvent(MemberInitVisited, init, base.VisitMemberInit);

        /// <summary>������� ��������� ��� ��������� ���� �������������� ��������� ��������</summary>
        public event EventHandlerReturn<EventArgs<MemberListBinding>, MemberListBinding> MemberListBindingVisited;
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding) => InvokeEvent(MemberListBindingVisited, binding, base.VisitMemberListBinding);

        /// <summary>������� ��������� ��� ��������� ���� �������������� ��������� ��������</summary>
        public event EventHandlerReturn<EventArgs<MemberMemberBinding>, MemberMemberBinding> MemberMemberBindingVisited;
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding) => InvokeEvent(MemberMemberBindingVisited, binding, base.VisitMemberMemberBinding);

        /// <summary>������� ��������� ��� ��������� ���� ������ ������</summary>
        public event EventHandlerReturn<EventArgs<MethodCallExpression>, Expression> MethodCallVisited;
        protected override Expression VisitMethodCall(MethodCallExpression m) => InvokeEvent(MethodCallVisited, m, base.VisitMethodCall);

        /// <summary>������� ��������� ��� ��������� ���� ������������</summary>
        public event EventHandlerReturn<EventArgs<NewExpression>, NewExpression> NewVisited;
        protected override NewExpression VisitNew(NewExpression nex) => InvokeEvent(NewVisited, nex, base.VisitNew);

        /// <summary>������� ��������� ��� ��������� ���� ������������ �������</summary>
        public event EventHandlerReturn<EventArgs<NewArrayExpression>, Expression> NewArrayVisited;
        protected override Expression VisitNewArray(NewArrayExpression na) => InvokeEvent(NewArrayVisited, na, base.VisitNewArray);

        /// <summary>������� ��������� ��� ��������� ���� ��������� ���������</summary>
        public event EventHandlerReturn<EventArgs<ParameterExpression>, Expression> ParameterVisited;
        protected override Expression VisitParameter(ParameterExpression p) => InvokeEvent(ParameterVisited, p, base.VisitParameter);

        /// <summary>������� ��������� ��� ��������� ���� ����������� ���� ���������</summary>
        public event EventHandlerReturn<EventArgs<TypeBinaryExpression>, Expression> TypeIsVisited;
        protected override Expression VisitTypeIs(TypeBinaryExpression b) => InvokeEvent(TypeIsVisited, b, base.VisitTypeIs);

        /// <summary>������� ��������� ��� ��������� ���� �������� ���������</summary>
        public event EventHandlerReturn<EventArgs<UnaryExpression>, Expression> UnaryVisited;
        protected override Expression VisitUnary(UnaryExpression u) => InvokeEvent(UnaryVisited, u, base.VisitUnary);
    }
}