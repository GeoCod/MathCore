namespace System.Linq.Reactive
{
    /// <summary>����������� ����������� ������</summary>
    /// <typeparam name="T">��� ������� ������������������</typeparam>
    sealed class TriggeredObservable<T> : SimpleObservableEx<T>
    {
        /// <summary>�����������</summary>
        private readonly IObserver<T> _Observer;

        /// <summary>������� ���������� ��������� �������</summary>
        public bool Open { get; set; }

        /// <summary>����������� ����������� ������</summary>
        /// <param name="observable">����������� ������</param>
        /// <param name="IsOpen">�������� ���������</param>
        public TriggeredObservable(IObservable<T> observable, bool IsOpen = true)
        {
            _Observer = new LinkedObserver<T>(observable, this);
            Open = IsOpen;
        }

        public override void OnNext(T item) { if(Open) base.OnNext(item); }

        public override void OnCompleted() { if(Open) base.OnCompleted(); }

        public override void OnReset() { if(Open) base.OnReset(); }

        public override void OnError(Exception error) { if(Open) base.OnError(error); }

        public override void Dispose()
        {
            base.Dispose();
            (_Observer as IDisposable)?.Dispose();
        }
    }
}