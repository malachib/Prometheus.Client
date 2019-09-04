using NSubstitute;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.CounterTests
{
    public class MetricFactoryLegacyExtensionsTests : BaseMetricTests
    {
        [Fact]
        public void CreateCounter()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", "label");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.Default, "label");
        }

        [Fact]
        public void CreateCounterWithTs()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, "label");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label");
        }

        [Fact]
        public void CreateCounterWithTsWithSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, false, "label");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.SupressEmptySamples | MetricFlags.IncludeTimestamp, "label");
        }

        [Fact]
        public void CreateCounterWithTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", true, false, "label");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.IncludeTimestamp, "label");
        }

        [Fact]
        public void CreateCounterWithoutTsWithoutSupressEmpty()
        {
            var factory = Substitute.For<MetricFactory>(new CollectorRegistry());

            factory.CreateCounter("testName", "testHelp", false, false, "label");

            factory.Received().CreateCounter("testName", "testHelp", MetricFlags.None, "label");
        }
    }
}
