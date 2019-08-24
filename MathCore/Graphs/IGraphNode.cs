using System.Collections.Generic;

namespace MathCore.Graphs
{
    /// <summary>���� �����</summary>
    /// <typeparam name="TValue">��� �������� ����</typeparam>
    /// <typeparam name="TWeight">��� �������� �� �����</typeparam>
    public interface IGraphNode<out TValue, out TWeight> : IEnumerable<IGraphNode<TValue, TWeight>>
    {
        /// <summary>����� ����</summary>
        IEnumerable<IGraphLink<TValue, TWeight>> Links { get; }

        /// <summary>�������� ����</summary>
        TValue Value { get; }
    }

    public interface IGraphNode<out TValue> : IEnumerable<IGraphNode<TValue>>
    {
        /// <summary>����� ����</summary>
        IEnumerable<IGraphNode<TValue>> Childs { get; }

        /// <summary>�������� ����</summary>
        TValue Value { get; }
    }
}