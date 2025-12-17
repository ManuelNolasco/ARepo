using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IUsuarioDao _dao;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    private const string ESTADO_ACTIVO = "A";
    private const string TIPO_TOKEN = "bearer"; // O definir regla

    public AuthController(IUsuarioDao dao, IPasswordHasher hasher, ITokenService tokenService)
    {
        _dao = dao;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Loginv1Response>> Login([FromBody] Loginv1Request req)
    {
        try {
            var usuario = await _dao.GetByPhoneAsync(req.telefono);

            if (usuario == null) return Unauthorized(new { mensaje = "Credenciales inválidas." });

            /*if (!string.Equals(usuario.Estado, ESTADO_ACTIVO, StringComparison.OrdinalIgnoreCase))
                return Forbid("Cuenta no activa.");*/

            if (!_hasher.Verify(req.password, usuario.Password))
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            string token = _tokenService.GenerateToken(usuario);
            //usuario.Password = string.Empty; // Por seguridad no mostrar hash

            bool sesion =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    != null || !string.IsNullOrWhiteSpace(token) ? true : false;

            var response = new Loginv1Response {
                    user = new Userv1Response {
                        id = usuario.Id,
                        nombres = usuario.Nombres,
                        apellidos = usuario.Apellidos,
                        session_active = sesion,
                        fechanacimiento = usuario.FechaNacimiento,
                        email = usuario.Email,
                        telefono = usuario.Telefono,
                        password = usuario.Password, // "secret", // O por seguridad no mostrar hash, ni clave plana
                        address = usuario.Direccion,
                    },
                    access_token = token,
                    token_type = TIPO_TOKEN,
                };
            return Ok(response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }
}
