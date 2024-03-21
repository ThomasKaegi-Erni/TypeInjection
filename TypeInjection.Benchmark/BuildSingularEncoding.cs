using BenchmarkDotNet.Attributes;
using ParameterInjection.Builder;
using TypeInjection.Builder;

using T = TypeInjection;
using P = ParameterInjection;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class BuildSingularEncoding
{
    private readonly ITypeBuilder tBuilder = T.Builder.Builder.With<T.None>();
    private readonly IParameterizedBuilder pBuilder = P.Builder.Builder.With(new P.None());

    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => this.pBuilder.Add(new P.Flatten()).Build();

    [Benchmark]
    public ITextProcessor TypeInjection() => this.tBuilder.Inject<T.Flatten>().Build();
}

/*
// * Summary *

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Parameterised | 12.456 ns | 0.2979 ns | 0.3188 ns |  1.00 |    0.00 | 0.0110 |     104 B |        1.00 |
| TypeInjection |  8.064 ns | 0.2073 ns | 0.1731 ns |  0.64 |    0.03 |      - |         - |        0.00 |
*/