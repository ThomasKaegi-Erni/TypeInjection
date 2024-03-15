using System.Collections;

namespace TypeInjection.Encoding;

public sealed class Message(String message)
{
    public override String ToString() => message;
    public static implicit operator String(Message message) => message.ToString();
}