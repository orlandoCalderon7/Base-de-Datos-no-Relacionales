using System;

namespace Sesion_04;

/// <summary>
/// Tema 1: Abstracción y Encapsulamiento en C#
/// Demostración de modificadores de acceso, propiedades inmutables con 'init'
/// y validación estricta de invariantes de negocio en el constructor.
/// </summary>
public class Encapsulamiento
{
    public void probarEncapsulamientoBasico()
    {
        Console.WriteLine("\n=== 1. ABSTRACCIÓN Y ENCAPSULAMIENTO BÁSICO ===");

        // Instanciación controlada mediante constructor con validación
        var prod1 = new Producto(1, "Alimento Premium Perro", 85.50m, 20);
        var prod2 = new Producto(2, "Correa Ajustable Reforzada", 25.00m, 12);

        Console.WriteLine($"Producto 1: {prod1.Nombre} | Precio: S/ {prod1.Precio:F2} | Stock: {prod1.Stock}");
        Console.WriteLine($"Subtotal por 3 unidades: S/ {prod1.CalcularSubtotal(3):F2}");

        Console.WriteLine($"Producto 2: {prod2.Nombre} | Precio: S/ {prod2.Precio:F2} | Stock: {prod2.Stock}");
        Console.WriteLine($"Subtotal por 2 unidades: S/ {prod2.CalcularSubtotal(2):F2}");

        // NOTA: La siguiente línea daría error de compilación porque 'Precio' tiene accesor 'init'
        //prod1.Precio = -50; // Error CS8852: Init-only property cannot be assigned outside constructor
    }

    public void probarInvariantesValidacion()
    {
        Console.WriteLine("\n=== 2. CONTROL DE INVARIANTES Y PROTECCIÓN DE ESTADO ===");

        try
        {
            Console.WriteLine("Intentando crear un producto con precio inválido (S/ -15.00)...");
            var prodInvalido = new Producto(99, "Producto Defectuoso", -15.00m, 10);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Validación de Negocio]: {ex.Message}");
        }
    }
}

/// <summary>
/// Entidad Producto que representa un artículo del refugio / tienda.
/// Aplica propiedades inmutables tras inicialización ('init') y stock protegido.
/// </summary>
public class Producto
{

    public int Id { get; init; }
    public string Nombre { get; init; }
    public decimal Precio { get; init; }
    public int Stock { get; private set; }

    public Producto(int id, string nombre, decimal precio, int stock)
    {
        if (precio <= 0)
            throw new ArgumentException("El precio del producto debe ser un valor positivo.");

        Id = id;
        Nombre = nombre;
        Precio = precio;
        Stock = stock >= 0 ? stock : 0;
    }

    public decimal CalcularSubtotal(int cantidad)
    {
        if (cantidad <= 0) return 0;
        return Precio * cantidad;
    }
}
