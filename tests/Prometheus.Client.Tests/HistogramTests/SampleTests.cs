using System;
using System.Collections.Generic;
using Prometheus.Client.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class SampleTests : BaseMetricTests
    {
        [Theory]
        [InlineData()]
        public void ShouldPopulateSumOnObservations(IReadOnlyList<double> items)
        {


        }

        private IHistogram CreateHistogram(double[] buckets = null, MetricFlags options = MetricFlags.Default)
        {
            var config = new HistogramConfiguration("test", string.Empty, Array.Empty<string>(), buckets, options);
            return new Histogram(config, Array.Empty<string>());
        }
    }
}
