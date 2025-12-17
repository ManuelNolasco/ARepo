using Dapper;
using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;//dif conector

public class UsuarioDao : IUsuarioDao
{
    private readonly string _connectionString;

    public UsuarioDao(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    private IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);

    public async Task<Usuario> GetByPhoneAsync(string telefono)
    {
        try
        {
            const string sql = @"SELECT Id, Nombres, Apellidos, FechaNacimiento, Direccion,
                Password, Telefono, Email, Estado FROM Usuarios
                WHERE Telefono = @Telefono
                ORDER BY Id DESC LIMIT 1;"; // O posibles mejoras en consulta de usuario
            using var db = CreateConnection();
            return await db.QuerySingleOrDefaultAsync<Usuario>(sql, new { Telefono = telefono });
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error al buscar el usuario con Telefono={telefono}.", ex);
        }
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        try
        {
            const string sql = @"SELECT Id, Nombres, Apellidos, FechaNacimiento, Direccion,
                Telefono, Email FROM Usuarios;"; // Por seguridad no consulta clave
            using var db = CreateConnection();
            return await db.QueryAsync<Usuario>(sql);
        }
        catch (Exception ex)
        {
            throw new DaoException("Error al obtener la lista de usuarios.", ex);
        }
    }

    public async Task<Usuario> GetByIdAsync(int id)
    {
        try
        {
            const string sql = @"SELECT Id, Nombres, Apellidos, FechaNacimiento, Direccion,
                Password, Telefono, Email FROM Usuarios WHERE Id = @Id;"; // O por seguridad no consulta clave
            using var db = CreateConnection();
            return await db.QuerySingleOrDefaultAsync<Usuario>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error al buscar el usuario con Id={id}.", ex);
        }
    }

    public async Task<int> InsertAsync(Usuario usuario)
    {
        try
        {
            const string sql = @"
                INSERT INTO Usuarios
                (Nombres, Apellidos, FechaNacimiento, Direccion, Password,
                Telefono, Email, Estado)
                VALUES
                (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password,
                @Telefono, @Email, @Estado);
                SELECT LAST_INSERT_ID();";
            using var db = CreateConnection();
            var id = await db.ExecuteScalarAsync<int>(sql, usuario);
            return id;
        }
        catch (Exception ex)
        {
            throw new DaoException("Error al insertar el usuario.", ex);
        }
    }

    public async Task<int> GetIdByFieldsAsync(int id)
    {
        try
        {
            const string sql = @"SELECT Id FROM Usuarios WHERE Id = @Id;"; // O posibles mejoras en consulta de usuario
            using var db = CreateConnection();
            id = await db.QuerySingleOrDefaultAsync<int>(sql, new { Id = id });
            return id;
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error al buscar el usuario con la petición.", ex);
        }
    }

    public async Task<bool> UpdateAsync(Usuario usuario)
    {
        try
        {
            const string sql = @"
                UPDATE Usuarios SET
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    FechaNacimiento = @FechaNacimiento,
                    Direccion = @Direccion,
                    Password = @Password,
                    Telefono = @Telefono,
                    Email = @Email,
                    Estado = @Estado
                WHERE Id = @Id;";
            using var db = CreateConnection();
            var rows = await db.ExecuteAsync(sql, usuario);
            return rows > 0;
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error al actualizar el usuario con Id={usuario.Id}.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            const string sql = @"DELETE FROM Usuarios WHERE Id = @Id;";
            using var db = CreateConnection();
            var rows = await db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error al eliminar el usuario con Id={id}.", ex);
        }
    }
}
