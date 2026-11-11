using System;
using System.Collections.Generic;
using System.Linq;

namespace Sesion_04;

/// <summary>
/// Tema 5: Taller Oficial Zegel: T01 — Guía Práctica 1 (Caso ShopNow)
/// Implementación integral de las entidades del dominio de comercio electrónico:
/// Cliente, ProductoItem, Pedido, cálculo de subtotales con LINQ y notificación desacoplada.
/// </summary>
public class LaboratorioShopNow
{
    public void ejecutarCasoShopNow()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TALLER OFICIAL T01: GUÍA PRÁCTICA 1 (CASO SHOPNOW)");
        Console.WriteLine("=======================================================");

        // 1. Configuramos el notificador desacoplado
        INotificadorShopNow notificador = new EmailNotificadorShopNow("pedidos@shopnow.pe");

        // 2. Creamos al cliente de la plataforma
        var cliente = new ClienteShopNow(101, "Ana Torres", "ana.torres@zegel.pe");

        // 3. Creamos el pedido vinculando cliente y notificador
        var pedido = new PedidoShopNow(5001, cliente, notificador);

        // 4. Agregamos artículos al pedido
        pedido.AgregarItem(new ProductoItem(1, "Laptop Gamer Core i7", 3450.00m, 1));
        pedido.AgregarItem(new ProductoItem(2, "Mouse Inalámbrico RGB", 75.50m, 2));
        pedido.AgregarItem(new ProductoItem(3, "Pad Mouse Ergonómico", 30.00m, 1));

        // 5. Procesamos y confirmamos el pedido
        pedido.ConfirmarPedido();
    }
}

/// <summary>
/// DTO inmutable del cliente de ShopNow.
/// </summary>
public record ClienteShopNow(int Id, string Nombre, string Email);

/// <summary>
/// Ítem de producto dentro del carrito / pedido con cálculo polimórfico de subtotal.
/// </summary>
public record ProductoItem(int Id, string Descripcion, decimal PrecioUnitario, int Cantidad)
{
    public decimal Subtotal => PrecioUnitario * Cantidad;
}

/// <summary>
/// Contrato de notificación desacoplado para ShopNow.
/// </summary>
public interface INotificadorShopNow
{
    void Notificar(string destino, string mensaje);
}

/// <summary>
/// Servicio de notificación por correo.
/// </summary>
public class EmailNotificadorShopNow(string remitente) : INotificadorShopNow
{
    public void Notificar(string destino, string mensaje)
    {
        Console.WriteLine($"[EMAIL ShopNow desde {remitente} -> Para: {destino}]: {mensaje}");
    }
}

/// <summary>
/// Agregado raíz Pedido que encapsula la lista de ítems y las reglas de facturación.
/// </summary>
public class PedidoShopNow(int id, ClienteShopNow cliente, INotificadorShopNow notificador)
{
    public int Id { get; } = id;
    public ClienteShopNow Cliente { get; } = cliente;
    private readonly List<ProductoItem> _items = [];

    public void AgregarItem(ProductoItem item)
    {
        if (item.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        _items.Add(item);
    }

    public decimal Total => _items.Sum(i => i.Subtotal);

    public void ConfirmarPedido()
    {
        Console.WriteLine($"\n-> Registrando Pedido #{Id} en plataforma ShopNow...");
        Console.WriteLine($"Cliente: {Cliente.Nombre} ({Cliente.Email})");
        Console.WriteLine("-------------------------------------------------------");
        foreach (var item in _items)
        {
            Console.WriteLine($" • {item.Descripcion,-28} x{item.Cantidad,2} = S/ {item.Subtotal,8:F2}");
        }
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($" TOTAL A PAGAR: S/ {Total:F2}");
        Console.WriteLine("-------------------------------------------------------");

        // Disparo de notificación desacoplada
        notificador.Notificar(Cliente.Email, $"Tu pedido #{Id} por S/ {Total:F2} fue aprobado exitosamente.");
    }
}
