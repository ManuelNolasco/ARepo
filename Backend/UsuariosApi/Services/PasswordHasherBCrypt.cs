//using BCrypt.Net;

public class PasswordHasherBCrypt : IPasswordHasher
{
    private const int WorkFactor = 12; // work‑factor (cost) recomendado: 10‑12

    public string Hash(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(plainPassword));

        return BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: WorkFactor);
    }

    public bool Verify(string plainPassword, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) ||
        (!string.IsNullOrWhiteSpace(hashedPassword) && !hashedPassword.StartsWith("$2")))
            return false;

        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}

