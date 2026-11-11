using System;
using System.Threading.Tasks;
using Sesion_06;

// =========================================================================
// DESARROLLO DE SERVICIOS WEB - SESIÓN 06
// Tema: Introducción a Microservicios con .NET Core y Patrón ApiGateway
// Ciclo 2026-II | Zegel - Escuela de Tecnología
// =========================================================================

Console.WriteLine("===========================================================");
Console.WriteLine("  SESIÓN 06: ARQUITECTURA DE MICROSERVICIOS CON .NET 10");
Console.WriteLine("===========================================================");

// 1. BOUNDED CONTEXT Y DOMINIOS INDEPENDIENTES (Diapositivas 103 y 104)
ArquitecturaMicroservicios microservicios = new ArquitecturaMicroservicios();
microservicios.ejecutarDemostracion();

// 2. COMUNICACIÓN SÍNCRONA HTTP/REST ENTRE SERVICIOS (Diapositivas 105 y 106)
ComunicacionHttpEntreServicios comunicacion = new ComunicacionHttpEntreServicios();
//await comunicacion.ejecutarDemostracionAsync();

// 3. TOLERANCIA A FALLOS Y RESILIENCIA (Diapositivas 107 y 108)
ResilienciaYFallas resiliencia = new ResilienciaYFallas();
//await resiliencia.ejecutarDemostracionAsync();

// 4. PATRÓN APIGATEWAY CON YARP EN PUERTO 6000 (Diapositivas 109 y 110)
ApiGatewayYarp gateway = new ApiGatewayYarp();
//gateway.ejecutarDemostracion();
