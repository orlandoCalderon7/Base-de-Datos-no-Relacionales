using System;

namespace Sesion_05;

/// <summary>
/// Subtema 5 - Principios 4 y 5: Interface Segregation (ISP) y Dependency Inversion (DIP)
/// ISP: Los clientes no deben estar obligados a implementar métodos que no usan (interfaces pequeñas).
/// DIP: Los módulos de alto nivel deben depender de abstracciones (interfaces), no de clases concretas.
/// </summary>
public class PrincipioISP_DIP
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 3. PRINCIPIOS DE SEGREGACIÓN (ISP) E INVERSIÓN (DIP)");
        Console.WriteLine("=======================================================");

        var solicitud = new SolicitudAdopcion(201, "Rex", "adoptante.estrella@zegel.pe");

        // 1. Instanciamos el servicio inyectando la abstracción INotificadorServicio
        INotificadorServicio canalEmail = new EmailNotificadorServicio();
        var procesador = new AdopcionProcesador(canalEmail);

        procesador.Procesar(solicitud);

        // 2. DIP en acción: Si cambiamos a SMS o WhatsApp, el procesador NO cambia en absoluto
        Console.WriteLine("\n-> Cambiando canal de comunicación a SMS en tiempo de ejecución...");
        INotificadorServicio canalSms = new SmsNotificadorServicio();
        var procesadorSms = new AdopcionProcesador(canalSms);

        procesadorSms.Procesar(solicitud);

        // 3. Mock
        Console.WriteLine("\n-> Cambiando canal de comunicación a Mock en tiempo de ejecución...");
        INotificadorServicio canalMock = new MockNotificadorServicio();
        var procesadorMock = new AdopcionProcesador(canalMock);

        procesadorMock.Procesar(solicitud);

    }
}

public record SolicitudAdopcion(int Id, string Mascota, string Contacto);

/// <summary>
/// ISP: Interface pequeña y atómica orientada a una única responsabilidad.
/// </summary>
public interface INotificadorServicio
{
    void EnviarNotificacion(string destino, string mensaje); //Abstraccion
}

/// <summary>
/// Implementación concreta 1: Correo electrónico.
/// </summary>
public class EmailNotificadorServicio : INotificadorServicio
{
    public void EnviarNotificacion(string destino, string mensaje) //Implementacion
    {
        Console.WriteLine($"[EMAIL -> {destino}]: {mensaje}");
    }
}

/// <summary>
/// Implementación concreta 2: Mensaje de texto (SMS).
/// </summary>
public class SmsNotificadorServicio : INotificadorServicio
{
    public void EnviarNotificacion(string destino, string mensaje)
    {
        Console.WriteLine($"[SMS -> {destino}]: {mensaje}"); 
    }
}

public class MockNotificadorServicio : INotificadorServicio
{
    public void EnviarNotificacion(string destino, string mensaje)
    {
        Console.WriteLine($"[Mock -> {destino}]: {mensaje}");
    }
}

/// <summary>
/// DIP: Clase de alto nivel que orquesta el caso de uso.
/// Depende exclusivamente de 'INotificadorServicio' (Abstracción) inyectada en su constructor.
/// </summary>
public class AdopcionProcesador(INotificadorServicio notificador)
{
    public void Procesar(SolicitudAdopcion solicitud)
    {
        Console.WriteLine($"Validando requisitos para la adopción de {solicitud.Mascota} (Solicitud #{solicitud.Id})...");
        notificador.EnviarNotificacion(solicitud.Contacto, $"¡Tu solicitud de adopción para '{solicitud.Mascota}' fue APROBADA!");
    }
}
