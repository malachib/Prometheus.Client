using System;
using System.IO;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class HistogramTestsA : BaseMetricTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var histogram = factory.CreateHistogram("test_Histogram", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => histogram.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateHistogram("test_Histogram", string.Empty, label));
        }

        [Theory]
        [InlineData("le")]
        [InlineData("le", "label")]
        [InlineData("label", "le")]
        public void ThrowOnReservedLabelNames(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateHistogram("test_Histogram", string.Empty, labels));
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var histogram = factory.CreateHistogram("test_histogram", string.Empty, "label");
            var labeled1 = histogram.WithLabels("value");
            var labeled2 = histogram.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var histogram = factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");
            var labeled1 = histogram.WithLabels("value1", "value2");
            var labeled2 = histogram.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }
    }
}
