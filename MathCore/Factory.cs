using System;
using System.ComponentModel;
using System.Diagnostics;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedMember.Global

namespace MathCore
{
    public interface IFactory<out T>
    {
        /// <summary>������� ����� ������</summary>
        /// <returns>����� ������ ���� <typeparamref name="T"/></returns>
        [DebuggerStepThrough]
        T Create();
    }

    /// <summary>��������� �������� ���� <typeparamref name="T"/></summary>
    /// <typeparam name="T">��� ������������ ��������</typeparam>
    public class Factory<T> : INotifyPropertyChanged, IFactory<T>
    {
        /* ------------------------------------------------------------------------------------------ */

        private event PropertyChangedEventHandler e_PropertyChanged;

        /// <summary>������� ��������� ��� ��������� ����� ������</summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            [DebuggerStepThrough]
            add => e_PropertyChanged += value;
            [DebuggerStepThrough]
            remove => e_PropertyChanged -= value;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => e_PropertyChanged?.Invoke(this, e);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>����� ��������� ��������</summary>
        private Func<T> _FactoryMethod;

        private T _Last;
        private readonly PropertyChangedEventArgs c_PropertyLastChengedArgs = new PropertyChangedEventArgs(nameof(Last));

        protected bool _RaiseLastChangedEvents = true;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>��������� ��������������� ������</summary>
        public T Last
        {
            [DebuggerStepThrough]
            get => _Last;
            private set
            {
                _Last = value;
                if(_RaiseLastChangedEvents)
                    OnPropertyChanged(c_PropertyLastChengedArgs);
            }
        }

        /// <summary>����� ��������� �������� ���� <typeparamref name="T"/></summary>
        public Func<T> FactoryMethod
        {
            [DebuggerStepThrough]
            get => _FactoryMethod;
            [DebuggerStepThrough]
            set => _FactoryMethod = value;
        }

        /* ------------------------------------------------------------------------------------------ */

        protected Factory() { }

        /// <summary>����� ��������� �������� ���� <typeparamref name="T"/></summary>
        /// <param name="CreateMethod">����� ��������� �������� ���� <typeparamref name="T"/></param>
        [DebuggerStepThrough]
        public Factory(Func<T> CreateMethod) => _FactoryMethod = CreateMethod;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������� ����� ������</summary>
        /// <returns>����� ������ ���� <typeparamref name="T"/></returns>
        [DebuggerStepThrough]
        public virtual T Create() => _FactoryMethod == null ? default : Last = _FactoryMethod();

        /* ------------------------------------------------------------------------------------------ */

        [DebuggerStepThrough]
        public override int GetHashCode() => typeof(T).GetHashCode() ^ _FactoryMethod.GetHashCode();

        /* ------------------------------------------------------------------------------------------ */
    }
}