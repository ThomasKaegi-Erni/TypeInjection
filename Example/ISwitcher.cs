namespace Example;

public interface ISwitcher
{
    ISwitcher Current { get; }

    Boolean MoveNext();

    Processor Build();
}