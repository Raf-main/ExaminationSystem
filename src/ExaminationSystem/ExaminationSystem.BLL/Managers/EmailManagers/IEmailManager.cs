using ExaminationSystem.BLL.Models;

namespace ExaminationSystem.BLL.Managers.EmailManagers;

public interface IEmailManager
{
    public Task SendEmailAsync(EmailMessage message);
}