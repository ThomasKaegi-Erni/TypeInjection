using BenchmarkDotNet.Attributes;
using TypeInjection.Sharing;

using P = ParameterInjection;
using E = ParameterInjection.Encodings;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class ShareSingleBenchmark
{
    private readonly AllocFreeSerializer tSerializer = AllocFreeSerializer.With<Flatten>();
    private readonly P.Sharing.Serializer pSerializer = new(E.HomogeniseNewLines);

    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => new P.TextProcessor(this.pSerializer.Encoder);

    [Benchmark]
    public ITextProcessor TypeInjection() => this.tSerializer.Inject(TextProcessingCreator.Item);
}

/*
// * Summary *

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean      | Error     | Ratio | Allocated |
|-------------- |----------:|----------:|------:|----------:|
| Parameterised |  3.970 ns | 0.0534 ns |  1.00 |      24 B |
| TypeInjection | 14.458 ns | 0.2106 ns |  3.64 |         - | 
*/