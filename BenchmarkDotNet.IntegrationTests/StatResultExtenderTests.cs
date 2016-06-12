﻿using System;
using System.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.IntegrationTests
{
    public class StatResultExtenderTests
    {
        private readonly ITestOutputHelper output;

        public StatResultExtenderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var logger = new OutputLogger(output);
            var columns = new[]
            {
                StatisticColumn.StdDev,
                StatisticColumn.Min,
                StatisticColumn.Q1,
                StatisticColumn.Median,
                StatisticColumn.Q3,
                StatisticColumn.Max,
                StatisticColumn.OperationsPerSecond,
                StatisticColumn.P85,
                StatisticColumn.P95,
                StatisticColumn.P95
            };
            var config = DefaultConfig.Instance
                .With(Job.Dry.WithTargetCount(10).WithIterationTime(10))
                .With(logger)
                .With(columns);
            var summary = BenchmarkTestExecutor.CanExecute<Target>(config);

            var table = summary.Table;
            var headerRow = table.FullHeader;
            foreach (var column in columns)
                Assert.True(headerRow.Contains(column.ColumnName));
        }

        [Config(typeof(SingleRunMediumConfig))]
        public class Target
        {
            private readonly Random random = new Random(42);

            [Benchmark]
            public void Main50()
            {
                Thread.Sleep(50 + random.Next(50));
            }

            [Benchmark]
            public void Main100()
            {
                Thread.Sleep(100 + random.Next(50));
            }
        }
    }
}