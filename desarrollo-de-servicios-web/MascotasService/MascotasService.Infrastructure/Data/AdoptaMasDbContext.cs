namespace MascotasService.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using MascotasService.Domain.Entities;

public class AdoptaMasDbContext(DbContextOptions<AdoptaMasDbContext> options) : DbContext(options)
{
    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Albergue> Albergues => Set<Albergue>();
    public DbSet<SolicitudAdopcion> Solicitudes => Set<SolicitudAdopcion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Nombre).HasMaxLength(60).IsRequired();
            entity.Property(m => m.Especie).HasMaxLength(30).IsRequired();
            entity.Property(m => m.Raza).HasMaxLength(50);
            entity.Property(m => m.Sexo).HasMaxLength(10);
            entity.Property(m => m.Tamano).HasMaxLength(20);
            entity.Property(m => m.Estado).HasMaxLength(20);
            entity.Property(m => m.FotoUrl).HasMaxLength(300);

            entity.HasOne(m => m.Albergue)
                  .WithMany(a => a.Mascotas)
                  .HasForeignKey(m => m.AlbergueId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Albergue>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(a => a.Ciudad).HasMaxLength(50);
            entity.Property(a => a.Direccion).HasMaxLength(150);
            entity.Property(a => a.Telefono).HasMaxLength(20);
            entity.Property(a => a.EmailContacto).HasMaxLength(80);
        });

        modelBuilder.Entity<SolicitudAdopcion>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Estado).HasMaxLength(20);
            entity.Property(s => s.MotivoAdopcion).HasMaxLength(500);
            entity.Property(s => s.ComentariosAlbergue).HasMaxLength(500);

            entity.HasOne(s => s.Mascota)
                  .WithMany(m => m.Solicitudes)
                  .HasForeignKey(s => s.MascotaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seeding Inicial
        modelBuilder.Entity<Albergue>().HasData(
            new Albergue
            {
                Id = 1,
                Nombre = "Albergue Patitas Felices",
                Ciudad = "Lima",
                Direccion = "Av. Brasil 1234, Jesús María",
                Telefono = "987654321",
                EmailContacto = "patitas@adopta.pe",
                CapacidadMaxima = 40,
                EsActivo = true
            },
            new Albergue
            {
                Id = 2,
                Nombre = "Refugio Huellitas del Alma",
                Ciudad = "Lima",
                Direccion = "Jr. Los Pinos 456, Surco",
                Telefono = "912345678",
                EmailContacto = "huellitas@adopta.pe",
                CapacidadMaxima = 30,
                EsActivo = true
            }
        );

        modelBuilder.Entity<Mascota>().HasData(
            new Mascota
            {
                Id = 1,
                Nombre = "Bobby",
                Especie = "Perro",
                Raza = "Mestizo",
                EdadMeses = 18,
                Sexo = "Macho",
                Tamano = "Mediano",
                Estado = "Disponible",
                Descripcion = "Juguetón, amigable con niños y vacunado.",
                FotoUrl = "https://images.unsplash.com/photo-1543466835-00a7907e9de1",
                AlbergueId = 1,
                FechaIngreso = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Mascota
            {
                Id = 2,
                Nombre = "Michi",
                Especie = "Gato",
                Raza = "Común Europeo",
                EdadMeses = 8,
                Sexo = "Hembra",
                Tamano = "Pequeno",
                Estado = "Disponible",
                Descripcion = "Tranquila, esterilizada y cariñosa.",
                FotoUrl = "https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba",
                AlbergueId = 1,
                FechaIngreso = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Mascota
            {
                Id = 3,
                Nombre = "Luna",
                Especie = "Perro",
                Raza = "Poodle",
                EdadMeses = 36,
                Sexo = "Hembra",
                Tamano = "Pequeno",
                Estado = "Adoptado",
                Descripcion = "Muy educada y sociable.",
                FotoUrl = "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e",
                AlbergueId = 2,
                FechaIngreso = new DateTime(2024, 11, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
