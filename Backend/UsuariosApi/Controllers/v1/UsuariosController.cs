using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioDao _dao;
    private readonly IUsuarioService _service;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public UsuariosController(IUsuarioDao dao, IUsuarioService service, IPasswordHasher hasher, ITokenService tokenService)
    {
        _dao = dao;
        _service = service;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<UsuarioActuDto>>> GetUsers()
    {
        try
        {
            var response = await _service.SetAll();
            return response == null ? NotFound() : Ok(response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UsuarioActuDto>> GetUser(int id)
    {
        try
        {
            if (id < 1) return BadRequest();
            var response = await _service.SetById(id);
            return response == null ? NotFound() : Ok(response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpPost]
    //[Authorize] // COLOCAR DE NUEVO
    [AllowAnonymous] // DE MOMENTO
    public async Task<ActionResult<UsuarioActuPassDto>> CreateUser([FromBody] UsuarioCreaPassDto request)
    {
        try {
            var req = new UsuarioActuPassDto {
                nombres = request.nombres,
                apellidos = request.apellidos,
                fechanacimiento = request.fechanacimiento,
                telefono = request.telefono,
                email = request.email,
                password = request.password,
                direccion = request.direccion
            };
            var response = await _service.Save(Accion.CREA, req);
            return response == null ? NotFound()
                : CreatedAtAction(nameof(GetUser), new { response.id }, response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UsuarioActuPassDto>> UpdateUser(int id, [FromBody] UsuarioCreaPassDto request)
    {
        try
        {
            if (id < 1) return BadRequest();
            var req = new UsuarioActuPassDto {
                id = id,
                nombres = request.nombres,
                apellidos = request.apellidos,
                fechanacimiento = request.fechanacimiento,
                telefono = request.telefono,
                email = request.email,
                password = request.password,
                direccion = request.direccion
            };
            var response = await _service.Save(Accion.ACTUALIZA, req);
            return response == null ? NotFound() : Ok(response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            if (id < 1) return BadRequest();
            var response = await _service.Delete(id);
            return response == null ? NotFound() : Ok(response);
        }
        catch (DaoException ex) { return StatusCode(500, ex.Message); }
    }
}
