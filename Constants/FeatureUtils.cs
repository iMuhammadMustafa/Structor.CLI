using Spectre.Console;
using Structor.CLI.Helpers;

namespace Structor.CLI.Constants;

public static class FeatureUtils
{
    public static string FeatureFolderPath(string directory) => Directory.GetDirectories(directory, Consts.FEATURE_FOLDER_NAME, SearchOption.AllDirectories).First();
    public static string AddServicesMethodName(string featureName) => $"Add{featureName}Services";
    public static void RenameFeatureCollectionName(string path, string featureName)
    {
        path = Path.Combine(path, Consts.FEATURE_TEMPLATE_COLLECTION_NAME);
        FilesUtils.ReplaceTextInFile(path, Consts.FEATURE_TEMPLATE_ADDSERVICES_NAME, AddServicesMethodName(featureName));
    }

    public static string GetCoreFolderPath(string directory) => Directory.GetDirectories(directory, Consts.CORE_FOLDER_NAME, SearchOption.AllDirectories).First();
    public static string GetCoreFoldersPath(string directory) => Directory.GetDirectories(directory, Consts.CORE_FOLDER_NAME, SearchOption.AllDirectories).First();
    public static string GetCoreServicesCollectionsPath(string directory) => Directory.GetFiles(GetCoreFoldersPath(directory), Consts.CORE_SERVICES_COLLECTIONS_NAME, SearchOption.AllDirectories).First();

    public static void AddFeatureCollectionsToCoreServicesCollections(string directory, string featureName)
    {
        var coreFolderPath = GetCoreFolderPath(directory);
        var coreServicesCollectionPath = GetCoreServicesCollectionsPath(directory);
        if (string.IsNullOrWhiteSpace(coreFolderPath))
        {
            AnsiConsole.MarkupLine($"[red]No {coreFolderPath} folder found.[/]");
            AnsiConsole.MarkupLine($"[red]Make sure you are on project root directory.[/]");
            throw new Exception("Core Folder Not Found");
        }
        if (string.IsNullOrWhiteSpace(coreServicesCollectionPath))
        {
            throw new Exception($"{Consts.CORE_SERVICES_COLLECTIONS_NAME} Not Found");
        }

        FileInfo file = new FileInfo(coreServicesCollectionPath);
        string fileContent = File.ReadAllText(file.FullName);


        fileContent = fileContent.FEATURE_ADD_USING_STATMENT(featureName);
        fileContent = fileContent.FEATURE_ADD_SERVICE_STATMENT(featureName);

        File.WriteAllText(file.FullName, fileContent);
    }


}
