using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class MetricFamilyTests : BaseMetricTests
    {
        [Fact]
        public void SameLabelReturnsSameSample()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label");
            var labeled1 = counter.WithLabels("value");
            var labeled2 = counter.WithLabels("value");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled1 = counter.WithLabels("value1", "value2");
            var labeled2 = counter.WithLabels("value1", "value2");

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var labeled1 = counter.WithLabels(("value1", "value2"));
            var labeled2 = counter.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }

        [Fact]
        public void SameLabelsReturnsSameSample_StringsAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter = factory.CreateCounter("test_counter", string.Empty, "label1", "label2");
            var labeled1 = counter.WithLabels("value1", "value2");

            var counterTuple = factory.CreateCounter("test_counter", string.Empty, ("label1", "label2"));
            var labeled2 = counterTuple.WithLabels(("value1", "value2"));

            Assert.Equal(labeled1, labeled2);
        }
    }
}
