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
        public void Test1()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            // TODO: Try to get this working with ICounter rather than LabelledCounter
            ICollector<Counter.LabelledCounter> counter = factory.CreateCounter("test_counter", string.Empty, "label");
            //counter.Inc(1);
            var labeled = counter.WithLabels("value");
            labeled.Inc(2);

            //counter.Reset();

            //Assert.Equal(0, counter.Value);
            Assert.Equal(2, labeled.Value);
        }
    }
}
