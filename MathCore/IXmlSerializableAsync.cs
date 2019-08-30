using System.Threading.Tasks;

namespace System.Xml.Serialization
{
    /// <summary>���������� ������������� � XML ������</summary>
    public interface IXmlSerializableAsync : IXmlSerializable
    {
        /// <summary>����������� ������ ������ �� XML</summary>
        /// <param name="reader">�������� ������ XML</param>
        /// <returns>������ �������� ������ ������</returns>
        Task ReadXmlAsync(XmlReader reader);

        /// <summary>����������� ������ ������ � XML</summary>
        /// <param name="writer">������ ������ ������</param>
        /// <returns>������ ������ ������</returns>
        Task WriteXmlAsync(XmlWriter writer);
    } 
}