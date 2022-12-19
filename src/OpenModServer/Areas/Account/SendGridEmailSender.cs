using Microsoft.AspNetCore.Identity.UI.Services;
using OpenModServer.Structures;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OpenModServer.Areas.Account;

public class SendGridEmailSender : IEmailSender
{
    private readonly ILogger<SendGridEmailSender> _logger;
    private readonly SendGridClient _sendGridClient;
    private readonly OmsConfig _config;

    public SendGridEmailSender(OmsConfig config, ILogger<SendGridEmailSender> logger)
    {
        _logger = logger;
        _config = config;

        if (string.IsNullOrWhiteSpace(_config.Keys.SendGrid))
        {
            throw new InvalidOperationException(
                "The SendGrid email sender was configured, but there is no token. " +
                "Please fill in the Keys.SendGrid configuration value.");
        }
        
        _sendGridClient = new SendGridClient(_config.Keys.SendGrid);
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress(_config.Email.FromAddress, _config.Email.FromName),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = htmlMessage
        };
        message.AddTo(new EmailAddress(email));

        var response = await _sendGridClient.SendEmailAsync(message);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email queued successfully.");
        } else _logger.LogError("Failed to send email.");
    }
}