using Ecommerce.Application.Models.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application.Extensions;

public static class FluentEmailExtensions
{
    public static void AddServiceEmail(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailFluentSettings>(
            config.GetSection(nameof(EmailFluentSettings))
        );
        var emailSettings = config.GetSection(nameof(EmailFluentSettings));
        var fromEmail = emailSettings.GetValue<string>("Email");
        var host = emailSettings.GetValue<string>("Host");
        var port = emailSettings.GetValue<int>("Port");

        services.AddFluentEmail(fromEmail).AddSmtpSender(host, port);
    }
}