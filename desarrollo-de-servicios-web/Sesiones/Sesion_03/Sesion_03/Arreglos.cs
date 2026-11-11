using System;
using System.Collections.Generic;
using System.Text;

internal class Arreglos
{
    public void mostrarArreglo()
    {
        // 1. Arreglo de tamaño fijo (T[]): 3 elementos inmutables en longitud
        string[] especiesFijas = ["Perro", "Gato", "Conejo"];
        Console.WriteLine($"Arreglo: longitud={especiesFijas.Length}, primero={especiesFijas[0]}");
        //especiesFijas[3] = "Ave"; // Error: IndexOutOfRangeException!
    }

    public void mostrarListaDinamica()
    {
        // 2. Lista dinámica (List<T>): crece automáticamente en memoria
        List<string> listaMascotas = ["Pelusa", "Rex"];
        listaMascotas.Add("Rocky");
        listaMascotas.Add("Luna");      // Crece dinámicamente
        listaMascotas.Remove("Pelusa"); // Remueve elemento

        Console.WriteLine($"Lista dinámica: Total={listaMascotas.Count}");

        foreach (string mascota in listaMascotas)
        {
            Console.WriteLine($" - Mascota: {mascota}");
        }
    }

}

