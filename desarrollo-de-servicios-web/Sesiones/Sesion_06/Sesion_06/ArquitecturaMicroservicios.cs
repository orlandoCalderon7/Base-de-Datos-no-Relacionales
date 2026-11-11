using System;

namespace Sesion_06;

/// <summary>
/// Subtema 6 - Concepto 1: Monolito vs Microservicios y Bounded Context (DDD)
/// En una arquitectura de microservicios, cada servicio es dueño exclusivo
/// de su modelo de datos y de sus reglas de negocio (Bounded Context).
/// </summary>
public class ArquitecturaMicroservicios
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 1. BOUNDED CONTEXT Y AUTONOMÍA DE MICROSERVICIOS");
        Console.WriteLine("=======================================================");

        // Dominio 1: Mascotas (MascotasService en Puerto 5001)
        var mascota = new MascotaItem(101, "Rex", "Perro", 24, "Disponible");
        Console.WriteLine($"[Microservicio Mascotas (:5001)]");
        Console.WriteLine($" • Entidad: {mascota.Nombre} ({mascota.Especie}) | Edad: {mascota.EdadMeses}m | Estado: {mascota.Estado}");

        // Dominio 2: Adopciones (AdopcionesService en Puerto 5002)
        // Nota: Solo referencia a la mascota por su Id ('MascotaId'), NO tiene la entidad completa en su BD.
        var solicitud = new SolicitudAdopcion(501, 101, "Juan Pérez", "juan.perez@zegel.pe", "En Revisión");
        Console.WriteLine($"\n[Microservicio Adopciones (:5002)]");
        Console.WriteLine($" • Solicitud #{solicitud.Id} | Mascota Referenciada ID: #{solicitud.MascotaId} | Adoptante: {solicitud.Adoptante} ({solicitud.Email})");

        Console.WriteLine("\n-> Conclusión Arquitectónica:");
        Console.WriteLine(" • Cada microservicio tiene su propia base de datos (Database-per-Service).");
        Console.WriteLine(" • No existen Foreign Keys directas entre bases de datos de distintos microservicios.");
    }
}

/// <summary>
/// Modelo de datos del Bounded Context de Mascotas.
/// </summary>
public record MascotaItem(int Id, string Nombre, string Especie, int EdadMeses, string Estado);

/// <summary>
/// Modelo de datos del Bounded Context de Adopciones.
/// </summary>
public record SolicitudAdopcion(int Id, int MascotaId, string Adoptante, string Email, string Estado);
