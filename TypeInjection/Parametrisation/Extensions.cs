using System.Net.Sockets;

namespace TypeInjection.Parametrisation;

public static class Extensions
{
    public static IEnumerable<TimeSpan> Delay<TExp>(this TimeSpan initial)
        where TExp : IExponential
    {
        Int32 retry = -1;
        Double root = initial.TotalSeconds;
        while (++retry < Int32.MaxValue)
        {
            yield return TimeSpan.FromSeconds(root * TExp.Exp(retry));
        }
    }

    public static async Task<TcpClient> TryConnect(String url, CancellationToken token)
    {
        var client = new TcpClient("localhost", 80);
        var timeout = TimeSpan.FromMilliseconds(200);
        foreach (var retry in timeout.Delay<Exponential<Two>>())
        {
            try
            {
                await client.ConnectAsync(url, 80, token);
                return client;
            }
            catch
            {
                await Task.Delay(retry, token);
            }
        }

        throw new Exception("Failed connecting");
    }
}

file sealed class Two : ICreate<Int32>
{
    public static Int32 Create() => 2;
}
