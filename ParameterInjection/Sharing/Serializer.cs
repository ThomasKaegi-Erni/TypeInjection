using TypeInjection;

namespace ParameterInjection.Sharing;

public sealed class Serializer : ISerialize
{
    public IEncoder Encoder { get; } // It's enough to expose the encoding using a property :-)
    public Serializer(IEncoder encoder) => Encoder = encoder;
    public String Serialize(IEnumerable<String> text) => String.Join(" ", text.Select(Encoder.Encode));
}