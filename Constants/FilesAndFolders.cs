using System.Reflection;

namespace Structor.CLI.Constants;

public static class FilesAndFolders
{
    public const string SOLUTION_FOLDER_NAME = "Structor.Net-main";
    public const string PROJECT_ZIP_NAME = "repository.zip";
    public const string PROJECT_GITHUB_URL = "https://github.com/iMuhammadMustafa/Structor.Net";
    public const string PROJECT_GITHUB_ZIP_URL = $"https://github.com/iMuhammadMustafa/Structor.Net/archive/master.zip";
    public const string SOLUTION_NAME = "Structor.Net";
    public const string PROJECT_NAME = "Structor";

    public const string TEMPLATES_FOLDER_NAME = "Templates";

    private static string CLIPATH = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new Exception("Can't Find Assembly Path");
    public static string TEMPLATES_PATH = Path.Combine(CLIPATH, TEMPLATES_FOLDER_NAME);



    public const string FEATURE_TEMPLATE_ZIP_NAME = "Feature.zip";
    public static string FEATURE_TEMPLATE_ZIP_PATH = Path.Combine(TEMPLATES_PATH, FEATURE_TEMPLATE_ZIP_NAME);


    
    

    private const string ADD_FEATURE_USING_BEFORE = "using Structor.Infrastructure;";
    private const string ADD_FEATURE_USING_TEMPLATE = "using Structor.Features";
    public static string FEATURE_ADD_USING_STATMENT(this string content, string featureName)
        => content.Replace(ADD_FEATURE_USING_BEFORE, $"{ADD_FEATURE_USING_TEMPLATE}.{featureName};{Environment.NewLine}{ADD_FEATURE_USING_BEFORE}");


    private static string FEATURE_ADD_SERVICES_TEMPLATE(string featureName) => $"services.Add{featureName}Services(configuration);";
    
    
    private static string FEATURE_ADD_SERVICES_HEADER = $"public static IServiceCollection AddFeaturesServices(this IServiceCollection services, IConfiguration configuration)";

    public static string FEATURE_ADD_SERVICE_STATMENT(this string content, string featureName)
    {

        int methodStartIndex = content.IndexOf(FEATURE_ADD_SERVICES_HEADER);
        int returnServicesIndex = content.IndexOf("return services;", methodStartIndex);

        if (methodStartIndex != -1 && returnServicesIndex != -1)
        {
            int insertionIndex = returnServicesIndex - 1; // Insert before 'r' in 'return'
            content = content.Insert(insertionIndex, $"{Environment.NewLine}        {FEATURE_ADD_SERVICES_TEMPLATE(featureName)}" +
                                                     $"{Environment.NewLine}       ");
        }
        return content;
    }


    public const string DOMAIN_TEMPLATE_ZIP_NAME = "Domain.zip";
    public static string DOMAIN_TEMPLATE_ZIP_PATH = Path.Combine(TEMPLATES_PATH, DOMAIN_TEMPLATE_ZIP_NAME);
    public const string DOMAIN_TEMPLATE_NAME = "Domain";

    public static string DOMAIN_ADDREPO_STATMENT(string domainName) => $"services.AddScoped<I{domainName}Repository, {domainName}Repository>();";
    public static string DOMAIN_ADDSERVICES_STATMENT(string domainName) => $"services.AddScoped<I{domainName}Services, {domainName}Services>();";

    private static string DOMAIN_ADD_SERVICES_HEADER(string featureName) => $"public static IServiceCollection Add{featureName}Services(this IServiceCollection services, IConfiguration _configuration)";

    public static string DOMAIN_ADD_USING_STATMENT(this string content, string featureName, string domainName)
    => content.Replace("namespace", $"{ADD_FEATURE_USING_TEMPLATE}.{featureName}.Repositories.{domainName}s;{Environment.NewLine}" +
                                    $"{ADD_FEATURE_USING_TEMPLATE}.{featureName}.Services.{domainName}s;");
    public static string DOMAIN_ADD_SERVICE_STATMENT(this string content, string featureName, string domainName)
    {

        int methodStartIndex = content.IndexOf(DOMAIN_ADD_SERVICES_HEADER(featureName));
        int returnServicesIndex = content.IndexOf("return services;", methodStartIndex);

        if (methodStartIndex != -1 && returnServicesIndex != -1)
        {
            int insertionIndex = returnServicesIndex - 1; // Insert before 'r' in 'return'
            content = content.Insert(insertionIndex, $"{Environment.NewLine}        {DOMAIN_ADDREPO_STATMENT(domainName)}" +
                                                     $"{Environment.NewLine}        {DOMAIN_ADDSERVICES_STATMENT(domainName)}" +
                                                     $"{Environment.NewLine}       ");
        }
        return content;
    }


}
