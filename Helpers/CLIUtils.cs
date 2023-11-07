using CommandLine.Text;
using Spectre.Console;
using Structor.CLI.Constants;

namespace Structor.CLI.Helpers;

public static class CLIUtils
{
    public static void PrintWelcome()
    {
        var header = new FigletText(Consts.APP_NAME).Color(Color.Green);

        var panel = new Panel(Align.Center(header)).Collapse();

        AnsiConsole.Write(panel);
        Console.WriteLine(HeadingInfo.Default);
    }
}

