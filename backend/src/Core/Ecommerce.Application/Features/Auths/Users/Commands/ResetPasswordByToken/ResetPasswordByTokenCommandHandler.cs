using System.Text;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;

public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
{
    private readonly UserManager<Usuario>   _userManager;
    public ResetPasswordByTokenCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword))
            throw new BadRequestException("Las contraseñas no coinciden");

        var updateUsuario = await _userManager.FindByEmailAsync(request.Email!);
        if (updateUsuario is null)
            throw new BadRequestException("No se encontró el usuario registrado con ese email");

        var token = Convert.FromBase64String(request.Token!);
        var decodedToken = Encoding.UTF8.GetString(token);
        var result = await _userManager.ResetPasswordAsync(updateUsuario, decodedToken, request.Password!);
        if (!result.Succeeded)
            throw new Exception("No se pudo restablecer la contraseña");
        return $"Se ha restablecido la contraseña correctamente{request.Email}";
    }
}