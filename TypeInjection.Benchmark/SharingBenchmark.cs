using BenchmarkDotNet.Attributes;
using ParameterInjection.Builder;
using TypeInjection.Builder;

using T = TypeInjection;
using P = ParameterInjection;
using TypeInjection.Sharing;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser]
public class SharingBenchmark
{
    private readonly Serializer tSerializer = Serializer.With<T.HomogeniseNewLines>();
    private readonly AllocFreeSerializer tFreeSerializer = AllocFreeSerializer.With<T.Flatten>();
    /*
    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => this.pBuilder.Add(new P.Flatten()).Build();
    */

    [Benchmark]
    public ITextProcessor TypeInjectionProperty() => this.tSerializer.Injector.Inject(TextProcessingCreator.Item);

    [Benchmark]
    public ITextProcessor TypeInjectionInstance() => this.tFreeSerializer.Inject(TextProcessingCreator.Item);

    [Benchmark]
    public ITextProcessor TypeInjectionCombo()
    {
        var rightInjector = this.tFreeSerializer.Inject(ComboProcessingCreator.Item);
        return this.tSerializer.Injector.Inject(rightInjector);
    }
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