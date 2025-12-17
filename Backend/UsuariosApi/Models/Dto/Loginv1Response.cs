public class Loginv1Response
{
    public Userv1Response user { get; set; } = null!;
    public string access_token { get; set; } = null!;
    public string token_type { get; set; } = "bearer"; // O definir regla
}