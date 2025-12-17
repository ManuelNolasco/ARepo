public class UsuarioCreaDto
{
    public string nombres { get; set; } = null!;
    public string apellidos { get; set; } = null!;
    public DateTime fechanacimiento { get; set; }
    public string telefono { get; set; } = null!;
    public string email { get; set; } = null!;
    public string direccion { get; set; } = null!;
}
