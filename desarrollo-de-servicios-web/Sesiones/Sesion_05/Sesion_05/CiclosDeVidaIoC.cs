using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sesion_05;

/// <summary>
/// Subtema 5 - Inyección de Dependencias (IoC Container en .NET)
/// Demostración práctica de los 3 ciclos de vida:
/// - Transient: Nueva instancia cada vez que se solicita el servicio.
/// - Scoped: Misma instancia dentro del mismo Scope (petición HTTP), nueva entre Scopes.
/// - Singleton: Una sola instancia viva durante toda la ejecución de la aplicación.
/// </summary>
public class CiclosDeVidaIoC
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 4. INYECCIÓN DE DEPENDENCIAS (IoC) Y CICLOS DE VIDA");
        Console.WriteLine("=======================================================");

        // 1. Registro de servicios en el contenedor IServiceCollection
        var services = new ServiceCollection();

        services.AddTransient<IOperacionTransient, OperacionServicio>();
        services.AddScoped<IOperacionScoped, OperacionServicio>();
        services.AddSingleton<IOperacionSingleton, OperacionServicio>();

        // 2. Construcción del contenedor (ServiceProvider)
        var serviceProvider = services.BuildServiceProvider();

        // 3. Simulación de Petición HTTP 1 (Scope 1)
        Console.WriteLine(">>> SIMULACIÓN PETICIÓN HTTP 1 (Scope 1):");
        using (var scope1 = serviceProvider.CreateScope())
        {
            var transient1 = scope1.ServiceProvider.GetRequiredService<IOperacionTransient>();
            var transient2 = scope1.ServiceProvider.GetRequiredService<IOperacionTransient>();

            var scoped1 = scope1.ServiceProvider.GetRequiredService<IOperacionScoped>();
            var scoped2 = scope1.ServiceProvider.GetRequiredService<IOperacionScoped>();

            var singleton1 = scope1.ServiceProvider.GetRequiredService<IOperacionSingleton>();

            Console.WriteLine($" • Transient 1: {transient1.OperacionId} | Transient 2: {transient2.OperacionId}  -> (Distintos: nueva instancia en cada llamada)");
            Console.WriteLine($" • Scoped 1:    {scoped1.OperacionId} | Scoped 2:    {scoped2.OperacionId}  -> (Idénticos: misma instancia en este Scope)");
            Console.WriteLine($" • Singleton 1: {singleton1.OperacionId}                              -> (Único en toda la aplicación)");
        }

        // 4. Simulación de Petición HTTP 2 (Scope 2)
        Console.WriteLine("\n>>> SIMULACIÓN PETICIÓN HTTP 2 (Scope 2):");
        using (var scope2 = serviceProvider.CreateScope())
        {
            var scoped3 = scope2.ServiceProvider.GetRequiredService<IOperacionScoped>();
            var singleton2 = scope2.ServiceProvider.GetRequiredService<IOperacionSingleton>();

            Console.WriteLine($" • Scoped 3:    {scoped3.OperacionId}                              -> (¡Nuevo ID!: nuevo Scope HTTP)");
            Console.WriteLine($" • Singleton 2: {singleton2.OperacionId}                              -> (¡Mismo ID!: el Singleton nunca muere)");
        }
    }
}

public interface IOperacionBase
{
    string OperacionId { get; }
}

public interface IOperacionTransient : IOperacionBase { }
public interface IOperacionScoped : IOperacionBase { }
public interface IOperacionSingleton : IOperacionBase { }

public class OperacionServicio : IOperacionTransient, IOperacionScoped, IOperacionSingleton
{
    // Genera un identificador único recortado en cada instanciación
    public string OperacionId { get; } = Guid.NewGuid().ToString()[..8];
}
