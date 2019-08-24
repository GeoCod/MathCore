namespace MathCore.Graphs
{
    /// <summary>����� ����</summary>
    /// <typeparam name="TValue">��� �������� ����</typeparam>
    /// <typeparam name="TWeight">��� �������� �� �����</typeparam>
    public interface IGraphLink<out TValue, out TWeight>
    {
        /// <summary>��������� ����</summary>
        IGraphNode<TValue, TWeight> Node { get; }
        /// <summary>�������� �� �����</summary>
        TWeight Weight { get; }
    }
}