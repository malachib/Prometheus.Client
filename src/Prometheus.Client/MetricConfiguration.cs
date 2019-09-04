using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    public class MetricConfiguration: CollectorConfiguration
    {
        private static readonly Regex _metricNameLabelRegex = new Regex("^[a-zA-Z_:][a-zA-Z0-9_:]*$", RegexOptions.Compiled);
        private static readonly Regex _reservedLabelRegex = new Regex("^__.*$", RegexOptions.Compiled);

        public MetricConfiguration(string name, string help, IReadOnlyList<string> labels, MetricFlags options)
            : base(name)
        {
            Help = help;
            IncludeTimestamp = options.HasFlag(MetricFlags.IncludeTimestamp);
            SuppressEmptySamples = options.HasFlag(MetricFlags.SupressEmptySamples);
            LabelNames = labels ?? Array.Empty<string>();

            if (!_metricNameLabelRegex.IsMatch(Name))
                throw new ArgumentException("Metric name must match regex: " + _metricNameLabelRegex);

            foreach (string labelName in LabelNames)
            {
                if (string.IsNullOrEmpty(labelName))
                    throw new ArgumentException("Label name cannot be empty");

                if (!_metricNameLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Label name must match regex: " + _metricNameLabelRegex);

                if (_reservedLabelRegex.IsMatch(labelName))
                    throw new ArgumentException("Labels starting with double underscore are reserved!");
            }
        }

        public string Help { get; }

        public bool IncludeTimestamp { get; }

        public bool SuppressEmptySamples { get; }

        public IReadOnlyList<string> LabelNames { get; }
    }
}
