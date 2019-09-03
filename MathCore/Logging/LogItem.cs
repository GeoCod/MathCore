﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MathCore.Annotations;

namespace MathCore.Logging
{
    public sealed class LogItem : IEnumerable<LogItem>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string PropertyName = null) => PropertyChanged.Start(this, new PropertyChangedEventArgs(PropertyName));

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs Args) => CollectionChanged?.Invoke(this, Args);

        private DateTime _Time;
        private string _Value;
        private string _Message;
        private object _Data;
        private LogType _Type;
        [NotNull, ItemNotNull] private readonly List<LogItem> _Items = new List<LogItem>();
        private bool _Initialized;

        public int ItemsCount => _Items.Count;

        [NotNull] public LogItem First => _Items.Count == 0 ? this : _Items[0];
        [NotNull] public LogItem Last => this;

        public TimeSpan TimeDelta
        {
            get
            {
                this.GetMinMax(i => i.Time.Ticks, out var begin, out var end);
                return end.Time - begin.Time;
            }
        }

        public LogItem this[int Index] => Index == _Items.Count ? this : _Items[Index];

        public LogItem this[DateTime time] => this.Select(item => (item, delta: (item.Time - time).TotalSeconds.Abs())).GetMin(i => i.delta).item;

        public LogItem this[string value] => this.FirstOrDefault(i => i.Value == value);

        public DateTime Time
        {
            get => _Time;
            private set
            {
                if(_Time.Equals(value)) return;
                _Time = value;
                OnPropertyChanged();
            }
        }

        public string Value { get => _Value; set => Add(value, _Type); }

        public LogType Type
        {
            get => _Type;
            set
            {
                if(_Type == value) return;
                _Type = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _Message;
            set
            {
                if(_Message == value) return;
                _Message = value;
                OnPropertyChanged();
            }
        }

        public object Data
        {
            get => _Data;
            set
            {
                if(ReferenceEquals(_Data, value)) return;
                _Data = value;
                OnPropertyChanged();
            }
        }

        internal LogItem() { }

        internal LogItem(DateTime Time, string Value, LogType Type)
        {
            _Initialized = true;
            _Time = Time;
            _Value = Value;
            _Type = Type;
        }

        public void Clear()
        {
            _Time = default;
            _Value = null;
            _Initialized = false;
            _Items.Clear();
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(Value));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        [NotNull] public LogItem Add(string value, LogType type = LogType.Information) => Add(DateTime.Now, value, type);

        [NotNull]
        public LogItem Add(DateTime time, string value, LogType type = LogType.Information)
        {
            var old_value = _Value;
            var old_time = _Time;
            _Value = value;
            Time = time;
            if(!old_value.Equals(value)) OnPropertyChanged(nameof(Value));
            if(_Type != type)
            {
                _Type = type;
                OnPropertyChanged(nameof(Type));
            }
            var item = new LogItem(old_time, old_value, type);
            if(!_Initialized)
                _Initialized = true;
            else
            {
                _Items.Add(item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
            }
            return item;
        }

        public override string ToString() => $"{_Time}:{_Value}{(ItemsCount > 0 ? $"[{ItemsCount}]" : "")}";

        public IEnumerator<LogItem> GetEnumerator() => _Items.AppendLast(new LogItem(_Time, _Value, _Type)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}