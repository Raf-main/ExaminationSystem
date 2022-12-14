using System.Text;
using ExaminationSystem.BLL.Managers.FileManagers;
using Microsoft.Extensions.Caching.Memory;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ExaminationSystem.BLL.Managers.EmailManagers;

internal class TemplateManager : ITemplateManager
{
    private readonly IFileManager _fileManager;
    private readonly IMemoryCache _memoryCache;
    private readonly string _templatesPath;

    public TemplateManager(IFileManager fileManager, IMemoryCache memoryCache, IHostingEnvironment env)
    {
        _fileManager = fileManager;
        _memoryCache = memoryCache;
        _templatesPath = Path.Combine(env.WebRootPath, "Templates");
    }

    public async Task<string> GetTemplateAsync(string templateName)
    {
        string template;

        if ((template = _memoryCache.Get<string>(templateName)) != null)
        {
            return template;
        }

        using var reader = _fileManager.CreateTextReader(Path.Combine(_templatesPath, templateName));

        template = await reader.ReadToEndAsync();
        _memoryCache.Set(templateName, template, TimeSpan.FromHours(48));

        return template;
    }

    public string ReplaceTemplateProperty(string template, IDictionary<string, string> properties)
    {
        if (string.IsNullOrEmpty(template))
        {
            return template;
        }

        var templateBuilder = new StringBuilder(template);

        foreach (var (key, value) in properties)
        {
            templateBuilder.Replace($"{{{key}}}", value);
        }

        return templateBuilder.ToString();
    }
}