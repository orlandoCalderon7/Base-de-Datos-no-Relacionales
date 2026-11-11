using System;

namespace Sesion_05;

/// <summary>
/// Subtema 5 - Principio 1: Single Responsibility Principle (SRP)
/// Una clase debe tener una única razón para cambiar.
/// Se muestra la refactorización de una clase que hacía todo (anti-patrón)
/// hacia clases con responsabilidades únicas y bien delimitadas.
/// </summary>
public class PrincipioSRP
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 1. PRINCIPIO DE RESPONSABILIDAD ÚNICA (SRP)");
        Console.WriteLine("=======================================================");

        // Creación del objeto de negocio
        var orden = new Orden(101, "", 120.00m);

        // Cada clase se encarga estrictamente de su propósito:
        var validador = new OrdenValidador();
        var repositorio = new OrdenRepositorio();
        var impresor = new OrdenRecibo();

        if (validador.EsValida(orden))
        {
            repositorio.GuardarEnBaseDatos(orden);
            impresor.ImprimirTicket(orden);
        }
        else
        {
            Console.WriteLine("[Error]: La orden no cumple las reglas de validación.");
        }
    }
}

/// <summary>
/// 1. Responsabilidad: Representar la entidad de negocio y sus datos puros.
/// </summary>
public record Orden(int Id, string Cliente, decimal Monto);

/// <summary>
/// 2. Responsabilidad: Validar las reglas de negocio de una orden.
/// </summary>
public class OrdenValidador
{
    public bool EsValida(Orden orden)
    {
        return orden.Monto > 0 && !string.IsNullOrWhiteSpace(orden.Cliente);
    }
}

/// <summary>
/// 3. Responsabilidad: Persistir los datos de la orden en el almacenamiento.
/// </summary>
public class OrdenRepositorio
{
    public void GuardarEnBaseDatos(Orden orden)
    {
        Console.WriteLine($"[BD]: Orden #{orden.Id} guardada exitosamente para '{orden.Cliente}'.");
    }
}

/// <summary>
/// 4. Responsabilidad: Formatear y emitir la presentación física o digital del comprobante.
/// </summary>
public class OrdenRecibo
{
    public void ImprimirTicket(Orden orden)
    {
        Console.WriteLine($"[Ticket]: Imprimiendo comprobante de Orden #{orden.Id} por S/ {orden.Monto:F2}.");
    }
}
