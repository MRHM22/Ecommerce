using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.MessageImplementation;

public class EmailService : IEmailService
{
    private readonly IFluentEmail fluentEmail;
    private readonly EmailFluentSettings emailFluentSettings;

    public EmailService(IFluentEmail fluentEmail, IOptions<EmailFluentSettings> emailFluentSetting)
    {
        this.fluentEmail = fluentEmail;
        this.emailFluentSettings = emailFluentSetting.Value;
    }

    public async Task<bool> SendEmailAsync(EmailMessage email, string token)
    {
        var htmlContent = $"{email.Body} {this.emailFluentSettings.BaseUrlClient}/password/reset/{token}";
        var result = await this.fluentEmail
        .To(email.To)
        .Subject(email.Subject)
        .Body(htmlContent)
        .SendAsync();

        return result.Successful;
    }
}

