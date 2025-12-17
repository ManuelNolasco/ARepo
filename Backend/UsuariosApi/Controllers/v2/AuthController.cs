using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers.v2;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioDao _dao;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public AuthController(IUsuarioDao dao, IPasswordHasher hasher, ITokenService tokenService)
    {
        _dao = dao;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    [HttpGet("validate")]
    [Authorize]
    public ActionResult Validate()
    {
        // Si el request llega aquí, el middleware JWT ya confirmó:
        // - firma válida
        // - no expirado
        // - issuer/audience correctos
        return Ok(new { mensaje = "Sesión activa", usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value });
    }
}
