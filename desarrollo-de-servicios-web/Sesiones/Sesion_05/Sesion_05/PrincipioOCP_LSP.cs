using System;
using System.Collections.Generic;

namespace Sesion_05;

/// <summary>
/// Subtema 5 - Principios 2 y 3: Open/Closed (OCP) y Liskov Substitution (LSP)
/// OCP: Las entidades deben estar abiertas para extensión, pero cerradas para modificación.
/// LSP: Las subclases o implementaciones deben poder sustituir al contrato base sin romper el programa.
/// </summary>
public class PrincipioOCP_LSP
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 2. PRINCIPIOS OPEN/CLOSED (OCP) Y LISKOV (LSP)");
        Console.WriteLine("=======================================================");

        decimal costoBaseServicio = 100.00m;

        // Lista polimórfica que cumple LSP: cualquier estrategia sustituye a IDescuentoEstrategia
        List<IDescuentoEstrategia> estrategias = [
            new DescuentoEstudiante(),
            new DescuentoCampaniaNavidad(),
            new DescuentoSinDescuento(),
            new DescuentoAnioNuevo()
        ];

        Console.WriteLine($"Costo base de atención veterinaria: S/ {costoBaseServicio:F2}\n");

        foreach (var estrategia in estrategias)
        {
            // OCP: Si mañana creamos 'DescuentoTerceraEdad', NO tocamos esta lógica ni las clases existentes
            decimal montoFinal = estrategia.AplicarDescuento(costoBaseServicio);
            Console.WriteLine($" • [{estrategia.Descripcion,-25}]: Total a pagar = S/ {montoFinal:F2}");
        }
    }
}

/// <summary>
/// Contrato común para todas las reglas de descuento (Abstracción extensible).
/// </summary>
public interface IDescuentoEstrategia
{
    string Descripcion { get; }
    decimal AplicarDescuento(decimal montoBase);
}

/// <summary>
/// Extensión 1: Descuento para estudiantes de Zegel (15% OFF).
/// </summary>
public class DescuentoEstudiante : IDescuentoEstrategia
{
    public string Descripcion => "Estudiante Zegel (15% OFF)";
    public decimal AplicarDescuento(decimal montoBase) => montoBase * 0.85m;
}

/// <summary>
/// Extensión 2: Descuento especial de campaña navideña (20% OFF).
/// </summary>
public class DescuentoCampaniaNavidad : IDescuentoEstrategia
{
    public string Descripcion => "Campaña Navidad (20% OFF)";
    public decimal AplicarDescuento(decimal montoBase) => montoBase * 0.80m;
}

/// <summary>
/// Extensión 3: Tarifa regular sin descuento (0% OFF) que respeta el invariante (LSP).
/// </summary>
public class DescuentoSinDescuento : IDescuentoEstrategia
{
    public string Descripcion => "Tarifa Regular (0% OFF)";
    public decimal AplicarDescuento(decimal montoBase) => montoBase;
}

public class DescuentoAnioNuevo : IDescuentoEstrategia
{
    public string Descripcion => "Descuento año nuevo (40% OFF)";
    public decimal AplicarDescuento(decimal montoBase) => montoBase * 0.60m;
}
