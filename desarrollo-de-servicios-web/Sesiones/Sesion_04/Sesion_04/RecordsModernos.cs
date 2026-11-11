using System;

namespace Sesion_04;

/// <summary>
/// Tema 4: C# Moderno: Tipos Record y Constructores Primarios
/// Demostración de igualdad basada en valores (Value Equality), inmutabilidad
/// posicional y clonación no destructiva con el operador 'with'.
/// </summary>
public class RecordsModernos
{
    public void probarIgualdadPorValor()
    {
        Console.WriteLine("\n=== 1. IGUALDAD POR VALOR (VALUE EQUALITY) ===");

        // Dos instancias distintas en memoria con los mismos datos
        var dto1 = new MascotaDto(101, "Rex", "Perro", 24, 45.00m);
        var dto2 = new MascotaDto(101, "Rex", "Perro", 24, 45.00m);

        // En una 'class' común, == compararía referencias (False).
        // En un 'record', el compilador genera Equals basado en todas las propiedades (True).
        Console.WriteLine($"dto1 == dto2: {dto1 == dto2}  (¡Igualdad por valor en Records!)");
        Console.WriteLine($"object.ReferenceEquals(dto1, dto2): {object.ReferenceEquals(dto1, dto2)}  (Distinta dirección en Heap)");
    }

    public void probarClonacionWith()
    {
        Console.WriteLine("\n=== 2. CLONACIÓN NO DESTRUCTIVA CON OPERADOR 'with' ===");

        var dtoOriginal = new MascotaDto(102, "Pelusa", "Gato", 6, 35.00m);

        // 'with' crea un nuevo objeto copiando todo excepto lo que sobreescribimos
        var dtoActualizado = dtoOriginal with { Costo = 50.00m, EdadMeses = 7 };

        Console.WriteLine($"Original:    {dtoOriginal}");
        Console.WriteLine($"Con 'with':  {dtoActualizado}");
        Console.WriteLine($"¿Misma referencia?: {object.ReferenceEquals(dtoOriginal, dtoActualizado)}");
    }
}

/// <summary>
/// DTO Inmutable declarado con Primary Constructor en una sola línea.
/// Genera automáticamente: Equals, GetHashCode, ToString() y operador with.
/// </summary>
public record MascotaDto(int Id, string Nombre, string Especie, int EdadMeses, decimal Costo);
