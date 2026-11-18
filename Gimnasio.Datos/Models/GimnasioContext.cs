using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos;

public partial class GimnasioContext : DbContext
{
    public GimnasioContext(DbContextOptions<GimnasioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Membresia> Membresias { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Membresia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membresi__3214EC078C93B430");

            entity.Property(e => e.Precio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.TipoPlan).HasMaxLength(30);

            entity.HasOne(d => d.Socio).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.SocioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membresias_Socios");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Socios__3214EC07113DB31D");

            entity.HasIndex(e => e.Dni, "UQ__Socios__C0308575DD21D590").IsUnique();

            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Correo).IsUnique();

            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.ClaveHash).HasMaxLength(200);
            entity.Property(e => e.Rol).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}