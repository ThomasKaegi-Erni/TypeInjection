using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance;
config = config.HideColumns("Gen0", "StdDev", "Alloc Ratio", "RatioSD");

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
