// See https://aka.ms/new-console-template for more information
using Example;
using TypeInjection;
using TypeInjection.Builder;

Console.WriteLine("Hello, World!");
var text = new[] { " This is \n some", " WEIrd \t text. Full    ", "of whitespace    ", "and \n \r\n newLine Characters.    " };
var textProcessor = new Processor(Builder.With<None>().Build(), Delimitations.None);
var unprocessedText = textProcessor.Process(text);

ISwitcher switcher = new EncodingSwitcher();
while (switcher.MoveNext())
{
    switcher = switcher.Current;
}
textProcessor = switcher.Build();

var processedText = textProcessor.Process(text);

Console.WriteLine();
Console.WriteLine(" --- original vs. processed text ---");
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine(unprocessedText);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(processedText);
Console.ResetColor();
