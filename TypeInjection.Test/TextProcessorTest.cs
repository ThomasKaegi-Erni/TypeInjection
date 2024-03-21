namespace TypeInjection.Test;

public class TextProcessorTest
{
    [Fact]
    public void TextProcessorsOfSameEncodingAreTheSameInstance()
    {
        var firstInstance = Processor.With<Flatten>();
        var secondInstance = Processor.With<Flatten>();

        Assert.Same(firstInstance, secondInstance);
    }

    [Fact]
    public void TextProcessorOfDifferingEncodingAreDistinctInstances()
    {
        var firstInstance = Processor.With<Trim>();
        var secondInstance = Processor.With<Flatten>();

        Assert.NotSame(firstInstance, secondInstance);
    }
}
