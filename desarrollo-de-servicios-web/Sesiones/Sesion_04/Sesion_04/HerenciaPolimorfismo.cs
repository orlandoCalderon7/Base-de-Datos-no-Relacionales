using System;
using System.Collections.Generic;

namespace Sesion_04;

/// <summary>
/// Tema 2: Herencia y Polimorfismo en C#
/// Demostración de clases base con métodos 'virtual', clases derivadas con 'override'
/// y resolución dinámica de polimorfismo en colecciones genéricas.
/// </summary>
public class HerenciaPolimorfismo
{
    public void probarHerenciaSimple()
    {
        Console.WriteLine("\n=== 1. HERENCIA BÁSICA ===");

        Perro miPerro = new Perro("Rex", "Pastor Alemán");
        Gato miGato = new Gato("Pelusa", 7);

        Console.WriteLine($"Perro: {miPerro.Nombre} | Raza: {miPerro.Raza}");
        miPerro.EmitirSonido();

        Console.WriteLine($"Gato: {miGato.Nombre} | Vidas: {miGato.Vidas}");
        miGato.EmitirSonido();
    }

    public void probarPolimorfismoColeccion()
    {
        Console.WriteLine("\n=== 2. POLIMORFISMO EN TIEMPO DE EJECUCIÓN ===");

        // Lista de tipo Mascota (clase base) que almacena diversas instancias hijas
        List<Mascota> refugio = [
            new Perro("Rex", "Pastor Alemán"),
            new Gato("Pelusa", 7),
            new Perro("Rocky", "Golden Retriever"),
            new Gato("Michi", 5),
            new Paloma("Piolin", "Nueva York"),
            new Mascota("Animal Silvestre")
        ];

        // El compilador ejecuta el método correspondiente a la instancia real (override)
        foreach (var m in refugio)
        {
            Console.Write($"[{m.GetType().Name}] {m.Nombre} -> ");
            m.EmitirSonido();
        }
    }
}

/// <summary>
/// Clase Base Mascota que define el contrato común y comportamiento por defecto.
/// Utiliza Primary Constructor de C# moderno.
/// </summary>
public class Mascota(string nombre)
{
    public string Nombre { get; } = nombre;

    // 'virtual' permite que clases hijas sobreescriban este método
    public virtual void EmitirSonido()
    {
        Console.WriteLine("Emite un sonido genérico de animal.");
    }
}

/// <summary>
/// Clase Hija Perro que hereda de Mascota y sobreescribe su sonido.
/// </summary>
public class Perro(string nombre, string raza) : Mascota(nombre)
{
    public string Raza { get; } = raza;

    // 'override' reemplaza la implementación base
    public override void EmitirSonido()
    {
        Console.WriteLine($"¡Guau! Soy un {Raza}.");
    }
}

/// <summary>
/// Clase Hija Gato que hereda de Mascota y sobreescribe su sonido.
/// </summary>
public class Gato(string nombre, int vidas) : Mascota(nombre)
{
    public int Vidas { get; } = vidas;

    public override void EmitirSonido()
    {
        Console.WriteLine($"¡Miau! Tengo {Vidas} vidas.");
    }
}


public class Paloma(string nombre, string ciudad) : Mascota(nombre)
{
    public string Ciudad { get; } = ciudad;

    public override void EmitirSonido()
    {
        Console.WriteLine($"¡Gruuu! Vico en {Ciudad}.");
    }
}
