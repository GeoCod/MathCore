namespace System.Linq.Reactive
{
    /// <summary>����������� ������ � ��������� ������ ��������� �������</summary>
    /// <typeparam name="T">��� �������� ������������������</typeparam>
    sealed class TakeObservable<T> : SimpleObservableEx<T>
    {
        /// <summary>�������� �����������</summary>
        private readonly IObserver<T> _Observer;

        /// <summary>����������� ������ � ��������� ������ ��������� �������</summary>
        /// <param name="observable">�������� ����������� ������</param>
        /// <param name="Count">���������� ����������� �������</param>
        public TakeObservable(IObservable<T> observable, int Count)
        {
            _Observer = new TakeObserver<T>(observable, Count);
        }
    }
}