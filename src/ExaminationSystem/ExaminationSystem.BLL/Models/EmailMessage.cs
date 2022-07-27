namespace ExaminationSystem.BLL.Models;

public class EmailMessage
{
    public EmailMessage(IEnumerable<string> to, string subject = "", string body = "")
    {
        To = to;
        Subject = subject;
        Body = body;
    }

    public IEnumerable<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}