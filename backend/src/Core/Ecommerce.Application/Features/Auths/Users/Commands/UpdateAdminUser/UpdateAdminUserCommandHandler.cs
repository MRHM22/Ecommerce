using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public UpdateAdminUserCommandHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }
    public async Task<Usuario> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByIdAsync(request.Id!);
        if (updateUser is null)
            throw new BadRequestException("El usuario no existe");
        updateUser.Nombre = request.Nombre;
        updateUser.Apellido = request.Apellido;
        updateUser.Telefono = request.Telefono;

        var resultado = await _userManager.UpdateAsync(updateUser);
        if (!resultado.Succeeded)
            throw new Exception("No se pudo actualizar el usuario");

        var role = await _roleManager.FindByNameAsync(request.Role!);
        if (role is null)
            throw new BadRequestException("El rol no existe");

        var userRoles = await _userManager.AddToRoleAsync(updateUser, role.Name!);
        if (!userRoles.Succeeded)
            throw new Exception("No se pudo asignar el rol al usuario");
        return updateUser;
    }
}