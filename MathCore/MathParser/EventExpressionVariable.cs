using System;

namespace MathCore.MathParser
{
    /// <summary>���������� ����������</summary>
    /// <remarks>���������� ��������������� ���������, �������� ������� ������������ ����� ��������� �������</remarks>
    public class EventExpressionVariable : ExpressionVariabel
    {
        /// <summary>������� ������� �������� ����������</summary>
        public event EventHandler<EventArgs<double>> Call;

        /// <summary>����� ��������� �������</summary>
        /// <param name="Args">�������� �������</param>
        protected virtual void OnCall(EventArgs<double> Args) => Call?.Invoke(this, Args);

        /// <summary>�������� �������</summary>
        private readonly EventArgs<double> _EventArg = new EventArgs<double>(0);

        /// <summary>���� ��������������� ������� �������� ��������� �������</summary>
        private bool _ClearAtCall;

        /// <summary>�������� ����������</summary>
        public override double Value { get => _EventArg.Argument; set => _EventArg.Argument = value; }

        /// <summary>������� ���������������� ������ = false</summary>
        public override bool IsPrecomputable => false;

        /// <summary>���� ��������������� ������� �������� ��������� �������</summary>
        public bool ClearAtCall { get => _ClearAtCall; set => _ClearAtCall = value; }

        /// <summary>������������� ����� ���������� ����������</summary>
        public EventExpressionVariable() : this("") { }

        /// <summary>������������� ����� ���������� ����������</summary>
        /// <param name="Name">��� ����������</param>
        public EventExpressionVariable(string Name) : base(Name) { }

        /// <summary>��������� �������� ����������</summary>
        /// <returns>�������� ���������</returns>
        public override double GetValue()
        {
            if(_ClearAtCall)
                _EventArg.Argument = 0;
            OnCall(_EventArg);
            return base.Value = _EventArg.Argument;
        }

        /// <summary>����� ������������ ���������� ����������</summary>
        /// <returns>������������� ���������� ����������</returns>
        public override ExpressionVariabel Clone() => new EventExpressionVariable(Name) { ClearAtCall = ClearAtCall, Value = Value };
    }
}