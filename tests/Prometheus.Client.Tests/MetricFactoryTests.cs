using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class MetricFactoryTests
    {
        [Fact]
        public void ThrowOnDuplicatedMetricNames()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test", "");
            Assert.Throws<InvalidOperationException>(() => factory.CreateGauge("test", ""));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(16)]
        public void FactoryProxyUsesCache(int labelsCount)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var fn1 = factory.GetCounterFactory(labelsCount);
            var fn2 = factory.GetCounterFactory(labelsCount);

            Assert.True(fn1 == fn2);
        }
    }
}
