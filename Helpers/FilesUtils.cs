using Spectre.Console;
using Structor.CLI.Constants;
using System.IO.Compression;

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
}
