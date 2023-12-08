

using TypeInjection.Builder;

namespace TypeInjection.Sharing;

// Sealed, but not allocation free!
public sealed class Serializer : ISerialize
{
    private readonly Func<String, String> encoding;
    public IInject Injector { get; } // Notice how we can expose Injection via (allocation free) property.
    private Serializer(Func<String, String> encoding, IInject inject) => (this.encoding, Injector) = (encoding, inject);
    public static Serializer With<TEncoding>()
        where TEncoding : IEncoding => new(TEncoding.Encode, Inject<TEncoding>.Item);

    // note how the act of encoding can be captured via a function.
    public String Serialize(IEnumerable<String> text) => String.Join(" ", text.Select(this.encoding));
}

// Allocation free, but not sealed...
// notice how both interfaces need to be implemented on the serializer itself.
public abstract class AllocFreeSerializer : ISerialize, IInject
{
    private AllocFreeSerializer() { } // prevent malicious inheritance
    public static AllocFreeSerializer With<TEncoding>()
        where TEncoding : IEncoding => Impl<TEncoding>.Item;
    public abstract TResult Inject<TResult>(IEncodingInjector<TResult> injector);
    public abstract String Serialize(IEnumerable<String> text);

    private sealed class Impl<TEncoding> : AllocFreeSerializer
        where TEncoding : IEncoding
    {
        public static AllocFreeSerializer Item { get; } = new Impl<TEncoding>();
        public override TResult Inject<TResult>(IEncodingInjector<TResult> injector) => injector.Inject<TEncoding>();
        public override String Serialize(IEnumerable<String> text) => String.Join(" ", text.Select(TEncoding.Encode));
    }
}
