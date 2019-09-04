using System.Collections.Concurrent;
using System.Collections.Generic;
using MathCore.Annotations;

// ReSharper disable once CheckNamespace
namespace System.Linq.Reactive
{
    /// <summary>����� ��������-������ ����� ������������ � ������� ������������, ����������� ������� ����������� �� ����� ������������ � ������ ���� ������ ��������� �� ������</summary>
    /// <typeparam name="T">��� �������� ������������ �������</typeparam>
    internal sealed class ObserverLink<T> : IDisposable
    {
        /// <summary>�������� ���-��� �����</summary>
        /// <param name="Observers">��������� ������������</param>
        /// <param name="Observer">����������� �����������</param>
        /// <returns>���-��� �����</returns>
        private static int GetHash([NotNull] ICollection<IObserver<T>> Observers, [NotNull] IObserver<T> Observer) { unchecked { return Observer.GetHashCode() * 397 ^ Observers.GetHashCode(); } }

        /// <summary>������� ������</summary>
        [NotNull]
        private static readonly ConcurrentDictionary<int, ObserverLink<T>> __Links = new ConcurrentDictionary<int, ObserverLink<T>>();

        /// <summary>�������� ����� ����� ������������ � ������� ������������</summary>
        /// <param name="Observers">��������� ������������</param>
        /// <param name="Observer">����������� �����������</param>
        /// <returns>����� ����� ������������ � ������� ������������</returns>
        [NotNull]
        public static ObserverLink<T> GetLink([NotNull] ICollection<IObserver<T>> Observers, [NotNull] IObserver<T> Observer) => __Links.GetOrAdd(GetHash(Observers, Observer), h => new ObserverLink<T>(Observers, Observer));

        /// <summary>��������� �����������</summary>
        private IObserver<T> _Observer;
        /// <summary>��������� ������������, �� ������� ��������� ������� ������������� �����������</summary>
        private ICollection<IObserver<T>> _Observers;
        /// <summary>������ ������������ �������������</summary>
        [NotNull]
        private readonly object _SyncRoot = new object();

        /// <summary>������������� ����� ����� ����� ������� ������������ � ������������� ������������</summary>
        /// <param name="Observers">������ ������������</param>
        /// <param name="Observer">������������� �����������</param>
        private ObserverLink([NotNull] ICollection<IObserver<T>> Observers, [NotNull]IObserver<T> Observer)
        {
            _Observers = Observers;
            _Observer = Observer;
            if (!_Observers.Contains(_Observer))
                _Observers.Add(_Observer);
        }

        private bool _IsDisposed;
        void IDisposable.Dispose()
        {
            if (_IsDisposed) return;
            lock (_SyncRoot)
            {
                if (_IsDisposed) return;
                _IsDisposed = true;
                __Links.TryRemove(GetHash(_Observers, _Observer), out _);
                _Observers.Remove(_Observer);
                _Observers = null;
                _Observer = null;
            }
        }


    }
}