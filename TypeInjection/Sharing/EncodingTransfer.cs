namespace TypeInjection.Sharing;

// this is allocation free :-)
public sealed class TextProcessingCreator : IEncodingInjector<ITextProcessor>
{
    public static TextProcessingCreator Item { get; } = new TextProcessingCreator();
    private TextProcessingCreator() { }
    public ITextProcessor Inject<TEncoding>()
        where TEncoding : IEncoding => Processor.With<TEncoding>();
}


// thi is also allocation free :-)
public sealed class ComboProcessingCreator : IEncodingInjector<IEncodingInjector<ITextProcessor>> // ugly nested interface...
{
    public static ComboProcessingCreator Item { get; } = new ComboProcessingCreator();
    private ComboProcessingCreator() { }
    public IEncodingInjector<ITextProcessor> Inject<TEncoding>()
         where TEncoding : IEncoding => ComboCreator<TEncoding>.Item;

    // Disadvantage that we "need" a nested class to implement this
    private sealed class ComboCreator<TLeftEncoding> : IEncodingInjector<ITextProcessor>
        where TLeftEncoding : IEncoding
    {
        public static ComboCreator<TLeftEncoding> Item { get; } = new ComboCreator<TLeftEncoding>();
        public ITextProcessor Inject<TEncoding>() where TEncoding : IEncoding
        {
            return Processor.With<Combo<TLeftEncoding, TEncoding>>();
        }
    }
}