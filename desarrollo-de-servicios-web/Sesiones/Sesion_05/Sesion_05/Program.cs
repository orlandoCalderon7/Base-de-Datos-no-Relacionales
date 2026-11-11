using System;
using Sesion_05;

// =========================================================================
// DESARROLLO DE SERVICIOS WEB - SESIÓN 05
// Tema: Principios SOLID y Clean Architecture en 4 Capas
// Ciclo 2026-II | Zegel - Escuela de Tecnología
// =========================================================================

Console.WriteLine("===========================================================");
Console.WriteLine("  SESIÓN 05: PRINCIPIOS SOLID Y CLEAN ARCHITECTURE (.NET 10)");
Console.WriteLine("===========================================================");

// 1. PRINCIPIO DE RESPONSABILIDAD ÚNICA (SRP)
PrincipioSRP srp = new PrincipioSRP();
//srp.ejecutarDemostracion();

// 2. PRINCIPIOS OPEN/CLOSED (OCP) Y LISKOV (LSP)
PrincipioOCP_LSP ocpLsp = new PrincipioOCP_LSP();
//ocpLsp.ejecutarDemostracion();

// 3. PRINCIPIOS DE SEGREGACIÓN (ISP) E INVERSIÓN (DIP)
PrincipioISP_DIP ispDip = new PrincipioISP_DIP();
//ispDip.ejecutarDemostracion();

// 4. INYECCIÓN DE DEPENDENCIAS (IoC) Y CICLOS DE VIDA (TRANSIENT, SCOPED, SINGLETON)
CiclosDeVidaIoC ioc = new CiclosDeVidaIoC();
//ioc.ejecutarDemostracion();

// 5. CLEAN ARCHITECTURE EN 4 CAPAS (CASO ADOPTAMAS)
CleanArchitectureDemo cleanArch = new CleanArchitectureDemo();
cleanArch.ejecutarDemostracion();
