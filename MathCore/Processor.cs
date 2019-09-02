using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;
using MathCore;
using MathCore.Annotations;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedMember.Global

namespace System
{
    /// <summary>����� ��������, ����������� ��������� ����������� �������� � ��������� ������� ������</summary>
    public abstract class Processor : INotifyPropertyChanged, IDisposable
    {
        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������-���������, ������������ � �������� ��������� ������� ������ ��� ���������������� ������ ����������</summary>
        private static readonly Exception __AcyncException = new ApplicationException("���������������� ���������");

        /// <summary>������� ����� �������</summary>
        protected static DateTime Now { [DST] get => DateTime.Now; }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������� ��������� �������� �������</summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized), DST]
            add => PropertyChanged_ += value;
            [MethodImpl(MethodImplOptions.Synchronized), DST]
            remove => PropertyChanged_ -= value;
        }
        private event PropertyChangedEventHandler PropertyChanged_;
        /// <summary>����� ������� ��������� �������� �������</summary>
        ///  <param name="e">��������� ������� ��������� �������� �������, ���������� ��� ��������</param>
        [DST]
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged_?.Invoke(this, e);

        /// <summary>����� ������� ��������� �������� ������� � ��������� ����� ��������</summary>
        /// <param name="PropertyName">��� ������������� ��������</param>
        [DST, NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs(PropertyName));

        /// <summary>������� ��������� �������� ���������� ����������</summary>
        public event EventHandler EnableChanged;
        /// <summary>�������� ������� ��������� �������� ���������� ����������</summary><param name="e">��������� �������</param>
        [DST]
        protected virtual void OnEnableChanged(EventArgs e) => EnableChanged?.Invoke(this, e);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������� ������� ����������</summary>
        public event EventHandler ProcessStarted;
        /// <summary>�������� ������� ������� ����������</summary><param name="e">��������� �������</param>
        [DST]
        protected virtual void OnProcessStarted(EventArgs e) => ProcessStarted?.Invoke(this, e);

        /// <summary>������� ���������� ������ ����������</summary>
        public event EventHandler ProcessComplited;
        /// <summary>�������� ������� ���������� ������ ����������</summary><param name="e">��������� �������</param>
        [DST]
        protected virtual void OnProcessComplited(EventArgs e) => ProcessComplited.FastStart(this, e);

        /// <summary>�������, ����������� ��� ������������� ���������� � �������� ������ ����������</summary>
        public event ExceptionEventHandler<Exception> Error;
        /// <summary>�������� ������� ������������� �������������� �������� � �������� ������ ����������</summary>
        /// <param name="e">�������� ������� ������, ���������� ������ ����������</param>
        [DST]
        protected virtual void OnError(ExceptionEventHandlerArgs<Exception> e) => Error.ThrowIfUnhandled(this, e, true);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>
        /// ������� ������� �������� ������������� ������ ��������� ��� ��� ����������, 
        /// ����� �������� ����� ����������� ������� Abort()
        /// �� ��������� 100 ��.
        /// </summary>
        protected int _JoinThreadTimeout = 100;

        /// <summary>���� ���������� ������ ���������. ���� �������� ����� "������" - ����� �����������</summary>
        protected volatile bool _Enabled;

        /// <summary>������ ������������� �������/��������� ���������� - ������ ��� ������</summary>
        protected readonly object _StartStopSectionLocker = new object();

        /// <summary>�������� ����� ������ ����������</summary>
        protected Thread _MainWorkThread;

        /// <summary>����� �������</summary>
        private DateTime? _StartTime;

        /// <summary>����� ���������</summary>
        private DateTime? _StopTime;

        /// <summary>������-����������� �� ���������� ����������</summary>
        private readonly ProgressMonitor _Monitor = new ProgressMonitor("��������");

        /// <summary>������� ��������� ������ ����������</summary>
        private ThreadPriority _Priority = ThreadPriority.Normal;

        /// <summary>������� ���������� ����� ������� ���������� </summary>
        private int _ActionTimeout;

        /// <summary>����� ��������� ������� �������� ��� ����������� ������ ����������</summary>
        private Action<int> _Set_Timeout;

        /// <summary>������� ���������� ������</summary>
        private volatile bool _IsSychronus;

        /// <summary>����, ����������� ����� ������� ������ � ������ ���������������� ������</summary>
        private volatile bool _ErrorIfAcync;

        /// <summary>���������� ����������� ������ ���������</summary>
        private long _CyclesCount;

        // ReSharper disable FieldCanBeMadeReadOnly.Global
        /// <summary>��� ��� ������������ ������ ���������� ����������</summary>
        protected string _NameForeNewMainThread;
        // ReSharper restore FieldCanBeMadeReadOnly.Global

        // ReSharper disable NotAccessedField.Global
        /// <summary>����� ���������� ������� ���������� ������ ����� ��������� ������ ����������</summary>
        protected Func<TimeSpan> _Get_LastDeltaTime;
        // ReSharper restore NotAccessedField.Global

        /// <summary>������ ������������ ������� �� ������� ����������</summary>
        protected readonly EventWaitHandle _StartWaitHandle = new ManualResetEvent(false);

        /// <summary>������ ������������ ������� �� ��������� ����������</summary>
        protected readonly EventWaitHandle _StopWaitHandle = new ManualResetEvent(false);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>��������� ���������� ������ ����������</summary>
        public ThreadPriority Priority
        {
            [DST]
            get => _Priority;
            [MethodImpl(MethodImplOptions.Synchronized), DST]
            set
            {
                _Priority = value;
                var lv_Thread = _MainWorkThread;
                lock (_StartStopSectionLocker)
                    if(lv_Thread != null && (lv_Thread.IsAlive || lv_Thread.IsBackground))
                        lv_Thread.Priority = value;
                OnPropertyChanged();
            }
        }

        /// <summary>������� ���������� ����������</summary>
        public bool Enable { [DST] get => _Enabled; [DST] set { if(value) Start(); else Stop(); } }

        /// <summary>�������� ����� ������ ����������</summary>
        public Thread MainThread { [DST] get => _MainWorkThread; }

        /// <summary>������� ������� ������������� ��������� ������ ���������� � �������, ����������� ��� ������.</summary>
        public int JoinThreadTimeout
        {
            [DST]
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return _JoinThreadTimeout;
            }
            [DST]
            set
            {
                Contract.Requires(value >= 0);

                _JoinThreadTimeout = value;
                OnPropertyChanged();
            }
        }

        /// <summary>����� �������</summary>
        public DateTime? StartTime { [DST] get => _StartTime; [DST] protected set { _StartTime = value; OnPropertyChanged(); } }

        /// <summary>����� ���������</summary>
        public DateTime? StopTime { [DST] get => _StopTime; protected set { _StopTime = value; OnPropertyChanged(); } }

        /// <summary>�����, ��������� ����� �������</summary>
        public TimeSpan? ElapsedTime { [DST] get { var start = _StartTime; return start is null ? (TimeSpan?)null : Now - start.Value; } }

        /// <summary>������-����������� �� ���������� ����������</summary>
        public ProgressMonitor Monitor => _Monitor;

        /// <summary>������� �������� ����������� �������� � �������������</summary>
        public int ActionTimeout
        {
            [DST]
            get => _ActionTimeout;
            [DST]
            set
            {
                Contract.Requires(value >= 0);
                Contract.Ensures(_ActionTimeout >= 0);

                if(_ActionTimeout == value) return;
                _ActionTimeout = value;
                _Set_Timeout?.Invoke(value);
                OnPropertyChanged();
            }
        }

        /// <summary>������� ���������� ������</summary>
        public bool IsSynchronus { [DST] get => _IsSychronus; }

        /// <summary>������������ ������ � ������ ����������������?</summary>
        public bool ErrorIfAsync { [DST] get => _ErrorIfAcync; [DST] set { _ErrorIfAcync = value; OnPropertyChanged(); } }

        /// <summary>���������� ���������� ������</summary>
        public long CyclesCount { [DST] get => _CyclesCount; }

        /* ------------------------------------------------------------------------------------------ */

        [DST]
        protected Processor() => _NameForeNewMainThread = $"{GetType().Name}Thread";

        /* ------------------------------------------------------------------------------------------ */

        [DST]
        public virtual void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>������ ���������</summary>
        [DST]
        // ReSharper disable VirtualMemberNeverOverriden.Global
        public virtual void Start()
        // ReSharper restore VirtualMemberNeverOverriden.Global
        {
            if(_Enabled) return;
            lock (_StartStopSectionLocker)
            {
                if(_Enabled) return;
                _Enabled = true;
                _MainWorkThread = new Thread(ThreadMethod)
                {
                    IsBackground = true,
                    Name = _NameForeNewMainThread,
                    Priority = _Priority
                };

                _MainWorkThread.Start();

                _StopWaitHandle.Reset();
                _StartWaitHandle.Set();
            }
            OnEnableChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Enable));
        }

        /// <summary>��������� ���������</summary>
        [DST]
        // ReSharper disable VirtualMemberNeverOverriden.Global
        public virtual void Stop()
        // ReSharper restore VirtualMemberNeverOverriden.Global
        {
            if(!_Enabled) return;
            lock (_StartStopSectionLocker)
            {
                if(!_Enabled) return;
                _Enabled = false;
                if(!_MainWorkThread.Join(_JoinThreadTimeout))
                    _MainWorkThread.Abort();
                _MainWorkThread = null;
                _Set_Timeout = null;
                _Get_LastDeltaTime = null;

                _StartWaitHandle.Reset();
                _StopWaitHandle.Set();
            }
            OnEnableChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Enable));
        }

        /// <summary>����������� ����� �� ������� ����������</summary>
        [DST]
        public bool WaitToStart(TimeSpan? Timeout = null) => Timeout is null || Timeout.Value.Ticks == 0
            ? _StartWaitHandle.WaitOne()
            : _StartWaitHandle.WaitOne(Timeout.Value);

        /// <summary>����������� ���� �� ��������� ����������</summary>
        [DST]
        public bool WaitToStop(TimeSpan? Timeout = null) => Timeout is null || Timeout.Value.Ticks == 0
            ? _StopWaitHandle.WaitOne()
            : _StopWaitHandle.WaitOne(Timeout.Value);

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>�������� ����� ����������, ����������� � ��������� ������ </summary>
        // ReSharper disable VirtualMemberNeverOverriden.Global
        //        [DST]
        protected virtual void ThreadMethod()
        // ReSharper restore VirtualMemberNeverOverriden.Global
        {
            InitializeAction();

            #region ����������

            var timeout = TimeSpan.FromMilliseconds(_ActionTimeout);
            _Set_Timeout = new_timeout => timeout = TimeSpan.FromMilliseconds(new_timeout);


            var delta = new TimeSpan();
            // ReSharper disable AccessToModifiedClosure
            _Get_LastDeltaTime = () => delta;
            // ReSharper restore AccessToModifiedClosure

            // ReSharper disable TooWideLocalVariableScope
            DateTime start_time;
            DateTime stop_time;
            TimeSpan time_to_sleep;
            bool is_sychronus;
            // ReSharper restore TooWideLocalVariableScope 

            #endregion

            #region �������� ��������

            while(_Enabled)
            {
                start_time = Now;
                try { MainAction(); } catch(Exception Error)
                {
                    var lv_EventArgs = new ExceptionEventHandlerArgs<Exception>(Error);
                    OnError(lv_EventArgs);
                    if(lv_EventArgs.NeedToThrow) throw;
                    if(Error is ThreadAbortException) Thread.ResetAbort();
                }
                stop_time = Now;
                _CyclesCount++;
                if(timeout.Ticks <= 0) continue;
                delta = stop_time - start_time;
                time_to_sleep = timeout - delta;
                is_sychronus = _IsSychronus = timeout.Ticks > 0 && time_to_sleep.Ticks > 0;
                if(is_sychronus)
                    Thread.Sleep(time_to_sleep);
                else if(_ErrorIfAcync)
                    OnError(__AcyncException);
            }

            #endregion

            FinalizeAction();
        }

        /// <summary>�������� ����� �������� ����������, ���������� � �����. ������ ���� �������������� � �������-�����������</summary>
        protected abstract void MainAction();

        /// <summary>������������� ��������</summary>
        [DST]
        private void InitializeAction()
        {
            try { Initializer(); } catch(Exception Error)
            {
                var lv_EventArgs = new ExceptionEventHandlerArgs<Exception>(Error);
                OnError(lv_EventArgs);
                if(lv_EventArgs.NeedToThrow) throw;
                if(Error is ThreadAbortException) Thread.ResetAbort();
            }
        }

        /// <summary>
        /// ����� �������������. ���������� ����� ������� ��������� ����� ������� ��������� �����.
        /// �� ��������� �������� ��������� ������� ������� ����������
        /// </summary>
        [DST]
        protected virtual void Initializer()
        {
            _StartTime = Now;
            _StopTime = null;
            _CyclesCount = 0;
            _IsSychronus = true;
            _Monitor.Status = "���������";
            OnProcessStarted(EventArgs.Empty);
        }

        /// <summary>
        /// �����, ����������� ������� ���������. ���������� ����� ������ ���������� �� ��������� �����.
        /// �� ��������� �������� ��������� ������� ���������� ������ ����������
        /// </summary>
        // ReSharper disable VirtualMemberNeverOverriden.Global
        [DST]
        protected virtual void Finalizer()
        // ReSharper restore VirtualMemberNeverOverriden.Global
        {
            _StopTime = Now;
            _Monitor.Status = "���������";
            OnProcessComplited(EventArgs.Empty);
        }

        /// <summary>����������� �������� ��������</summary>
        [DST]
        private void FinalizeAction()
        {
            try { Finalizer(); } catch(Exception Error)
            {
                var lv_EventArgs = new ExceptionEventHandlerArgs<Exception>(Error);
                OnError(lv_EventArgs);
                if(lv_EventArgs.NeedToThrow) throw;

                if(Error is ThreadAbortException) Thread.ResetAbort();
            }
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            Stop();
            _StartWaitHandle.Dispose();
            _StopWaitHandle.Dispose();
        }

        /// <inheritdoc />
        public void Dispose() => Dispose(true);

        /* ------------------------------------------------------------------------------------------ */
    }
}