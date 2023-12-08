namespace TypeInjection.Builder;

public static class Builder
{
    public static ITypeBuilder With<TEncoding>()
        where TEncoding : IEncoding => Build<TEncoding>.Item;

    private sealed class Build<TEncoding> : ITypeBuilder
        where TEncoding : IEncoding
    {
        public static Build<TEncoding> Item { get; } = new Build<TEncoding>();
        public ITypeBuilder Inject<TEncodingArg>()
            where TEncodingArg : IEncoding => Build<Combo<TEncoding, TEncodingArg>>.Item;
        ITextProcessor IBuilder<ITextProcessor>.Build() => Processor.With<TEncoding>();
    }
}