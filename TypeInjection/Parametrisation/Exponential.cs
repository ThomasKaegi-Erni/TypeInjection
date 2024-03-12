namespace TypeInjection.Parametrisation;

public sealed class Exponential<TBase> : IExponential
    where TBase : ICreate<Int32>
{
    private static readonly Int32 basis = TBase.Create();
    public static Double Exp(Double value) => Math.Pow(basis, value);
}
