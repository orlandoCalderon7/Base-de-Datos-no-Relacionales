using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Sesion_05;

/// <summary>
/// Subtema 5 - Clean Architecture en 4 Capas (Caso AdoptaMas)
/// Demostración de la estructura modular desacoplada:
/// 1. Domain: Entidad pura independiente de frameworks.
/// 2. Application: Contrato IMascotaRepositorio y caso de uso MascotaService.
/// 3. Infrastructure: Implementación concreta MascotaRepositorioEnMemoria.
/// 4. WebAPI / Composition Root: Cableado en IServiceCollection y ejecución.
/// </summary>
public class CleanArchitectureDemo
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 5. CLEAN ARCHITECTURE EN 4 CAPAS (ADOPTAMAS)");
        Console.WriteLine("=======================================================");

        // [Capa 4: WebAPI / Composition Root]
        // Se registran las dependencias cumpliendo Inversión de Control
        var services = new ServiceCollection();

        // Inversión: La interfaz (Application) se resuelve con el repositorio (Infrastructure)
        services.AddScoped<IMascotaRepositorio, MascotaRepositorioEnMemoria>();

        // Servicio de caso de uso (Application)
        services.AddScoped<MascotaService>();

        var provider = services.BuildServiceProvider();

        // Consumo simulado desde un controlador o endpoint
        var mascotaService = provider.GetRequiredService<MascotaService>();
        var catalogo = mascotaService.ObtenerCatalogoDisponible();

        Console.WriteLine($"[WebAPI -> Application]: Catálogo obtenido con {catalogo.Count} mascotas disponibles:\n");

        foreach (var m in catalogo)
        {
            Console.WriteLine($" • [{m.Id}] {m.Nombre,-10} | Especie: {m.Especie,-6} | Edad: {m.EdadMeses,2} meses | Costo: S/ {m.CostoVacunacion,6:F2}");
        }
    }
}

// =====================================================================
// CAPA 1: DOMAIN (Núcleo puro, CERO dependencias externas ni paquetes)
// =====================================================================
public record Mascota(int Id, string Nombre, string Especie, int EdadMeses, decimal CostoVacunacion);

// =====================================================================
// CAPA 2: APPLICATION (Contratos de repositorio y orquestación de casos de uso)
// =====================================================================
public interface IMascotaRepositorio
{
    List<Mascota> ListarTodas();
}

public class MascotaService(IMascotaRepositorio repositorio)
{
    public List<Mascota> ObtenerCatalogoDisponible()
    {
        // Regla de aplicación: obtener datos a través de la abstracción
        return repositorio.ListarTodas();
    }
}

// =====================================================================
// CAPA 3: INFRASTRUCTURE (Acceso a datos real o simulado en memoria)
// =====================================================================
public class MascotaRepositorioEnMemoria : IMascotaRepositorio
{
    private readonly List<Mascota> _almacenLocal = [
        new(1, "Rex", "Perro", 24, 45.00m),
        new(2, "Pelusa", "Gato", 6, 30.00m),
        new(3, "Rocky", "Perro", 14, 50.00m),
        new(4, "Michi", "Gato", 36, 25.00m)
    ];

    public List<Mascota> ListarTodas() => _almacenLocal;
}
