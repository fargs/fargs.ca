using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.CompanyUserRegistrationInvitation
{
    public class CompanyUserRegistrationInvitationSendGridEmailService : ICompanyUserRegistrationInvitationEmailService
    {
        private readonly ILogger<CompanyUserRegistrationInvitationSendGridEmailService> _logger;
        private readonly SendGridOptions _options;

        public CompanyUserRegistrationInvitationSendGridEmailService(ILogger<CompanyUserRegistrationInvitationSendGridEmailService> logger, IOptions<SendGridOptions> optionsAccessor)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
        }
        public Task SendEmailAsync(string toAddress, CompanyUserRegistrationInvitationTemplateData templateData)
        {
            return Execute(_options.SendGridKey, _options.Templates.CompanyUserRegistrationInvitationId, toAddress, templateData);
        }
        public async Task Execute(string apiKey, string templateId, string toAddress, object templateData)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(_options.FromAddress));
            msg.AddTo(new EmailAddress(toAddress));
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);

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
