using Sample.Common.CQRS.Commands;
using System.Net;
using System.Net.Mail;
namespace Sample.Application.SendMails.SendMailsByEa;

public class SendMailsByEaCommandHandler : CommandHandlerBase<SendMailsByEaCommand, string>
{
    public SendMailsByEaCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override Task<string> CommandHandle(SendMailsByEaCommand command, CancellationToken cancellationToken)
    {
        string fromEmail = _configuration["EmailsSettings:FromAddressEmail"];
        string fromPassword = _configuration["EmailsSettings:FromAddressEmailPassword"];

        MailMessage mail = new()
        {
            From = new MailAddress(fromEmail, "Trung tâm dịch vụ việc làm")
        };
        mail.To.Add(command.Request.ToEmail);
        mail.Subject = command.Request.Subject;
        mail.Body = command.Request.TextBody;
        mail.IsBodyHtml = true;

        SmtpClient smtpClient = new ("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(fromEmail, fromPassword),
            EnableSsl = true
        };

        try
        {
            smtpClient.Send(mail);
            return Task.FromResult("Email sent successfully.");
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }
}