using System.Net.Sockets;

namespace ParameterInjection.Parametrisation;

public static class Extensions
{
    public static IEnumerable<TimeSpan> Delay(this TimeSpan initial, IExponential exp)
    {
        Int32 retry = -1;
        Double root = initial.TotalSeconds;
        while (++retry < Int32.MaxValue)
        {
            yield return TimeSpan.FromSeconds(root * exp.Exp(retry));
        }
    }

    public static async Task<TcpClient> TryConnect(String url, CancellationToken token)
    {
        var client = new TcpClient("localhost", 80);
        var timeout = TimeSpan.FromMilliseconds(200);
        foreach (var retry in timeout.Delay(new Exponential(basis: 2)))
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
