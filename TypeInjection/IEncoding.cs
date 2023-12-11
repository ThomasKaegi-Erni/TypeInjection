namespace TypeInjection;

public interface IEncoding
{
    // For the sake of better readability, we're not encoding
    // bytes here as we'd usually do.
    static abstract String Encode(String text);
}
