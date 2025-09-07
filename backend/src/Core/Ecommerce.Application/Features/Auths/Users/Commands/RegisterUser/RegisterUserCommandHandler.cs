using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;
    public RegisterUserCommandHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }
    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existenteUserByEmail = _userManager.FindByEmailAsync(request.Email!) is null ? false : true;
        if (existenteUserByEmail)
            throw new BadRequestException("El email ya se encuentra registrado");

        var existeUserByUsername = _userManager.FindByNameAsync(request.Username!) is null ? false : true;
        if (existeUserByUsername)
            throw new BadRequestException("El nombre de usuario ya se encuentra registrado");

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.Username,
            AvatarUrl = request.FotoUrl,
        };

        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);
        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(usuario!, AppRole.GenericUser);
            var roles = await _userManager.GetRolesAsync(usuario!);
            return new AuthResponse
            {
                Id = usuario.Id,
                Nomnbre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Username = usuario.UserName,
                Email = usuario.Email,
                Avatar = usuario.AvatarUrl,
                Roles = roles,
                Token = _authService.CreateToken(usuario, roles)
            };
        }
        throw new Exception("No se pudo registrar el usuario");
            
    }
}