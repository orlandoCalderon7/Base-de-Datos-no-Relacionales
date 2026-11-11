using System;

namespace Sesion_04;

/// <summary>
/// Tema 3: Contratos Desacoplados mediante Interfaces
/// Demostración de bajo acoplamiento: un servicio de adopciones consume
/// el contrato INotificador sin importar si la entrega es por Email, SMS o WhatsApp.
/// </summary>
public class ContratosInterfaces
{
    public void probarNotificadorEmail()
    {
        Console.WriteLine("\n=== 1. CONTRATO CON IMPLEMENTACIÓN EMAIL ===");

        INotificador notificador = new EmailNotificador("adopciones@zegel.pe");
        var servicio = new AdopcionServicio(notificador);

        servicio.AprobarSolicitud("Rex", "Juan Pérez", "juan.perez@zegel.pe");
    }

    public void probarIntercambioServicio()
    {
        Console.WriteLine("\n=== 2. INTERCAMBIO DE IMPLEMENTACIÓN SIN TOCAR EL CONSUMIDOR ===");

        // Cambiamos el mecanismo a SMS pasando simplemente una nueva instancia
        INotificador notificadorSms = new SmsNotificador("+51 987654321");
        var servicio = new AdopcionServicio(notificadorSms);

        servicio.AprobarSolicitud("Pelusa", "María Gómez", "987111222");
    }
}

/// <summary>
/// Contrato puro que define qué acción debe realizarse, sin acoplar la tecnología.
/// </summary>
public interface INotificador
{
    void EnviarMensaje(string destinatario, string mensaje);
}

/// <summary>
/// Implementación concreta para envío de correo electrónico.
/// </summary>
public class EmailNotificador(string remitente) : INotificador
{
    public void EnviarMensaje(string destinatario, string mensaje)
    {
        Console.WriteLine($"[EMAIL desde {remitente} -> Para: {destinatario}]: {mensaje}");
    }
}

/// <summary>
/// Implementación concreta para envío de mensajes de texto (SMS).
/// </summary>
public class SmsNotificador(string numeroCentral) : INotificador
{
    public void EnviarMensaje(string destinatario, string mensaje)
    {
        Console.WriteLine($"[SMS desde {numeroCentral} -> Para: {destinatario}]: {mensaje}");
    }
}

/// <summary>
/// Servicio de Adopción que depende de la ABSTRACCIÓN (INotificador) y no de clases concretas.
/// Principio de Inversión de Dependencias (DIP).
/// </summary>
public class AdopcionServicio(INotificador notificador)
{
    public void AprobarSolicitud(string nombreMascota, string nombreAdoptante, string canalContacto)
    {
        Console.WriteLine($"-> Aprobando formalmente la adopción de {nombreMascota}...");
        notificador.EnviarMensaje(canalContacto, $"¡Felicidades {nombreAdoptante}! Tu adopción de {nombreMascota} fue aprobada.");
    }
}
