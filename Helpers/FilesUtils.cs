using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Spectre.Console;
using Structor.CLI.Constants;
using System.IO.Compression;
using System.Text;

namespace Structor.CLI.Helpers;

public class FilesUtils
{
    public static void CreateDirectoryIfNotExists(string downloadDirectory)
    {
        if (!Directory.Exists(downloadDirectory))
        {
            Directory.CreateDirectory(downloadDirectory);
        }
    }
    public static async Task<HttpResponseMessage> DownloadFile(string url)
    {
        using HttpClient httpClient = new HttpClient();
        HttpResponseMessage response;
        response = await httpClient.GetAsync(Consts.PROJECT_GITHUB_ZIP_URL);

        return response;
    }
    public static async Task SaveResponseToFile(HttpResponseMessage response, string path)
    {
        using var fs = new FileStream(path, FileMode.Create);

        await response.Content.CopyToAsync(fs);

    }
    public static void UnzipFile(string destinationPath, string zipFilePath, bool deleteAfterUnZip = false)
    {
        if (File.Exists(zipFilePath))
        {
            ZipFile.ExtractToDirectory(zipFilePath, destinationPath);
            if (deleteAfterUnZip)
            {
                File.Delete(zipFilePath);
            }
        }
    }
    public static void RenameDirectory(string path, string oldName, string newName)
    {
        Directory.Move(Path.Combine(path, oldName), Path.Combine(path, newName));
    }
    public static void RenameSubDirectories(string path, string newName, string template)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        DirectoryInfo[] subDirectories = dirInfo.GetDirectories("*", SearchOption.AllDirectories);

        foreach (var directory in subDirectories)
        {
            if (directory.Name.Equals(template))
            {
                var newDirectoryName = directory.Name.Replace(template, newName);
                var newPath = Path.Combine(directory.Parent!.FullName, newDirectoryName);
                directory.MoveTo(newPath);
            }
        }
    }
    public static void RenameFile(FileInfo file, string newName, string oldName)
    {
        var newNamePath = file.Name.Replace(oldName, newName);
        if (file.Name.Contains(oldName))
        {
            file.MoveTo(Path.Combine(file.Directory!.FullName, newNamePath));
        }
    }
    public static void RenameFile(string path, string newName, string oldName)
    {
        var file = new FileInfo(path);
        var newFileName = file.Name.Replace(oldName, newName);
        if (file.Name.Contains(oldName))
        {
            file.MoveTo(Path.Combine(file.Directory!.FullName, newFileName));
        }
    }
    public static void ReplaceTextInFile(string filePath, string oldText, string newText)
    {
        string[] lines = File.ReadAllLines(filePath);
        StringBuilder updatedCode = new StringBuilder();

        foreach (string line in lines)
        {
            string updatedLine = line.Replace(oldText, newText);
            updatedCode.AppendLine(updatedLine);
        }

        File.WriteAllText(filePath, updatedCode.ToString());
    }
    public static void RenameFilesInDirectory(string path, string newName, string oldName)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] filesInDirectory = dirInfo.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (var file in filesInDirectory)
        {
            RenameFile(file, newName, oldName);
        }
    }
    public static void RenameFilesContentInDirectory(string path, string newName, string oldName)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] filesInDirectory = dirInfo.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (var file in filesInDirectory)
        {
            ReplaceFileContent(file, newName, oldName);
        }
    }
    private static void ReplaceFileContent(FileInfo file, string newText, string oldText)
    {
        string fileContent = File.ReadAllText(file.FullName);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        var rewriter = new CodeRewriter(oldText, newText);
        var newRoot = rewriter.Visit(root);

        if (!newRoot.IsEquivalentTo(root))
        {
            File.WriteAllText(file.FullName, newRoot.ToFullString());
        }
    }

}

internal class CodeRewriter : CSharpSyntaxRewriter
    {
        private readonly string oldName;
        private readonly string newName;

        public CodeRewriter(string oldName, string newName)
        {
            this.oldName = oldName;
            this.newName = newName;
        }

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            if (token.Text == oldName)
            {
                return SyntaxFactory.Identifier(newName);
            }
            return base.VisitToken(token);
        }
    }