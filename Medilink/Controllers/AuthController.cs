using Medilink.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Medilink.DTO;
namespace Medilink.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] LoginDto loginDto)
    {
        var token = _authService.Login(loginDto);
        if (token == null) return Unauthorized("Usuario o contraseña incorrectos");
        return Ok(token);
    }
}