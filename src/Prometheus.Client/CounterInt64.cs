using System;
using System.Collections.Generic;
using Prometheus.Client.Abstractions;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client
{
    public class CounterInt64 : MetricBase<MetricConfiguration>, ICounter<long>
    {
        private ThreadSafeLong _value;

        public CounterInt64(MetricConfiguration configuration, IReadOnlyList<string> labels)
            : base(configuration, labels)
        {
        }

        public void Inc()
        {
            Inc(1, null);
        }

        public void Inc(long increment)
        {
            Inc(increment, null);
        }

        public void Inc(long increment, long? timestamp)
        {
            if (increment < 0)
                throw new ArgumentOutOfRangeException(nameof(increment), "Counter cannot go down");

            _value.Add(increment);
            TimestampIfRequired(timestamp);
        }

        public long Value => _value.Value;

        protected internal override void Collect(IMetricsWriter writer)
        {
            writer.WriteSample(Value, string.Empty, Labels, Timestamp);
        }
    }
}
