namespace Sample.Application.SendMails.SendMailsByEa;

public class SendMailsByEaRequest
{
    public string? ToEmail { get; set; }
    public string? Subject { get; set; }
    public string? TextBody { get; set; }
}