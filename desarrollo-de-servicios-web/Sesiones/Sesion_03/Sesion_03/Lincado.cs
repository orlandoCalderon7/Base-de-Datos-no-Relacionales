internal class Lincado
{
    public void mostrarLincado()
    {
        // Colección de origen
        var mascotas = new List<MascotaItem>
        {
            new(1, "Pelusa", "Gato", 6, 25.0m),
            new(2, "Rex", "Perro", 24, 60.0m),
            new(3, "Luna", "Perro", 4, 30.0m),
            new(4, "Michi", "Gato", 36, 20.0m)
        };

        // Pipeline LINQ: Filtrar cachorros (<12 meses), ordenar por edad y proyectar DTO
        var reporteCachorros = mascotas
            .Where(m => m.EdadMeses > 12)
            .Where(m => m.Especie == "Perro")
            .OrderBy(m => m.EdadMeses)
            .Select(m => $"Cachorro: {m.Nombre} ({m.Especie}) - {m.EdadMeses} meses")
            .ToList();

        var reporteCachorros2 = mascotas
            .Where(m => m.EdadMeses > 12)
            .OrderBy(m => m.EdadMeses)
            .Select(m => new MascotaItem(m.Id, m.Nombre, m.Especie, m.EdadMeses, m.Costo))
            .ToList();

        var nuevaBusqueda = reporteCachorros2
            .Where(m => m.Especie == "Perro")
            .OrderBy (m => m.EdadMeses)
            .Select(m => $"Cachorro: {m.Nombre} ({m.Especie}) - {m.EdadMeses} meses")
            .ToList();

        foreach (var linea in reporteCachorros)
            Console.WriteLine(linea);

        /*foreach (var linea in nuevaBusqueda)
            Console.WriteLine(linea);*/

    }

    public void mostrarDiferido()
    {
        var numeros = new List<int> { 1, 2, 3, 4, 5 };

        // 1. Consulta DIFERIDA: NO se ejecuta en este punto
        var paresQuery = numeros.Where(n => n % 2 == 0);
        numeros.Add(6); // Añadimos ANTES de iterar

        Console.WriteLine("Iteración de consulta diferida (incluye el 6):");
        foreach (var n in paresQuery)
            Console.Write($"{n} "); // Imprime 2 4 6
        Console.WriteLine();

        // 2. Materialización INMEDIATA con .ToList()
        var paresLista = numeros.Where(n => n % 2 == 0).ToList();
        numeros.Add(8); // Ya NO afecta a paresLista
        Console.WriteLine($"Lista materializada fija: {string.Join(", ", paresLista)}"); // 2, 4, 6

    }

}

