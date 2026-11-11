using System;
using Sesion_04;

// =========================================================================
// DESARROLLO DE SERVICIOS WEB - SESIÓN 04
// Tema: Programación Orientada a Objetos en C# y Taller ShopNow (T01)
// Ciclo 2026-II | Zegel - Escuela de Tecnología
// =========================================================================

Console.WriteLine("===========================================================");
Console.WriteLine("  SESIÓN 04: PROGRAMACIÓN ORIENTADA A OBJETOS EN .NET CORE");
Console.WriteLine("===========================================================");

// 1. ABSTRACCIÓN Y ENCAPSULAMIENTO
Encapsulamiento encapsulamiento = new Encapsulamiento();
//encapsulamiento.probarEncapsulamientoBasico();
//encapsulamiento.probarInvariantesValidacion();

// 2. HERENCIA Y POLIMORFISMO
HerenciaPolimorfismo herencia = new HerenciaPolimorfismo();
//herencia.probarHerenciaSimple();
//herencia.probarPolimorfismoColeccion();

// 3. CONTRATOS DESACOPLADOS MEDIANTE INTERFACES
ContratosInterfaces contratos = new ContratosInterfaces();
//contratos.probarNotificadorEmail();
//contratos.probarIntercambioServicio();

// 4. C# MODERNO: RECORDS Y CONSTRUCTORES PRIMARIOS
RecordsModernos records = new RecordsModernos();
//records.probarIgualdadPorValor();
//records.probarClonacionWith();

// 5. TALLER OFICIAL T01: GUÍA PRÁCTICA 1 (CASO SHOPNOW)
LaboratorioShopNow shopNow = new LaboratorioShopNow();
shopNow.ejecutarCasoShopNow();
