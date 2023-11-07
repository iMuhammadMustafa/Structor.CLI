using Spectre.Console;
using Structor.CLI.Constants;
using Structor.CLI.Helpers;

namespace Structor.CLI.Creation;

public static class CreationHelpers
{
    public static async Task HandleProjectCreation(string name, string directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
        {
            directory = Environment.CurrentDirectory;
        }
        FilesUtils.CreateDirectoryIfNotExists(directory);

        AnsiConsole.MarkupLine("\nCloning Repository...");
        await AnsiConsole.Status()
                         .Spinner(Spinner.Known.Aesthetic)
                         .SpinnerStyle(Style.Parse("green bold"))
            .StartAsync("Loading...", async ctx =>
            {

                ctx.Status("Cloning Repository...");
                Thread.Sleep(500);
                
                var response = await FilesUtils.DownloadFile(Consts.PROJECT_GITHUB_ZIP_URL);
                
                AnsiConsole.MarkupLine("Cloned Repository.");


                if (response.IsSuccessStatusCode)
                {
                    ctx.Status("Downloading...");
                    Thread.Sleep(500);
                    
                    var zipPath = Path.Combine(directory, Consts.PROJECT_ZIP_NAME);
                    await FilesUtils.SaveResponseToFile(response, zipPath);
                    
                    AnsiConsole.MarkupLine("Repository downloaded successfully.");

                    ctx.Status("Unzipping...");
                    
                    Thread.Sleep(500);
                    FilesUtils.UnzipFile(directory, zipPath, true);
                    
                    AnsiConsole.MarkupLine("Unzipped Files Successfuly.");



                    GenerationUtils.RenameSolutionFolder(directory, name);

                    GenerationUtils.RenameSolutionFile(directory, name);


                    ctx.Status("Finishing Up...");
                    Thread.Sleep(500);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Failed to download the repository.[/]");
                }
            });

    }


}