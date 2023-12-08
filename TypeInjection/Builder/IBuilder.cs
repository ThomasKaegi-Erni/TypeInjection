namespace TypeInjection.Builder;

public interface IBuilder<out TResult>
{
    TResult Build();
}
