// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Rop.Types.Benchmark;

var summary = BenchmarkRunner.Run<ProxyBenchmark>();