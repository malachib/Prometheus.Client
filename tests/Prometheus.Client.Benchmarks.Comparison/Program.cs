using BenchmarkDotNet.Running;

namespace Prometheus.Client.Benchmarks.Comparison
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var b = new MetricCreationBenchmarks();
            //b.Setup();
            //b.Counter_Many();
            //b.Setup();
            //System.Threading.Thread.Sleep(5000);
            //System.Console.ReadLine();

            //b.Counter_Many();
            //System.Console.ReadLine();

            BenchmarkRunner.Run<MetricCreationBenchmarks>();
            //BenchmarkRunner.Run<SerializationBenchmarks>();
            //BenchmarkRunner.Run<LabelBenchmarks>();
            //BenchmarkRunner.Run<HttpExporterBenchmarks>();
            //BenchmarkRunner.Run<SummaryBenchmarks>();
        }
    }
}
