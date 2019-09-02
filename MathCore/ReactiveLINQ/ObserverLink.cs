using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MathCore.Annotations;

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
        private static readonly Dictionary<int, ObserverLink<T>> __Links = new Dictionary<int, ObserverLink<T>>();

        /// <summary>�������� ����� ����� ������������ � ������� ������������</summary>
        /// <param name="Observers">��������� ������������</param>
        /// <param name="Observer">����������� �����������</param>
        /// <returns>����� ����� ������������ � ������� ������������</returns>
        [NotNull]
        public static ObserverLink<T> GetLink([NotNull] ICollection<IObserver<T>> Observers, [NotNull] IObserver<T> Observer)
        {
            var hash = GetHash(Observers, Observer);
            lock(__Links)
                return __Links.TryGetValue(hash, out ObserverLink<T> link)
                    ? link
                    : (__Links[hash] = new ObserverLink<T>(Observers, Observer));
        }

        /// <summary>��������� �����������</summary>
        [NotNull]
        private IObserver<T> _Observer;
        /// <summary>��������� ������������, �� ������� ��������� ������� ������������� �����������</summary>
        [NotNull]
        private ICollection<IObserver<T>> _Observers;
        /// <summary>������ ������������ �������������</summary>
        [NotNull]
        private readonly object _SyncRoot = new object();

        /// <summary>������������� ����� ����� ����� ������� ������������ � ������������� ������������</summary>
        /// <param name="Observers">������ ������������</param>
        /// <param name="Observer">������������� �����������</param>
        private ObserverLink([NotNull] ICollection<IObserver<T>> Observers, [NotNull]IObserver<T> Observer)
        {
            Contract.Requires(Observer != null);
            Contract.Requires(Observers != null);
            Contract.Ensures(_Observer != null);
            Contract.Ensures(_Observers != null);
            Contract.Ensures(_Observers.Count > 0);
            Contract.Ensures(Contract.Exists(_Observers, o => Equals(o, _Observer)));
            _Observers = Observers;
            _Observer = Observer;
            if(!_Observers.Contains(_Observer))
                _Observers.Add(_Observer);
        }

        void IDisposable.Dispose()
        {
            Contract.Ensures(_Observer is null);
            Contract.Ensures(_Observers is null);
            Contract.Ensures(!Contract.Exists(_Observers, o => Equals(o, _Observer)));
            if(_Observer is null) return;
            lock(_SyncRoot)
            {
                if(_Observer is null) return;
                lock(__Links)
                {
                    __Links.Remove(GetHash(_Observers, _Observer));
                    _Observers.Remove(_Observer);
                }
                _Observer = null;
                _Observers = null;
            }
        }


    }
}