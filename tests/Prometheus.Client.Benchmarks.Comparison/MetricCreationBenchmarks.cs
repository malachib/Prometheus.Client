extern alias Their;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Comparison
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class MetricCreationBenchmarks
    {
        private const int _metricsPerIteration = 10000;

        /// <summary>
        /// Some benchmarks try to register metrics that already exist.
        /// </summary>
        private const int _duplicateCount = 5;

        private const string _help = "arbitrary help message for metric, not relevant for benchmarking";

        private static readonly string[] _metricNames;

        static MetricCreationBenchmarks()
        {
            _metricNames = new string[_metricsPerIteration];

            for (var i = 0; i < _metricsPerIteration; i++)
                _metricNames[i] = $"metric_{i:D4}";
        }

        private MetricFactory _factory;
        private Their.Prometheus.MetricFactory _theirFactory;

        [IterationSetup]
        public void Setup()
        {
            _factory = new MetricFactory(new CollectorRegistry());

            var registry = Their.Prometheus.Metrics.NewCustomRegistry();
            _theirFactory = Their.Prometheus.Metrics.WithCustomRegistry(registry);
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Counter_Single")]
        public void Counter_SingleBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                _theirFactory.CreateCounter("testcounter", _help);
        }

        [Benchmark, BenchmarkCategory("Counter_Single")]
        public void Counter_Single()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                _factory.CreateCounter("testcounter", _help);
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsBaseLine()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                _theirFactory.CreateCounter("testcounter", _help, "foo", "bar", "baz");
        }

        [Benchmark, BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabels()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                _factory.CreateCounter("testcounter", _help, "foo", "bar", "baz");
        }

        [Benchmark, BenchmarkCategory("Counter_Single_WithLabels")]
        public void Counter_SingleLabelsTuple()
        {
            for (var i = 0; i < _metricsPerIteration; i++)
                _factory.CreateCounter("testcounter", _help, ("foo", "bar", "baz"));
        }

        //[Benchmark]
        //public void Counter_Many()
        //{
        //    for (var i = 0; i < _metricCount; i++)
        //        _factory.CreateCounter(_metricNames[i], _help, _labelNames);
        //}

        //[Benchmark]
        //public void CounterTuple_Many()
        //{
        //    for (var i = 0; i < _metricCount; i++)
        //        _factory.CreateCounter(_metricNames[i], _help, _labelNamesTuple);
        //}

        //[Benchmark]
        //public void Gauge_Many()
        //{
        //    for (var i = 0; i < _metricCount; i++)
        //        _factory.CreateGauge(_metricNames[i], _help, _labelNames).Inc();
        //}

        //[Benchmark]
        //public void Summary_Many()
        //{
        //    for (var i = 0; i < _metricCount; i++)
        //        _factory.CreateSummary(_metricNames[i], _help, _labelNames).Observe(123);
        //}

        //[Benchmark]
        //public void Histogram_Many()
        //{
        //    for (var i = 0; i < _metricCount; i++)
        //        _factory.CreateHistogram(_metricNames[i], _help, _labelNames).Observe(123);
        //}

        //[Benchmark]
        //public void Counter_Many_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //    {
        //        for (var i = 0; i < _metricCount; i++)
        //            _factory.CreateCounter(_metricNames[i], _help, _labelNames).Inc();
        //    }
        //}

        //[Benchmark]
        //public void Gauge_Many_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //    {
        //        for (var i = 0; i < _metricCount; i++)
        //            _factory.CreateGauge(_metricNames[i], _help, _labelNames).Inc();
        //    }
        //}

        //[Benchmark]
        //public void Summary_Many_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //    {
        //        for (var i = 0; i < _metricCount; i++)
        //            _factory.CreateSummary(_metricNames[i], _help, _labelNames).Observe(123);
        //    }
        //}

        //[Benchmark]
        //public void Histogram_Many_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //    {
        //        for (var i = 0; i < _metricCount; i++)
        //            _factory.CreateHistogram(_metricNames[i], _help, _labelNames).Observe(123);
        //    }
        //}

        //[Benchmark]
        //public void Counter()
        //{
        //    _factory.CreateCounter(_metricNames[0], _help, _labelNames).Inc();
        //}

        //[Benchmark]
        //public void Gauge()
        //{
        //    _factory.CreateGauge(_metricNames[0], _help, _labelNames).Inc();
        //}

        //[Benchmark]
        //public void Summary()
        //{
        //    _factory.CreateSummary(_metricNames[0], _help, _labelNames).Observe(123);
        //}

        //[Benchmark]
        //public void Histogram()
        //{
        //    _factory.CreateHistogram(_metricNames[0], _help, _labelNames).Observe(123);
        //}

        //[Benchmark]
        //public void Counter_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //        _factory.CreateCounter(_metricNames[0], _help, _labelNames).Inc();
        //}

        //[Benchmark]
        //public void Gauge_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //        _factory.CreateGauge(_metricNames[0], _help, _labelNames).Inc();
        //}

        //[Benchmark]
        //public void Summary_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //        _factory.CreateSummary(_metricNames[0], _help, _labelNames).Observe(123);
        //}

        //[Benchmark]
        //public void Histogram_Duplicates()
        //{
        //    for (var dupe = 0; dupe < _duplicateCount; dupe++)
        //        _factory.CreateHistogram(_metricNames[0], _help, _labelNames).Observe(123);
        //}
    }
}
