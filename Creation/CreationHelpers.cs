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

    public static async Task HandleFeatureCreation(string name, string directory)
    {
        var featuresPath = FeatureUtils.FeatureFolderPath(directory);

        if (string.IsNullOrWhiteSpace(featuresPath))
        {
            AnsiConsole.MarkupLine($"[red]No features folder found.[/]");
            AnsiConsole.MarkupLine($"[red]Make sure you are on project root directory.[/]");
            throw new Exception("Features folder does not exist");
        }

        AnsiConsole.MarkupLine(string.Empty);
        AnsiConsole.Status()
                         .Spinner(Spinner.Known.Aesthetic)
                         .SpinnerStyle(Style.Parse("green bold"))
            .Start("Loading...", ctx =>
            {
                ctx.Status("Creating Feature Folder...");
                Thread.Sleep(500);

                GenerationUtils.UnzipFeatureTemplate(featuresPath, name);
                
                AnsiConsole.MarkupLine("Feature Folder Created.");

                ctx.Status("Creating Sub Directories...");
                Thread.Sleep(500);

                var newFeaturePath = Path.Combine(featuresPath, name);
                FilesUtils.RenameSubDirectories(name, newFeaturePath, Consts.FEATURE_TEMPLATE_NAME);
                
                AnsiConsole.MarkupLine("Sub Directories Created.");

                ctx.Status("Creating Files...");
                Thread.Sleep(500);

                FeatureUtils.RenameFeatureCollectionName(newFeaturePath, name);
                FilesUtils.RenameFilesInDirectory(newFeaturePath, name, Consts.FEATURE_TEMPLATE_NAME);
                FilesUtils.RenameFilesContentInDirectory(newFeaturePath, name, Consts.FEATURE_TEMPLATE_NAME);
                
                AnsiConsole.MarkupLine("Files Created.");

                ctx.Status("Adding Feature to Core Services Collection...");
                Thread.Sleep(500);

                FeatureUtils.AddFeatureCollectionsToCoreServicesCollections(directory, name);
                AnsiConsole.MarkupLine("Feature Added to Core Services Collection.");

                ctx.Status("Finishing Up...");
                Thread.Sleep(500);
            });




    }


}