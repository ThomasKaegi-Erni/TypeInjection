using BenchmarkDotNet.Attributes;
using ParameterInjection.Builder;
using TypeInjection.Builder;

using T = TypeInjection;
using P = ParameterInjection;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser]
public class BuildMultipleEncodings
{
    private readonly ITypeBuilder tBuilder = T.Builder.Builder.With<T.None>();
    private readonly IParameterizedBuilder pBuilder = P.Builder.Builder.With(new P.None());

    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised()
    {
        return this.pBuilder.Add(new P.HomogeniseNewLines()).Add(new P.Flatten()).Add(new P.Trim()).Add(new P.LowerCase()).Build();
    }

    [Benchmark]
    public ITextProcessor TypeInjection()
    {
        return this.tBuilder.Inject<T.HomogeniseNewLines>().Inject<T.Flatten>().Inject<T.Trim>().Inject<T.LowerCase>().Build();
    }

}

/*
// * Summary *

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parameterised | 38.04 ns | 0.658 ns | 0.583 ns |  1.00 |    0.00 | 0.0365 |     344 B |        1.00 |
| TypeInjection | 34.97 ns | 0.640 ns | 0.534 ns |  0.92 |    0.02 |      - |         - |        0.00 |
*/
