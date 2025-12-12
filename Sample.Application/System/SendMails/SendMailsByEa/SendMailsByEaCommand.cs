using Sample.Common.CQRS.Commands;

namespace Sample.Application.SendMails.SendMailsByEa;

public class SendMailsByEaCommand : CommandBase<string>
{
    public SendMailsByEaRequest Request { get; set; }

    public SendMailsByEaCommand(SendMailsByEaRequest request)
    {
        Request = request;
    }
}