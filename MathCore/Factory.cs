using System;
using System.ComponentModel;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedMember.Global

namespace MathCore
{
    public interface IFactory<out T>
    {
        /// <summary>������� ����� ������</summary>
        /// <returns>����� ������ ���� <typeparamref name="T"/></returns>
        [DST] T Create();
    }

    /// <summary>��������� �������� ���� <typeparamref name="T"/></summary>
    /// <typeparam name="T">��� ������������ ��������</typeparam>
    public class Factory<T> : INotifyPropertyChanged, IFactory<T>
    {
        /* ------------------------------------------------------------------------------------------ */

        private event PropertyChangedEventHandler _PropertyChanged;

        /// <summary>������� ��������� ��� ��������� ����� ������</summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            [DST] add => _PropertyChanged += value;
            [DST] remove => _PropertyChanged -= value;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => _PropertyChanged?.Invoke(this, e);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>����� ��������� ��������</summary>
        private Func<T> _FactoryMethod;

        private T _Last;
        private readonly PropertyChangedEventArgs _PropertyLastChengedArgs = new PropertyChangedEventArgs(nameof(Last));

        protected bool _RaiseLastChangedEvents = true;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>��������� ��������������� ������</summary>
        public T Last
        {
            [DST]
            get => _Last;
            private set
            {
                _Last = value;
                if(_RaiseLastChangedEvents)
                    OnPropertyChanged(_PropertyLastChengedArgs);
            }
        }

        /// <summary>����� ��������� �������� ���� <typeparamref name="T"/></summary>
        public Func<T> FactoryMethod
        {
            [DST]
            get => _FactoryMethod;
            [DST]
            set => _FactoryMethod = value;
        }

        /* ------------------------------------------------------------------------------------------ */

        protected Factory() { }

        /// <summary>����� ��������� �������� ���� <typeparamref name="T"/></summary>
        /// <param name="CreateMethod">����� ��������� �������� ���� <typeparamref name="T"/></param>
        [DST]
        public Factory(Func<T> CreateMethod) => _FactoryMethod = CreateMethod;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������� ����� ������</summary>
        /// <returns>����� ������ ���� <typeparamref name="T"/></returns>
        [DST]
        public virtual T Create() => _FactoryMethod == null ? default : Last = _FactoryMethod();

        /* ------------------------------------------------------------------------------------------ */

        [DST]
        public override int GetHashCode() => typeof(T).GetHashCode() ^ _FactoryMethod.GetHashCode();

        /* ------------------------------------------------------------------------------------------ */
    }
}