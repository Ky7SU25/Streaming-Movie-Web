﻿namespace StreamingMovie.Application.Interfaces.ExternalServices.Mail;

public class MailContent
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
