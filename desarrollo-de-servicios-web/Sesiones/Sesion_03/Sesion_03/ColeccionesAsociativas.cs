internal class ColeccionesAsociativas
{
    public void devolverDiccionario()
    {
        // 1. Dictionary<TKey, TValue>: Búsqueda ultra rápida O(1) por ID
        var catalogoPrecios = new Dictionary<int, decimal>
        {
            [101] = 45.50m, // MascotaId -> Costo Vacunación
            [102] = 80.00m,
            [103] = 35.00m
        };
        int idBuscar = 103;
        if (catalogoPrecios.TryGetValue(idBuscar, out decimal precio))
            Console.WriteLine($"Precio Mascota #{idBuscar}: S/ {precio:F2}");

    }

    public void devolverHash()
    {
        // 2. HashSet<T>: Garantiza unicidad absoluta (sin duplicados)
        var especiesRegistradas = new HashSet<string> { "Perro", "Gato" };
        bool agregoDuplicado = especiesRegistradas.Add("Perro"); // Retorna false (ignorado)
        especiesRegistradas.Add("Loro");
        Console.WriteLine($"Especies únicas ({especiesRegistradas.Count}): {string.Join(", ", especiesRegistradas)}");
    }
}


