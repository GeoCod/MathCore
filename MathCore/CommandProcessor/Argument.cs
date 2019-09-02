using System;
using System.Linq;

namespace MathCore.CommandProcessor
{
    /// <summary>�������� �������</summary>
    public struct Argument
    {
        /// <summary>��� ���������</summary>
        public string Name { get; set; }
        /// <summary>������ �������� ���������</summary>
        public string[] Values { get; set; }
        /// <summary>�������� ���������</summary>
        public string Value => Values?.Length > 0 ? Values[0] : string.Empty;

        /// <summary>���������� �������� ���������</summary>
        public int Count => Values?.Length ?? 0;

        /// <summary>������ � ��������� ��������� �� ������</summary>
        /// <param name="i">����� ��������</param>
        /// <returns>�������� ��������� � ��������� �������</returns>
        public string this[int i] => Values[i];

        /// <summary>�������� �������</summary>
        /// <param name="ArgStr">��������� �������� ���������</param>
        /// <param name="ValueSplitter">����������� ����� ��������� � ��������</param>
        public Argument(string ArgStr, char ValueSplitter = '=')
            : this()
        {
            var ArgItems = ArgStr.Split(ValueSplitter);
            Name = ArgItems[0].ClearSystemSymbolsAtBeginAndEnd();
            Values = ArgItems.Skip(1)
                        .Select(value => value.ClearSystemSymbolsAtBeginAndEnd())
                        .Where(value => !string.IsNullOrEmpty(value))
                        .ToArray();
        }

        /// <summary>�������������� � ������</summary>
        /// <returns>��������� ������������� ���������</returns>
        public override string ToString() => $"{Name}{(Values is null || Values.Length == 0 ? "" : Values.ToSeparatedStr(", ").ToFormattedString("={0}"))}";
    }
}