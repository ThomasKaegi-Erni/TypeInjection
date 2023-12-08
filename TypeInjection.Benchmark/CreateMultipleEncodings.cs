using BenchmarkDotNet.Attributes;
using ParameterInjection;

using E = ParameterInjection.Encodings;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser]
public class CreateMultipleEncodings
{

    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised()
    {
        return new TextProcessor(new Combo(E.HomogeniseNewLines, E.Flatten).With(E.Trim).With(E.LowerCase));
    }

    [Benchmark]
    public ITextProcessor TypeInjection()
    {
        return Processor.With<Combo<Combo<Combo<HomogeniseNewLines, Flatten>, Trim>, LowerCase>>();
    }

}

/*
// * Summary *

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean       | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|-------------- |-----------:|----------:|----------:|------:|-------:|----------:|------------:|
| Parameterised | 15.9230 ns | 0.1987 ns | 0.1551 ns |  1.00 | 0.0127 |     120 B |        1.00 |
| TypeInjection |  0.7198 ns | 0.0170 ns | 0.0142 ns |  0.05 |      - |         - |        0.00 |
*/
