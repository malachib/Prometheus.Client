using System;
using System.Collections.Generic;
using System.Text;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class AbstractionTests : BaseTests
    {
        [Fact]
        public void CounterTest()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            // TODO: Try to get this working with ICounter rather than LabelledCounter
            ICollector<ICounter> counter = factory.CreateCounter("test_counter", string.Empty, "label");
            //counter.Inc(1);
            var labeled = counter.WithLabels("value");
            labeled.Inc(2);

            //counter.Reset();

            //Assert.Equal(0, counter.Value);
            Assert.Equal(2, labeled.Value);
        }

        [Fact]
        public void HistogramTest()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            ICollector<IHistogram> histogram = factory.CreateHistogram("test_histogram", string.Empty, "label");
            var labeled1 = histogram.WithLabels("value");
            var labeled2 = histogram.WithLabels("value");

            Assert.Equal(labeled1, labeled2);

            labeled1.Observe(1);
        }
    }
}
