using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sesion_06;

/// <summary>
/// Subtema 6 - Concepto 3: Tolerancia a Fallos y Resiliencia en Microservicios
/// A diferencia del monolito (donde un fallo tumba toda la aplicación),
/// en microservicios un servicio caído se aísla mediante mecanismos de Fallback.
/// </summary>
public class ResilienciaYFallas
{
    public async Task ejecutarDemostracionAsync()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 3. TOLERANCIA A FALLOS Y RESILIENCIA (FALLBACK)");
        Console.WriteLine("=======================================================");

        var notificadorProxy = new NotificacionesProxyResiliente();

        Console.WriteLine("Escenario: El microservicio secundario de Notificaciones (:5003) se ha caído.");
        Console.WriteLine("Intentando confirmar adopción de la mascota 'Rex'...");

        await notificadorProxy.EnviarConfirmacionAsync("juan.perez@zegel.pe", "¡Tu solicitud para Rex fue aprobada!");

        Console.WriteLine("\n-> Conclusión Arquitectónica:");
        Console.WriteLine(" • En un monolito: Una excepción no capturada colapsa el proceso completo.");
        Console.WriteLine(" • En microservicios: El flujo principal de Adopción tuvo éxito, y el fallo secundario se encoló.");
    }
}

public class NotificacionesProxyResiliente
{
    public Task EnviarConfirmacionAsync(string emailDestino, string mensaje)
    {
        try
        {
            // Simulamos fallo de red o caída del microservicio secundario
            bool servicioSecundarioActivo = false;

            if (!servicioSecundarioActivo)
            {
                throw new HttpRequestException("HTTP 503 Service Unavailable: No se pudo conectar con NotificacionesService en el puerto 5003.");
            }

            Console.WriteLine($"[NotificacionesService]: Mensaje enviado con éxito a {emailDestino}.");
        }
        catch (HttpRequestException ex)
        {
            // Estrategia de Resiliencia / Fallback:
            Console.WriteLine($"[ALERTA DE RESILIENCIA]: {ex.Message}");
            Console.WriteLine($"[ACCION FALLBACK]: El mensaje se guardó en la cola local de reintentos (Outbox Pattern).");
            Console.WriteLine($"[ESTADO PRINCIPAL]: La adopción continúa procesándose normalmente.");
        }

        return Task.CompletedTask;
    }
}
