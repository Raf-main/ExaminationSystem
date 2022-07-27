using ExaminationSystem.BLL.Managers.EmailManagers.Options;
using ExaminationSystem.BLL.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ExaminationSystem.BLL.Managers.EmailManagers;

internal class EmailManager : IEmailManager
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailManager> _logger;

    public EmailManager(IOptions<EmailOptions> emailOptions, ILogger<EmailManager> logger)
    {
        _logger = logger;
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        var emailMessage = CreateEmail(message);
        await SendEmailAsync(emailMessage);
    }

    public async Task SendEmailAsync(MimeMessage emailMessage)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, true);
            await client.AuthenticateAsync(_emailOptions.From, _emailOptions.Password);
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    private MimeMessage CreateEmail(EmailMessage message)
    {
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Body
        };

        var email = new MimeMessage
        {
            Subject = message.Subject,
            Body = bodyBuilder.ToMessageBody()
        };

        email.From.Add(new MailboxAddress(_emailOptions.UserName, _emailOptions.From));
        email.To.AddRange(message.To.Select(to => new MailboxAddress(to.Split("@")[0], to)));

        return email;
    }
}