namespace System.Linq.Reactive
{
    /// <summary>��������� �����������</summary>
    /// <typeparam name="T">��� �������� ������������������ �������</typeparam>
    public interface IObserverEx<T> : IObserver<T>, IDisposable
    {
        /// <summary>������� ��������� ���������� ������� ������������������</summary>
        event Action<T> Next;

        /// <summary>������� ���������� ������������������</summary>
        event Action Complited;

        /// <summary>������� ������ ������������������</summary>
        event Action Reset;

        /// <summary>������� ��������� ����������</summary>
        event Action<Exception> Error;

        /// <summary>����� ��������� ������� ������ ������������������</summary>
        void OnReset();
    }
}