using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestControlles : ControllerBase
{
    private readonly IEmailService emailService;
    public TestControlles(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SendEmail()
    {
        var message = new EmailMessage
        {
            To = "hdezmartin22@gmail.com",
            Body = "Es una prueba de envio",
            Subject = "Cambiar clave",
        };

        var result = await this.emailService.SendEmailAsync(message, "token22");
        return result ? Ok() : BadRequest();
    }
}