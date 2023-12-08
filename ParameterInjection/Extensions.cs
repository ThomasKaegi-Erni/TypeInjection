namespace ParameterInjection;

public static class Extensions
{
    internal static Combo With(this Combo combo, IEncoder encoder) => new(combo, encoder);
}