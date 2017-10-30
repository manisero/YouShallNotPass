using BenchmarkDotNet.Running;
using Manisero.YouShallNotPass.Benchmarks.Core;

namespace Manisero.YouShallNotPass.Benchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<GetCustomAttribute_vs_cache>();
        }
    }
}
