using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
