using SysEncoding = System.Text.Encoding;

namespace TypeInjection.Encoding;

public interface IEncoding
{
    static abstract Byte[] Encode(String text);
    static abstract String Decode(IEnumerable<Byte> data);
}

public sealed class Utf8 : IEncoding
{
    static readonly SysEncoding encoder = SysEncoding.UTF8;
    public static String Decode(IEnumerable<Byte> data) => encoder.GetString(data.ToArray());
    public static Byte[] Encode(String text) => encoder.GetBytes(text);
}

public sealed class Ascii : IEncoding
{
    static readonly SysEncoding encoder = SysEncoding.ASCII;
    public static String Decode(IEnumerable<Byte> data) => encoder.GetString(data.ToArray());
    public static Byte[] Encode(String text) => encoder.GetBytes(text);
}
