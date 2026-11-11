internal class BusquedaSegura
{
    public void primero()
    {
        var mascotas = new List<MascotaItem>
        {
            new(1, "Pelusa", "Gato", 6, 25.0m),
            new(2, "Rex", "Perro", 24, 60.0m),
            new(3, "Luna", "Perro", 8, 30.0m),
            new(4, "Michi", "Gato", 36, 20.0m)
        };

        // 1. Búsqueda segura con FirstOrDefault (evita excepciones)
        var busqueda = mascotas.FirstOrDefault(m => m.Nombre == "Toby");
        Console.WriteLine($"Búsqueda 'Toby': {(busqueda is not null ? busqueda.Nombre : "No encontrado")}");

        // 2. Agrupación por Especie y Estadísticas de Negocio
        var resumenEspecies = mascotas
            .GroupBy(m => m.Especie)
            .Select(g => new {
                Especie = g.Key,
                Total = g.Count(),
                PromedioEdad = g.Average(m => m.EdadMeses),
                CostoTotal = g.Sum(m => m.Costo)
            });

        foreach (var r in resumenEspecies)
            Console.WriteLine($"[{r.Especie}] Cantidad: {r.Total} | Edad Promedio: {r.PromedioEdad:F1} m | Total: S/ {r.CostoTotal:F2}");

        
    }
}