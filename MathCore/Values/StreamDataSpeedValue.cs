using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

namespace MathCore.Values
{
    /// <summary>���������� �������� ������ ������</summary>
    public class StreamDataSpeedValue
    {
        /* ------------------------------------------------------------------------------------------ */

        /// <summary>�������� ���������</summary>
        private static readonly string[] __DataNames = Consts.DataLength.Bytes.GetDataNames().Initialize((s, i) => s + "/�");

        /// <summary>������� �����</summary>
        private static DateTime Now { [DST] get => DateTime.Now; }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>��������, ���������� �� �������</summary>
        private readonly TimeAverageValue _AverageValue = new TimeAverage2Value(30);

        /// <summary>����� ������</summary>
        private readonly Stream _DataStream;

        /// <summary>��������� ��������� � ������ ������</summary>
        private long _LastPosition;

        /// <summary>��������� �����</summary>
        private DateTime _LastTime;

        /// <summary>���������� ������ ����� ������� � ��������� �������������</summary>
        private int _Round = 2;

        /// <summary>��������� ��������������� �������� ��������</summary>
        private double _LastSpeedValue;

        /// <summary>���������� ���������� �������� ������� � �������� ��� �������� �������� ��������</summary>
        private double _SpeedCheckTimeout = 0.25;

        /* ------------------------------------------------------------------------------------------ */


        /// <summary>���������� ������ ����� ������� � ��������� �������������</summary>
        public int Round
        {
            [DST] get => _Round;
            [DST] set => _Round = value;
        }

        /// <summary>���������� �������� ��������</summary>
        public double Value { [DST] get => CheckSpeed(); }

        /// <summary>���������� �������� ��������</summary>
        public double AverageValue { [DST] get => _AverageValue.Add(Value); }

        /// <summary>��������� ������������� ��������</summary>
        public string SpeedStr
        {
            [DST]
            get
            {
                var speed = Value;
                var i = 0;
                while(speed / 1024 > 0.8) { speed /= 1024; i++; }
                return $"{speed.Round(Round)} {__DataNames[i]}";
            }
        }

        /// <summary>���������� ������� ���������� </summary>
        public double AverageTau
        {
            [DST] get => _AverageValue.Tau;
            [DST] set => _AverageValue.Tau = value;
        }

        /// <summary>���������� ���������� �������� ������� �������� ��������</summary>
        public double SpeedCheckTimeout
        {
            [DST]
            get => _SpeedCheckTimeout;
            [DST]
            set
            {
                Contract.Requires(value >= 0);
                Contract.Ensures(_SpeedCheckTimeout >= 0);

                _SpeedCheckTimeout = value;
            }
        }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>����� ���������� �������� ������ ������</summary>
        /// <param name="DataStream">����� ������ ��� ���������</param>
        /// <exception cref="ArgumentNullException">���������� ��������� ��� ������� ������ �� ����� ������</exception>
        public StreamDataSpeedValue(Stream DataStream)
        {
            Contract.Requires(DataStream != null, "DataStream is null");
            _DataStream = DataStream;
            Reset();
        }

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>����� ����������: ��������� ���������� �������� ��������� � ������, ����� �����������</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Reset()
        {
            _LastPosition = _DataStream.Position;
            _AverageValue.Reset();
            _LastTime = Now;
        }

        /// <summary>�������� ��������� ��������</summary>
        /// <returns>���������� ���������� �������� ����������� � ������</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private double CheckSpeed()
        {
            var now_time = Now;
            var delta_time = now_time - _LastTime;
            if(delta_time.TotalSeconds < _SpeedCheckTimeout) return _LastSpeedValue;

            var now_position = _DataStream.Position;
            var delta_position = now_position - _LastPosition;
            if(delta_position == 0) return _LastPosition = 0;


            var now_speed = delta_position / delta_time.TotalSeconds;
            _LastPosition = now_position;
            _LastTime = now_time;
            return _LastSpeedValue = now_speed;
        }

        /* ------------------------------------------------------------------------------------------ */

        public override string ToString() => SpeedStr;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>������� �������������� ���������� �������� � �������� �������� (�����������)</summary>
        /// <param name="speed">���������� ��������</param>
        /// <returns>�������� ��������</returns>
        public static implicit operator double(StreamDataSpeedValue speed) => speed.AverageValue;

        /* ------------------------------------------------------------------------------------------ */

        // ReSharper disable UnusedMember.Local
        // ReSharper disable InvocationIsSkipped
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_SpeedCheckTimeout >= 0);
            Contract.Invariant(_DataStream != null);
        }
        // ReSharper restore InvocationIsSkipped
        // ReSharper restore UnusedMember.Local

        /* ------------------------------------------------------------------------------------------ */
    }
}