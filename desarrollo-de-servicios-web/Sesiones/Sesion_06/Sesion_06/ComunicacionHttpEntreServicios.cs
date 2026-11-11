using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sesion_06;

/// <summary>
/// Subtema 6 - Concepto 2: Comunicación Síncrona HTTP/REST entre Microservicios
/// Demostración de cómo un servicio (AdopcionesService) consulta a otro servicio (MascotasService)
/// a través de una petición HTTP GET recibiendo y deserializando un JSON.
/// </summary>
public class ComunicacionHttpEntreServicios
{
    public async Task ejecutarDemostracionAsync()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 2. COMUNICACIÓN SÍNCRONA HTTP/JSON ENTRE SERVICIOS");
        Console.WriteLine("=======================================================");

        int idBuscado = 101;
        Console.WriteLine($"[AdopcionesService]: Necesito verificar si la mascota #{idBuscado} está disponible.");
        Console.WriteLine($"[AdopcionesService -> HTTP GET http://localhost:5001/api/mascotas/{idBuscado}]...");

        var cliente = new MascotaHttpClientSimulado();
        var mascota = await cliente.ObtenerMascotaPorIdAsync(idBuscado);

        Console.WriteLine($"[MascotasService -> HTTP 200 OK]: JSON recibido exitosamente:");
        Console.WriteLine($" • Mascota: {mascota.Nombre} ({mascota.Especie}) | Estado: '{mascota.Estado}'");

        if (mascota.Estado == "Disponible")
        {
            Console.WriteLine("-> Validación de negocio aprobada: La mascota puede ser adoptada.");
        }
        else
        {
            Console.WriteLine("-> Validación de negocio rechazada: La mascota no está disponible.");
        }
    }
}

/// <summary>
/// Cliente HTTP que simula la invocación a la Web API de Mascotas (:5001).
/// </summary>
public class MascotaHttpClientSimulado
{
    public Task<MascotaItem> ObtenerMascotaPorIdAsync(int id)
    {
        // En un controlador real de .NET se usaría:
        // return await _httpClient.GetFromJsonAsync<MascotaItem>($"http://localhost:5001/api/mascotas/{id}");

        // Simulamos la respuesta JSON devuelta por MascotasService
        string jsonServidor = $"{{\"Id\": {id}, \"Nombre\": \"Rex\", \"Especie\": \"Perro\", \"EdadMeses\": 24, \"Estado\": \"Disponible\"}}";
        
        var entidad = JsonSerializer.Deserialize<MascotaItem>(jsonServidor);
        return Task.FromResult(entidad!);
    }
}
