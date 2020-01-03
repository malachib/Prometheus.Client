using System;
using System.Globalization;
using System.Threading;

namespace Prometheus.Client
{
    internal struct ThreadSafeDouble : IEquatable<ThreadSafeDouble>
    {
        private long _value;
        private bool _isNan;

        public ThreadSafeDouble(double value)
        {
            _value = BitConverter.DoubleToInt64Bits(value);
            _isNan = IsNaN(value);
        }

        public double Value
        {
            get
            {
                if (Volatile.Read(ref _isNan))
                    return double.NaN;

                return BitConverter.Int64BitsToDouble(Interlocked.Read(ref _value));
            }
            set
            {
                if (IsNaN(value))
                    Volatile.Write(ref _isNan, true);
                else
                    Interlocked.Exchange(ref _value, BitConverter.DoubleToInt64Bits(value));
            }
        }

        public void Add(double increment)
        {
            while (true)
            {
                if (Volatile.Read(ref _isNan))
                    throw new InvalidOperationException("Cannot increment the NaN value.");

                long initialValue = _value;
                double computedValue = BitConverter.Int64BitsToDouble(initialValue) + increment;

                if (initialValue == Interlocked.CompareExchange(ref _value, BitConverter.DoubleToInt64Bits(computedValue), initialValue))
                    return;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(ThreadSafeDouble threadSafeLong)
        {
            return Value.Equals(threadSafeLong.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ThreadSafeDouble d)
                return Equals(d);

            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        internal static bool IsNaN(double val)
        {
            if (val >= 0d || val < 0d)
                return false;

            return true;
        }
    }
}
