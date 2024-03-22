using BenchmarkDotNet.Attributes;

using T = TypeInjection;
using P = ParameterInjection;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class CreateSingularEncodings
{
    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => new P.TextProcessor(new P.LowerCase());

    [Benchmark]
    public ITextProcessor TypeInjection() => T.Processor.With<T.LowerCase>();
}

/*
// * Summary *

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.103
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


| Method        | Mean      | Error     | Ratio | Allocated |
|-------------- |----------:|----------:|------:|----------:|-
| Parameterised | 6.0004 ns | 0.0771 ns |  1.00 |      48 B | 
| TypeInjection | 0.8203 ns | 0.0535 ns |  0.14 |         - |
*/