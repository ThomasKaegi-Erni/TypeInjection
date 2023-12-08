using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using TypeInjection.Sharing;
using ParameterInjection;

using P = ParameterInjection;
using E = ParameterInjection.Encodings;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser]
[HideColumns("Category")]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class SharingBenchmark
{
    private const String combo = "Combo";
    private const String singular = "Singular";
    private readonly Serializer tSerializer = Serializer.With<HomogeniseNewLines>();
    private readonly AllocFreeSerializer tFreeSerializer = AllocFreeSerializer.With<Flatten>();

    private readonly P.Sharing.Serializer pSerializerA = new(E.HomogeniseNewLines);
    private readonly P.Sharing.Serializer pSerializerB = new(E.Flatten);

    [BenchmarkCategory(singular), Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => new P.TextProcessor(pSerializerA.Encoder);

    [BenchmarkCategory(combo), Benchmark(Baseline = true)]
    public ITextProcessor ParameterisedCombo() => new P.TextProcessor(new Combo(pSerializerA.Encoder, pSerializerB.Encoder));

    [BenchmarkCategory(singular), Benchmark]
    public ITextProcessor TypeInjectionProperty() => this.tSerializer.Injector.Inject(TextProcessingCreator.Item);

    [BenchmarkCategory(singular), Benchmark]
    public ITextProcessor TypeInjectionInstance() => this.tFreeSerializer.Inject(TextProcessingCreator.Item);

    [BenchmarkCategory(combo), Benchmark]
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


| Method                | Mean      | Error     | Ratio | Allocated | 
|---------------------- |----------:|----------:|------:|----------:|-
| ParameterisedCombo    |  7.631 ns | 0.1968 ns |  1.00 |      56 B |
| TypeInjectionCombo    | 28.123 ns | 0.3949 ns |  3.66 |         - | 
|                       |           |           |       |           | 
| Parameterised         |  4.006 ns | 0.0861 ns |  1.00 |      24 B | 
| TypeInjectionProperty | 14.953 ns | 0.1842 ns |  3.73 |         - | 
| TypeInjectionInstance | 14.572 ns | 0.1489 ns |  3.64 |         - | 
*/