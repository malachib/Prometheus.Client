using System;
using NSubstitute;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.Abstractions;
using Prometheus.Client.MetricsWriter.Abstractions;
using Xunit;

namespace Prometheus.Client.Tests
{
    public class UntypedTestsA : BaseMetricTests
    {
        [Theory]
        [MemberData(nameof(GetLabels))]
        public void ThrowOnLabelsMismatch(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            Assert.ThrowsAny<ArgumentException>(() => untyped.WithLabels(labels));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateUntyped("test_untyped", string.Empty, label));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3.1)]
        public void MetricsWriterApiUsage(double value)
        {
            var writer = Substitute.For<IMetricsWriter>();
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);
            var untyped = factory.CreateUntyped("name1", "help1", "labelled");
            var val = value + 1;

            untyped.Set(val);
            untyped.WithLabels("lbl").Set(value);

            ((ICollector)untyped).Collect(writer);

            Received.InOrder(() =>
            {
                writer.StartMetric("name1");
                writer.WriteHelp("help1");
                writer.WriteType(MetricType.Untyped);

                var sample1 = writer.StartSample();
                sample1.WriteValue(val);
                sample1.EndSample();

                var sample2 = writer.StartSample();
                var lbl = sample2.StartLabels();
                lbl.WriteLabel("labelled", "lbl");
                lbl.EndLabels();
                sample2.WriteValue(value);
                sample2.EndSample();

                writer.EndMetric();
            });
        }

        [Fact]
        public void SameLabelReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label");
            var labeled1 = untyped.WithLabels("value");
            var labeled2 = untyped.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameMetric()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label1", "label2");
            var labeled1 = untyped.WithLabels("value1", "value2");
            var labeled2 = untyped.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void WithLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty, "label");
            untyped.Set(2);
            var labeled = untyped.WithLabels("value");
            labeled.Set(3);

            Assert.Equal(2, untyped.Unlabelled.Value);
            Assert.Equal(3, labeled.Value);
        }

        [Fact]
        public void WithoutLabels()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty);
            untyped.Set(2);

            Assert.Equal(2, untyped.Unlabelled.Value);
        }

        [Fact]
        public void ShouldAllowNaNValue()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var untyped = factory.CreateUntyped("test_untyped", string.Empty);
            untyped.Set(double.NaN);

            Assert.Equal(double.NaN, untyped.Unlabelled.Value);
        }
    }
}
