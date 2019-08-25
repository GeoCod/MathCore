using System;

namespace MathCore.CommandProcessor
{
    /// <summary>�������� ������� ��������� �������</summary>
    public class CommandEventArgs : EventArgs
    {
        /// <summary>�������������� �������</summary>
        public Command Command { get; set; }
        /// <summary>�������� ������ ������</summary>
        public Command[] Commands { get; set; }
        /// <summary>������ ������� � ������� ������ ������</summary>
        public int Index { get; set; }
        /// <summary>������� ����, ��� ������� ����������</summary>
        public bool Handled { get; set; }

        /// <summary>��������� �������������</summary>
        /// <returns>��������� �������������</returns>
        public override string ToString() => string.Format("Command({0}/{3}):> {1}{2}", Index + 1, Command, Handled ? "- processed" : "", Commands.Length);

        public void SetHandled() => Handled = true;
    }
}