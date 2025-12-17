using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUsuarioDao
{
    Task<Usuario> GetByPhoneAsync(string telefono);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario> GetByIdAsync(int id);
    Task<int> InsertAsync(Usuario usuario);
    Task<int> GetIdByFieldsAsync(int id);
    Task<bool> UpdateAsync(Usuario usuario);
    Task<bool> DeleteAsync(int id);
}
