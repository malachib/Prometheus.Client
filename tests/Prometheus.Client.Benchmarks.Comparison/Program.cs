using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;

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

            BenchmarkRunner.Run<MetricCreationBenchmarks>(
                DefaultConfig.Instance
                    .With(Job.Default.With(CsProjCoreToolchain.NetCoreApp30)));
            //BenchmarkRunner.Run<SerializationBenchmarks>();
            //BenchmarkRunner.Run<LabelBenchmarks>();
            //BenchmarkRunner.Run<HttpExporterBenchmarks>();
            //BenchmarkRunner.Run<SummaryBenchmarks>();
        }
    }
}
