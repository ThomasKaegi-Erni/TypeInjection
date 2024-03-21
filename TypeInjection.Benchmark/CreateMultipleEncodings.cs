using BenchmarkDotNet.Attributes;
using ParameterInjection;

using P = ParameterInjection;
using E = ParameterInjection.Encodings;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class CreateMultipleEncodings
{
    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised()
    {
        // Benefit of the doubt: Use static encodings with the multitude of Combo classes.
        return new TextProcessor(new Combo(new Combo(new Combo(E.HomogeniseNewLines, E.Flatten), E.Trim), E.LowerCase));
    }

    [Benchmark]
    public ITextProcessor TypeInjection()
    {
        return Processor.With<Combo<Combo<Combo<HomogeniseNewLines, Flatten>, Trim>, LowerCase>>();
    }

    [Benchmark]
    public ITextProcessor ParameterisedWorstCase()
    {
        // Worst case scenario: new-up all parameterised parameters.
        return new TextProcessor(new Combo(new Combo(new Combo(new P.HomogeniseNewLines(), new P.Flatten()), new P.Trim()), new P.LowerCase()));
    }
}

/*
// * Summary *

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.103
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


| Method                 | Mean       | Error     | Ratio | Allocated |
|----------------------- |-----------:|----------:|------:|----------:|-
| Parameterised          | 15.4586 ns | 0.1619 ns |  1.00 |     120 B |
| TypeInjection          |  0.8172 ns | 0.0457 ns |  0.05 |         - |
| ParameterisedWorstCase | 21.9172 ns | 0.1261 ns |  1.42 |     216 B |
*/
