using Structor.CLI.Constants;

namespace Structor.CLI.Helpers;

public static class GenerationUtils
{
    public static void RenameSolutionFolder(string path, string newName)
    {
        FilesUtils.RenameDirectory(path, Consts.SOLUTION_FOLDER_NAME, newName);
    }
    public static void RenameSolutionFile(string path, string newName)
    {
        var slnPath = Path.Combine(path, newName, $"{Consts.SOLUTION_NAME}.sln");
        FilesUtils.RenameFile(slnPath, newName, Consts.SOLUTION_NAME);
    }
}
