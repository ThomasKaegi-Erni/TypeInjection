namespace TypeInjection;

public interface ICreate<out T>
{
    static abstract T Create();
}
