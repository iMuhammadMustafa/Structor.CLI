using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Spectre.Console;
using System.IO.Compression;
using System.IO;
using System.Xml.Linq;
using System.Text;
using Structor.CLI.Constants;

namespace Structor.CLI.Helpers;

public static class FilesUtilsX
{








    public static void UnZipFeatureTemplate(string featureFolder, string featureName)
    {
        UnzipFile(featureFolder, FilesAndFolders.FEATURE_TEMPLATE_ZIP_PATH);
        Directory.Move($"{featureFolder}/{FilesAndFolders.FEATURE_TEMPLATE_NAME}", $"{featureFolder}/{featureName}");
    }
    public static void UnZipDomainTemplate(string featurePath)
    {
        UnzipFile(featurePath, FilesAndFolders.DOMAIN_TEMPLATE_ZIP_PATH);
    }



    public static void RenameProjectFolder(string path, string newName)
    {
        path = Path.Combine(path, newName);
        RenameDirectory(path, FilesAndFolders.PROJECT_NAME, newName);
    }






    public static void UpdateNamespaces(string path, string newName, string oldName)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] filesInDirectory = dirInfo.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (var file in filesInDirectory)
        {
            ReplaceFileContent(file, newName, oldName);
        }
    }

    public static void RenameProjectFile(string path, string newName)
    {
        var projPath = Path.Combine(path, newName, newName, $"{FilesAndFolders.PROJECT_NAME}.csproj");
        RenameFile(projPath, newName, FilesAndFolders.PROJECT_NAME);
    }





    public static void AddDomainToServicesCollections(string directory, string featureName, string domainName)
    {
        //directory = Path.Combine(directory, "Features" ,featureName);
        var servicesCollections = Directory.GetFiles(directory, $"{featureName}ServicesCollection.cs").First();
        if (string.IsNullOrWhiteSpace(servicesCollections))
        {
            AnsiConsole.MarkupLine($"[red]No ServicesCollections file found.[/]");
            AnsiConsole.MarkupLine($"[red]Make sure you are on project root directory.[/]");
            throw new Exception("Feature Services Collection Not Found");
        }


        FileInfo file = new FileInfo(servicesCollections);
        string fileContent = File.ReadAllText(file.FullName);


        fileContent = fileContent.DOMAIN_ADD_USING_STATMENT(featureName, domainName);
        fileContent = fileContent.DOMAIN_ADD_SERVICE_STATMENT(featureName, domainName);

        File.WriteAllText(file.FullName, fileContent);
    }
}
