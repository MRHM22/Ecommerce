using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;   
    public LoginUserCommandHandler(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
                                    RoleManager<IdentityRole> roleManager, IAuthService authService,
                                    IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
            throw new NotFoundException(nameof(Usuario), request.Email!);
        if (!user.IsActive)
            throw new Exception("El usuario no está activo. contacte al administrador.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (!result.Succeeded)
            throw new Exception("Credenciales del usuario son incorrectas.");

        var direccionEnvio = await _unitOfWork.Repository<Address>().GetEntityAsync(a => a.Username == user.UserName);
        var roles = await _userManager.GetRolesAsync(user);
        var AuthResponse = new AuthResponse
        {
            Id = user.Id,
            Nomnbre = user.Nombre,
            Apellido = user.Apellido,
            Telefono = user.Telefono,
            Username = user.UserName,
            Email = user.Email,
            Avatar = user.AvatarUrl,
            DireccionEnvio = _mapper.Map<AddressVm>(direccionEnvio),
            Roles = roles.ToList(),
            Token = _authService.CreateToken(user, roles),
        };
        return AuthResponse;
    }
}