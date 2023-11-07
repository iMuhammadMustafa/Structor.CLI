using CommandLine.Text;
using Spectre.Console;

namespace Structor.CLI;

public static class CLI
{
    public static void PrintWelcome()
    {
        var header = new FigletText("Structor CLI").Color(Color.Green);

        var panel = new Panel(Align.Center(header)).Collapse();

        AnsiConsole.Write(panel);
        Console.WriteLine(HeadingInfo.Default);
    }
}

