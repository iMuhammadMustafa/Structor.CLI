using CommandLine;

namespace Structor.CLI;


[Verb("new", HelpText = "Scaffold new files.")]
public class NewOptions
{
    [Option('p', "project", Required = false, HelpText = "Create a new project.")]
    public bool Project { get; set; }
    [Option('f', "feature", Required = false, HelpText = "Create a new feature.")]
    public bool Feature { get; set; }
    [Option('d', "domain", Required = false, HelpText = "Create a new domain.")]
    public bool Domain { get; set; }
}




