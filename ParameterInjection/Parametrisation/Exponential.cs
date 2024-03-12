namespace ParameterInjection.Parametrisation;

public sealed class Exponential(Int32 basis) : IExponential
{
    public Double Exp(Double value) => Math.Pow(basis, value);
}
