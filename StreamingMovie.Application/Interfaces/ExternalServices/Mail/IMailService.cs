namespace StreamingMovie.Application.Interfaces.ExternalServices.Mail;

public interface IMailService
{
    Task SendMailAsync(MailContent mailContent);
}
