using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;

public class UpdateAdminStatusUserCommandHandler : IRequestHandler<UpdateAdminStatusUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;
    public UpdateAdminStatusUserCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Usuario> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByIdAsync(request.Id!);
        if (updateUser is null)
        {
            throw new BadRequestException("El usuario no existe");
        }
        updateUser.IsActive = !updateUser.IsActive;
        var result = await _userManager.UpdateAsync(updateUser);
        if (!result.Succeeded)
        {
            throw new Exception("No se pudo actualizar el estado del usuario");
        }
        return updateUser;
    }
}