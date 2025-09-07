using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;
    public ResetPasswordCommandHandler(UserManager<Usuario> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updatedUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (updatedUser is null)
            throw new BadRequestException("El usuario no existe");
        var resultValidateOldPassword = _userManager.PasswordHasher
                    .VerifyHashedPassword(updatedUser, updatedUser.PasswordHash!, request.OldPassword!);
        if (!(resultValidateOldPassword == PasswordVerificationResult.Success))
            throw new BadRequestException("La contraseña actual es incorrecta");

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updatedUser, request.NewPassword!);
        updatedUser.PasswordHash = hashedNewPassword;
        var result = await _userManager.UpdateAsync(updatedUser);
        if (!result.Succeeded)
            throw new BadRequestException("No se pudo actualizar la contraseña");
        return Unit.Value;
    }
}