using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioDao _dao;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    private const string ESTADO_ACTIVO = "A";

    public UsuarioService(IUsuarioDao dao, IPasswordHasher hasher, ITokenService tokenService)
    {
        _dao = dao;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Responde todos los usuarios.
    /// Mapea una estructura de respesta a un DTO diseñado para varios usos.
    /// </summary>
    /// <returns>
    /// - UsuarioActuDto
    /// - Exception personalizado usado desde DAO
    /// </returns>
    public async Task<List<UsuarioActuDto>> SetAll()
    {
        try {
            var usuario = await _dao.GetAllAsync();
            return usuario == null ? null :
                usuario.Select(u => UsuarioAUsuarioActuDto(u)).ToList();;
        }
        catch (DaoException ex) { throw ex; }
    }

    public async Task<UsuarioActuDto> SetById(int id)
    {
        try
        {
            var usuario = await _dao.GetByIdAsync(id);
            return usuario == null ? null :
                UsuarioAUsuarioActuDto(usuario);
        }
        catch (DaoException ex) { throw ex; }
    }

    public async Task<UsuarioActuPassDto> Save(Accion action, UsuarioActuPassDto dto)
    {
        int id = 0;

        try {
            string fecha = dto.fechanacimiento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            dto.fechanacimiento = DateTime.ParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var usuario = new Usuario
            {
                Id = dto.id,
                Nombres = dto.nombres,
                Apellidos = dto.apellidos,
                FechaNacimiento = dto.fechanacimiento,
                Direccion = dto.direccion,
                Telefono = dto.telefono,
                Email = dto.email,
                Estado = ESTADO_ACTIVO// O posible acción actualizar estado usuario
            };

            usuario.Password = _hasher.Hash(dto.password);// O posible acción actualizar pass

            if (action == Accion.CREA)
            {
                id = await _dao.InsertAsync(usuario);
                if(id < 1) return null;
                else usuario.Id = id;
            }
            else
            { // Primero verifica si existe
                id = await _dao.GetIdByFieldsAsync(usuario.Id);
                if(id < 1) return null;
                else usuario.Id = id;
                bool ok = await _dao.UpdateAsync(usuario);
                if(!ok) return null;
            }

            return UsuarioAUsuarioActuPassDto(usuario);
        }
        catch (DaoException ex) { throw ex; }
    }

    public async Task<UsuarioActuPassDto> Delete(int id)
    {
        bool ok = false;

        try
        {    
            var usuario = await _dao.GetByIdAsync(id);
            if (usuario != null) ok = await _dao.DeleteAsync(id);
            else return null; // En caso haya encontrado, pero no haya eliminado
            return ok ? UsuarioAUsuarioActuPassDto(usuario) : null;
        }
        catch (DaoException ex) { throw ex; }
    }

    /// <summary>
    /// Convierte de usuario a solo campos seleccionados de ese.
    /// </summary>
    /// <returns>
    /// UsuarioActuDto
    /// </returns>
    public UsuarioActuDto UsuarioAUsuarioActuDto(Usuario usuario)
    {
        return new UsuarioActuDto
        {
            id = usuario.Id,
            nombres = usuario.Nombres,
            apellidos = usuario.Apellidos,
            fechanacimiento = usuario.FechaNacimiento,
            telefono = usuario.Telefono,
            email = usuario.Email,
            direccion = usuario.Direccion
        };
    }

    /// <summary>
    /// Convierte de usuario a solo campos seleccionados de ese, incluyendo el campo de contraseña.
    /// </summary>
    /// <returns>
    /// UsuarioActuPassDto
    /// </returns>
    public UsuarioActuPassDto UsuarioAUsuarioActuPassDto(Usuario usuario)
    {
        return new UsuarioActuPassDto
        {
            id = usuario.Id,
            nombres = usuario.Nombres,
            apellidos = usuario.Apellidos,
            fechanacimiento = usuario.FechaNacimiento,
            telefono = usuario.Telefono,
            email = usuario.Email,
            password = usuario.Password, // "secret", // O por seguridad no mostrar hash, ni clave plana
            direccion = usuario.Direccion
        };
    }
}
