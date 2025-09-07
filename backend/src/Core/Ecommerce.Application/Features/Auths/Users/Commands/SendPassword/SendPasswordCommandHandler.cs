using System.Text;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<Usuario> _userManager;
    public SendPasswordCommandHandler(IEmailService emailService, UserManager<Usuario> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
            throw new BadRequestException("El usuario no existe");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        var emailMessage = new EmailMessage
        {
            To = request.Email!,
            Subject = "Restablecimiento de contraseña",
            Body = "Dale click aqui"
        };
        var result = await _emailService.SendEmailAsync(emailMessage, token);

        if (!result)
            throw new Exception("No se pudo restablecer la contraseña");
        return $"Se ha enviado un correo {request.Email} para restablecer la contraseña";
    }
}