using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>��������� ������� ����������</summary>
    /// <typeparam name="TException">��� ����������</typeparam>
    public class ExceptionEventHandlerArgs<TException> : EventArgs<TException> where TException : Exception
    {
        /* ------------------------------------------------------------------------------------------ */

        /// <summary>���� ������������� ��������� ����������</summary>
        private bool _Unhandled;

        /// <summary>���� �������� ��������� ���������� �������������</summary>
        private bool _IsHandled;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>���������� ����������</summary>
        public bool IsHandled { [DST] get => !_Unhandled && _IsHandled; [DST] set => _IsHandled = value; }

        /// <summary>������� ������������� ��������� ����������</summary>
        public bool NeedToThrow => _Unhandled || !IsHandled;

        /* ------------------------------------------------------------------------------------------ */


        /// <summary>����� �������� ������� ��������� ����������</summary>
        /// <param name="Error">����������</param>
        [DST]
        public ExceptionEventHandlerArgs(TException Error) : base(Error) { }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>���������� ����������</summary>
        [DST]
        public void Handled() => IsHandled = true;

        /// <summary>���������� ������ ���� ������������� � ����� ������</summary>
        [DST]
        public void Unhandled() => _Unhandled = true;

        /* ------------------------------------------------------------------------------------------ */

        [DST]
        public static implicit operator TException(ExceptionEventHandlerArgs<TException> arg) => arg.Argument;

        [DST]
        public static implicit operator ExceptionEventHandlerArgs<TException>(TException exception) => new ExceptionEventHandlerArgs<TException>(exception);

        /* ------------------------------------------------------------------------------------------ */
    }
}