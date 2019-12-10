using System;
using System.Globalization;
using System.Threading;

namespace Prometheus.Client
{
    internal struct ThreadSafeDouble : IEquatable<ThreadSafeDouble>
    {
        private long _value;

        public ThreadSafeDouble(double value)
        {
            // TODO: replace this magic with Interlocked.Exchange(double)
            _value = BitConverter.DoubleToInt64Bits(value);
        }

        public double Value
        {
            get => BitConverter.Int64BitsToDouble(Interlocked.Read(ref _value));
            set => Interlocked.Exchange(ref _value, BitConverter.DoubleToInt64Bits(value));
        }

        public void Add(double increment)
        {
            while (true)
            {
                long initialValue = Interlocked.Read(ref _value);
                double computedValue = BitConverter.Int64BitsToDouble(initialValue) + increment;

                if (double.IsNaN(computedValue))
                    throw new InvalidOperationException("Cannot increment the NaN value.");

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
    }
}
