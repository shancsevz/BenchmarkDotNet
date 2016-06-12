using BenchmarkDotNet.Jobs;
using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.IntegrationTests
{
    [Config(typeof(SingleRunFastConfig))]
    public class SetupAttributeTest
    {
        private readonly ITestOutputHelper output;

        public SetupAttributeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var logger = new OutputLogger(output);
            var config = DefaultConfig.Instance.With(logger);
            BenchmarkTestExecutor.CanExecute<SetupAttributeTest>(config);
            Assert.Contains("// ### Setup called ###" + Environment.NewLine, logger.GetLog());
        }

        [Setup]
        public void Setup()
        {
            Console.WriteLine("// ### Setup called ###");
        }

        [Benchmark]
        public void Benchmark()
        {
            Console.WriteLine("// ### Benchmark called ###");
            Thread.Sleep(5);
        }
    }
}
