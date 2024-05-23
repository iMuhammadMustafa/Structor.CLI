using CommandLine;
using Spectre.Console;
using Structor.CLI;
using Structor.CLI.Creation;
using Structor.CLI.Helpers;

CLIUtils.PrintWelcome();



await Parser.Default.ParseArguments<NewOptions>(args)
    .MapResult
    (
        async (NewOptions o) =>
        {
            await Create(o);
        },
        errors => Task.FromResult(1)
    );

async Task<int> Create(NewOptions opts)
{
    string name = string.Empty;
    string directory = string.Empty;
    if (opts.Project)
    {
        name = AnsiConsole.Ask<string>($"[green]Project Name[/]?");
        await CreationHelpers.HandleProjectCreation(name, directory);
        return await Repeat();
    }
    if (opts.Feature)
    {
        name = AnsiConsole.Ask<string>($"[green]Feature Name[/]?");
        await CreationHelpers.HandleFeatureCreation(name, directory);
        return await Repeat();
    }
    if (opts.Domain)
    {
        name = AnsiConsole.Ask<string>($"[green]Domain Name[/]?");
        await CreationHelpers.HandleDomainCreation(name, directory);
        return await Repeat();
    }

    return await Pick();
}

static async Task<int> Pick()
{
    var type = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("What to create?")
                                .PageSize(3)
                                .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
                                .AddChoices(new[] {
                                        "Project", "Feature", "Domain"
                                }));
    var name = AnsiConsole.Ask<string>($"[green]{type} Name[/]?");
    var directory = AnsiConsole.Prompt(new TextPrompt<string>("[grey][[Optional]][/] What is the [green]Project Directory[/]?").AllowEmpty());
    if (string.IsNullOrWhiteSpace(directory))
    {
        directory = Environment.CurrentDirectory;
    }

    if (type == "Project")
    {
        await CreationHelpers.HandleProjectCreation(name, directory);
    }
    if (type == "Feature")
    {
        await CreationHelpers.HandleFeatureCreation(name, directory);
    }
    if (type == "Domain")
    {
        await CreationHelpers.HandleDomainCreation(name, directory);
    }
    return await Repeat();
}

static async Task<int> Repeat()
{
    var repeat = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Create Something else?")
                                .PageSize(3)
                                .AddChoices(new[] {
                                        "Yes", "No"
                                }));
    if (repeat == "Yes")
    {
        await Pick();
    }

    return 0;
}