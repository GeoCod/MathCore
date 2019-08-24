namespace MathCore.Trees
{
    /// <summary>������� ����������� ������</summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeItem<T> where T : class, ITreeItem<T>
    {
        /// <summary>������������ ����</summary>
        T Parent { get; set; }

        /// <summary>�������� ����</summary>
        T Child { get; set; }

        /// <summary>���������� ���� ������</summary>
        T Prev { get; set; }

        /// <summary>��������� ���� ������</summary>
        T Next { get; set; }
    }
}