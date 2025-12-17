public class Userv1Response
{
    public int id { get; set; }
    public string nombres { get; set; } = null!;
    public string apellidos { get; set; } = null!;
    public bool session_active { get; set; } = false!;
    public DateTime fechanacimiento { get; set; }
    public string email { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public string password { get; set; } = null!;
    public string address { get; set; } = null!;
}