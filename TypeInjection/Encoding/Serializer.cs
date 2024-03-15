namespace TypeInjection.Encoding;


public interface ISerializer
{
    IEnumerable<Byte> Serialize(Object item);
    TResult Deserialize<TResult>(IEnumerable<Byte> stream);
}

public sealed class Serializer<TEncoding> : ISerializer
    where TEncoding : IEncoding
{
    public TResult Deserialize<TResult>(IEnumerable<Byte> stream)
    {
        String text = TEncoding.Decode(stream);
        throw new NotImplementedException("Deserialization part...");
    }

    public IEnumerable<Byte> Serialize(Object item)
    {
        String deserializedObject = item.ToString() ?? "We'd use actual serialization here.";
        Byte[] rawData = TEncoding.Encode(deserializedObject);
        return rawData;
    }
}

file static class Example
{
    public static ISerializer Utf8Encoding()
    {
        ISerializer serializer = new Serializer<Utf8>();
        return serializer;
    }
    public static ISerializer AsciiEncoding()
    {
        ISerializer serializer = new Serializer<Ascii>();
        return serializer;
    }
}
