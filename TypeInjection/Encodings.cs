using System.Text;

namespace TypeInjection;

public sealed class None : IEncoding
{
    public static String Encode(String text) => text;
}

public sealed class UpperCase : IEncoding
{
    public static String Encode(String text) => text.ToUpperInvariant();
}

public sealed class LowerCase : IEncoding
{
    public static String Encode(String text) => text.ToLowerInvariant();
}

public sealed class HomogeniseNewLines : IEncoding
{
    public static String Encode(String text) => text.ReplaceLineEndings();
}

public sealed class Flatten : IEncoding
{
    public static String Encode(String text) => text.Replace(Environment.NewLine, String.Empty);
}

public sealed class Trim : IEncoding
{
    public static String Encode(String text) => String.Join(" ", text.Split(" ").Where(s => s.Length > 0));
}

public sealed class Separate : IEncoding
{
    public static String Encode(String text)
    {
        if (text.Length <= 1)
        {
            return text;
        }
        Char prev = text[0];
        var builder = new StringBuilder(text.Length).Append(prev);
        foreach (var character in text[1..])
        {
            var separate = Char.IsLower(prev) && Char.IsUpper(character);
            builder = separate ? builder.Append(' ').Append(Char.ToLowerInvariant(character)) : builder.Append(character);
            prev = character;
        }
        return builder.ToString();
    }
}

public sealed class Combo<TLeft, TRight> : IEncoding
    where TLeft : IEncoding
    where TRight : IEncoding
{
    public static String Encode(String text) => TRight.Encode(TLeft.Encode(text));
}
