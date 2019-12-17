using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class FactoryTests : BaseMetricTests
    {
        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateCounter("test_counter", string.Empty, label));
        }

        [Theory]
        [MemberData(nameof(InvalidLabels))]
        public void ThrowOnInvalidLabels_Tuple(string label)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.ThrowsAny<ArgumentException>(() => factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create(label)));
        }

        [Fact]
        public void ThrowOnNameConflict()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateCounter("test_counter", string.Empty, "label1", "label2");

            Assert.ThrowsAny<InvalidOperationException>(() => factory.CreateCounter("test_counter", string.Empty, "testlabel"));
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var counter2 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Empty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty);
            var counter2 = factory.CreateCounter("test_counter", string.Empty);

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStrings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyTuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateCounter("test_counter", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateCounter("test_counter", string.Empty, ValueTuple.Create());

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }
    }
}
