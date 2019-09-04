using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.GaugeTests
{
    public class LabelsTests : BaseMetricTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => gauge.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateGauge("test_gauge", string.Empty, label));
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            var labeled1 = gauge.WithLabels("value");
            var labeled2 = gauge.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label1", "label2");
            var labeled1 = gauge.WithLabels("value1", "value2");
            var labeled2 = gauge.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void WithLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty, "label");
            gauge.Inc(2);
            var labeled = gauge.WithLabels("value");
            labeled.Inc(3);

            Assert.Equal(2, gauge.Unlabelled.Value);
            Assert.Equal(3, labeled.Value);
        }

        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateGauge("test_gauge", string.Empty);
            gauge.Inc(2);

            Assert.Equal(2, gauge.Value);
        }
    }
}
