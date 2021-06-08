using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using sg = SendGrid;
using sgMail = SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fargs.Portal.Services.Email
{
    public class SendGrid : IEmailService
    {
        private readonly ILogger<SendGrid> _logger;
        public SendGrid(IOptions<SendGridOptions> optionsAccessor, ILogger<SendGrid> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public SendGridOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task SendEmailAsync(MailMessage message)
        {
            throw new NotSupportedException("This implementation is not supported using SendGrid.");
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new sg.SendGridClient(apiKey);

            var msg = new sgMail.SendGridMessage();
            msg.SetFrom(new sgMail.EmailAddress(Options.FromAddress));
            msg.AddTo(new sgMail.EmailAddress(email));
            msg.PlainTextContent = message;
            msg.HtmlContent = message;

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Send grid email response: {response.StatusCode}");
                _logger.LogError(response.Headers.ToString());
            }
        }
    }
}
