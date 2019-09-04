using System;

namespace Prometheus.Client.Collectors.Abstractions
{
    public interface ICollectorConfiguration
    {
        string Name { get; }
    }
}
