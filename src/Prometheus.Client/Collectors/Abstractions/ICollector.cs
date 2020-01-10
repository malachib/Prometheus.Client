using System.Collections.Generic;
using Prometheus.Client.MetricsWriter.Abstractions;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollector
    {
        ICollectorConfiguration Configuration { get; }

        IReadOnlyList<string> MetricNames { get; }

        void Collect(IMetricsWriter writer);
    }

    /// <typeparam name="TIChild">Interface to which collector conforms (i.e. ICounter, IHistogram, etc)</typeparam>
    public interface ICollector<TIChild> : ICollector
    {
        TIChild WithLabels(params string[] labels);

        TIChild Unlabelled { get; }
    }
}
