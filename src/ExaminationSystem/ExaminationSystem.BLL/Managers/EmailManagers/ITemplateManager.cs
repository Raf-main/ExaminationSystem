namespace ExaminationSystem.BLL.Managers.EmailManagers;

public interface ITemplateManager
{
    Task<string> GetTemplateAsync(string templateName);
    public string ReplaceTemplateProperty(string template, IDictionary<string, string> properties);
}