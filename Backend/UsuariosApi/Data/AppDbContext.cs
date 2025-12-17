using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

    // Configuración adicional de modelo para constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        });*/

        modelBuilder.Entity<Usuario>().ToTable(t => t.HasCheckConstraint("CK_Usuario_Estado", "Estado IN ('A', 'I')"));
    }
    
}
