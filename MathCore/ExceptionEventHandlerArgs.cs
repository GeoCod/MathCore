using System.Diagnostics;
using System.Diagnostics.Contracts;

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
        public bool IsHandled
        {
            [DebuggerStepThrough]
            get { return !_Unhandled && _IsHandled; }
            [DebuggerStepThrough]
            set { _IsHandled = value; }
        }

        /// <summary>������� ������������� ��������� ����������</summary>
        public bool NeedToThrow
        {
            [DebuggerStepThrough]
            get { return _Unhandled || !IsHandled; }
        }

        /* ------------------------------------------------------------------------------------------ */


        /// <summary>����� �������� ������� ��������� ����������</summary>
        /// <param name="Error">����������</param>
        [DebuggerStepThrough]
        public ExceptionEventHandlerArgs(TException Error) : base(Error) => Contract.Requires(Error != null);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>���������� ����������</summary>
        [DebuggerStepThrough]
        public void Handled() { IsHandled = true; }

        /// <summary>���������� ������ ���� ������������� � ����� ������</summary>
        [DebuggerStepThrough]
        public void Unhandled() { _Unhandled = true; }

        /* ------------------------------------------------------------------------------------------ */

        [DebuggerStepThrough]
        public static implicit operator TException(ExceptionEventHandlerArgs<TException> arg) { return arg.Argument; }

        [DebuggerStepThrough]
        public static implicit operator ExceptionEventHandlerArgs<TException>(TException exception)
        {
            return new ExceptionEventHandlerArgs<TException>(exception);
        }

        /* ------------------------------------------------------------------------------------------ */
    }
}