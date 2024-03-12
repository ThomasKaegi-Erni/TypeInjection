
using ParameterInjection.Parametrisation;
using TypeInjection.Parametrisation;

namespace TypeInjection.Test;

// Note: These tests are rather quick and dirty. They're mostly here to illustrate that the two paradigms do the same...
public class ParametrisationTest
{
    private const Int32 retries = 5;
    private static readonly TimeSpan initial = TimeSpan.FromSeconds(1);
    private static readonly IEnumerable<TimeSpan> expectedDelays = Enumerable.Range(0, retries).Select(i => TimeSpan.FromSeconds(Math.Pow(2, i))).ToArray();

    [Fact]
    public void ClassicParametrisation()
    {
        var exp = new Exponential(basis: 2);

        var actual = initial.Delay(exp).Take(retries);

        Assert.Equal(expectedDelays, actual);
    }

    [Fact]
    public void InjectedParametrisation()
    {
        var actual = initial.Delay<Exponential<Two>>().Take(retries);

        Assert.Equal(expectedDelays, actual);
    }
}

file sealed class Two : ICreate<Int32>
{
    public static Int32 Create() => 2;
}