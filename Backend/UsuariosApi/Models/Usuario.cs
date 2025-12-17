using System;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombres { get; set; }

    [Required, MaxLength(100)]
    public string Apellidos { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required, MaxLength(500)]
    public string Direccion { get; set; }

    [Required, MaxLength(120)]
    public string Password { get; set; }

    [Required, MaxLength(20)]
    public string Telefono { get; set; }

    [Required, MaxLength(150)]
    public string Email { get; set; }

    [Required, MaxLength(1)]
    public string Estado { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.Today;
    public DateTime? FechaModificacion { get; set; } /*= DateTime.UtcNow;*/
}