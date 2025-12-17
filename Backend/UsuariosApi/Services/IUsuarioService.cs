public interface IUsuarioService
{
    Task<List<UsuarioActuDto>> SetAll();
    Task<UsuarioActuDto> SetById(int id);
    Task<UsuarioActuPassDto> Save(Accion action, UsuarioActuPassDto usuario);
    Task<UsuarioActuPassDto> Delete(int id);
}
