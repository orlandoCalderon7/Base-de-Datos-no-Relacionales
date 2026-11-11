using System;
using System.Collections.Generic;

namespace Sesion_06;

/// <summary>
/// Subtema 6 - Concepto 4: Patrón ApiGateway con YARP (Yet Another Reverse Proxy)
/// Demostración de cómo el Gateway en el puerto unificado 6000 recibe todas las peticiones
/// de clientes externos y las enruta internamente a los microservicios sin exponer la topología privada.
/// </summary>
public class ApiGatewayYarp
{
    public void ejecutarDemostracion()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 4. PATRÓN APIGATEWAY CON YARP EN PUERTO UNIFICADO 6000");
        Console.WriteLine("=======================================================");

        var gateway = new YarpReverseProxySimulador();

        Console.WriteLine("Los clientes (Frontend Web, App Android/iOS, Postman) solo conocen: http://localhost:6000\n");

        // Simulación de peticiones entrantes al puerto 6000
        gateway.ProcesarPeticionEntrante("GET", "/api/mascotas/101");
        gateway.ProcesarPeticionEntrante("POST", "/api/adopciones");
        gateway.ProcesarPeticionEntrante("POST", "/api/auth/login");
        gateway.ProcesarPeticionEntrante("GET", "/api/reportes/anuales");

        Console.WriteLine("\n-> Conclusión Arquitectónica:");
        Console.WriteLine(" • Los clientes externos nunca tocan los puertos internos (5001, 5002, 5003).");
        Console.WriteLine(" • El Gateway centraliza autenticación JWT, políticas CORS, logging y balanceo de carga.");
    }
}

public class YarpReverseProxySimulador
{
    private readonly Dictionary<string, (string Cluster, string DestinoInterno)> _tablaEnrutamiento = new()
    {
        { "/api/mascotas",   ("mascotas-cluster",   "http://localhost:5001 (MascotasService)") },
        { "/api/adopciones", ("adopciones-cluster", "http://localhost:5002 (AdopcionesService)") },
        { "/api/auth",       ("auth-cluster",       "http://localhost:5003 (AuthService)") }
    };

    public void ProcesarPeticionEntrante(string metodoHttp, string path)
    {
        foreach (var ruta in _tablaEnrutamiento)
        {
            if (path.StartsWith(ruta.Key, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[YARP Gateway :6000] -> {metodoHttp} '{path}'");
                Console.WriteLine($"   Cluster: '{ruta.Value.Cluster}' ==> Redirigido a: {ruta.Value.DestinoInterno}");
                return;
            }
        }

        Console.WriteLine($"[YARP Gateway :6000] -> {metodoHttp} '{path}' ==> [HTTP 404 Not Found: Ruta no registrada]");
    }
}
