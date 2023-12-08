namespace ParameterInjection;

public sealed class None : IEncoder
{
    public String Encode(String text) => TypeInjection.None.Encode(text);
}

public sealed class UpperCase : IEncoder
{
    public String Encode(String text) => TypeInjection.UpperCase.Encode(text);
}

public sealed class LowerCase : IEncoder
{
    public String Encode(String text) => TypeInjection.LowerCase.Encode(text);
}

public sealed class HomogeniseNewLines : IEncoder
{
    public String Encode(String text) => TypeInjection.HomogeniseNewLines.Encode(text);
}

public sealed class Flatten : IEncoder
{
    public String Encode(String text) => TypeInjection.Flatten.Encode(text);
}

public sealed class Trim : IEncoder
{
    public String Encode(String text) => TypeInjection.Trim.Encode(text);
}

public sealed class Separate : IEncoder
{
    public String Encode(String text) => TypeInjection.Separate.Encode(text);
}

internal sealed class Combo : IEncoder
{
    private readonly IEncoder left, right;
    public Combo(IEncoder left, IEncoder right) => (this.left, this.right) = (left, right);
    public String Encode(String text) => this.right.Encode(this.left.Encode(text));
}
