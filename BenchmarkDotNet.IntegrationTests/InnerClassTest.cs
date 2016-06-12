﻿using BenchmarkDotNet.Jobs;
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.IntegrationTests
{
    // See https://github.com/PerfDotNet/BenchmarkDotNet/issues/55
    // https://github.com/PerfDotNet/BenchmarkDotNet/issues/59 is also related
    [Config(typeof(SingleRunFastConfig))]
    public class InnerClassTest
    {
        private readonly ITestOutputHelper output;

        public InnerClassTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var logger = new OutputLogger(output);
            var config = DefaultConfig.Instance.With(logger);
            BenchmarkTestExecutor.CanExecute<InnerClassTest>(config);

            var testLog = logger.GetLog();
            Assert.Contains("// ### BenchmarkInnerClass method called ###" + Environment.NewLine, testLog);
            Assert.Contains("// ### BenchmarkGenericInnerClass method called ###" + Environment.NewLine, testLog);
            Assert.DoesNotContain("No benchmarks found", logger.GetLog());
        }

        [Benchmark]
        public Tuple<Outer, Outer.Inner> BenchmarkInnerClass()
        {
            Console.WriteLine("// ### BenchmarkInnerClass method called ###");
            return Tuple.Create(new Outer(), new Outer.Inner());
        }

        [Benchmark]
        public Tuple<Outer, Outer.InnerGeneric<string>> BenchmarkGenericInnerClass()
        {
            Console.WriteLine("// ### BenchmarkGenericInnerClass method called ###");
            return Tuple.Create(new Outer(), new Outer.InnerGeneric<string>());
        }
    }

    public class Outer
    {
        public class Inner
        {
        }

        public class InnerGeneric<T>
        {
        }
    }
}
