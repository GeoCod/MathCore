using System;
using System.Linq;

namespace MathCore.CommandProcessor
{
    /// <summary>�������</summary>
    public struct Command
    {
        /// <summary>��� �������</summary>
        public string Name { get; set; }
        /// <summary>�������� �������</summary>
        public string Parameter { get; set; }
        /// <summary>������ ���������� �������</summary>
        public Argument[] Argument { get; set; }

        /// <summary>�������</summary>
        /// <param name="CommandStr">��������� ������������� �������</param>
        /// <param name="ParameterSplitter">����������� ����� � ��������� �������</param>
        /// <param name="ArgSplitter">����������� ���������� �������</param>
        /// <param name="ValueSplitter">����������� ����� ��������� � ��� ��������</param>
        public Command(string CommandStr, char ParameterSplitter = ':', char ArgSplitter = ' ', char ValueSplitter = '=')
            : this()
        {
            var items = CommandStr.Split(ArgSplitter);
            var nameitems = items[0].Split(ParameterSplitter);
            Name = nameitems[0];
            if(nameitems.Length > 1)
                Parameter = nameitems[1];

            Argument = items.Skip(1).Where(ArgStr => !string.IsNullOrEmpty(ArgStr))
                        .Select(ArgStr => new Argument(ArgStr, ValueSplitter))
                        .Where(arg => !string.IsNullOrEmpty(arg.Name))
                        .ToArray();
        }

        /// <summary>�������������� � ������</summary>
        /// <returns>��������� ������������� �������</returns>
        public override string ToString() => $"{Name}{(Parameter == null ? "" : Parameter.ToFormattedString("({0})"))}{(Argument == null || Argument.Length == 0 ? "" : Argument.ToSeparatedStr(" ").ToFormattedString(" {0}"))}";
    }
}