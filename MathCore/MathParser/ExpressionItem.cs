using System.ComponentModel;
using System.Runtime.CompilerServices;
using MathCore.Annotations;

namespace MathCore.MathParser
{
    /// <summary>������� ��������������� ���������</summary>
    public abstract class ExpressionItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string PeoprtyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PeoprtyName));

        [NotifyPropertyChangedInvocator]
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        private string _Name;

        /// <summary>���</summary>
        public string Name { get => _Name; set => Set(ref _Name, value); }

        /// <summary>������������� ������ �������� ��������������� ���������</summary>
        protected ExpressionItem() { }

        /// <summary>������������� ������ �������� ��������������� ���������</summary><param name="Name">��� ��������</param>
        protected ExpressionItem(string Name) => this.Name = Name;

        /// <summary>����� ����������� ��������</summary><returns>��������� �������� �������� ���������</returns>
        public abstract double GetValue();
    }
}