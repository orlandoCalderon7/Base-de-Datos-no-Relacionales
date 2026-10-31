/**
 * ============================================================================
 * MEGAPSHOP - MONGODB 7.0 SCRIPT 03: CONSULTAS Y MANIPULACIÓN CRUD AVANZADA
 * Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
 * ============================================================================
 */

db = db.getSiblingDB("megashop_db");

print("--- EJECUTANDO CONSULTAS Y OPERACIONES CRUD EN MEGAPSHOP ---");

// ============================================================================
// 1. OPERACIONES CREATE (Inserciones individuales y masivas)
// ============================================================================
print("\n[1] Insertando nuevo comercio y productos...");

const nuevoComercio = db.comercios.insertOne({
  nombre: "Polleria Criolla Real",
  categoria: "Restaurante",
  ruc: "20888999111",
  direccion: { distrito: "Miraflores", ciudad: "Lima", coordenadas: [-12.1220, -77.0310] },
  activo: true
});

db.productos.insertMany([
  {
    comercio_id: nuevoComercio.insertedId,
    sku: "PO-001",
    nombre: "1/4 Pollo a la Brasa con Papas y Ensalada",
    precio: 24.50,
    stock: NumberInt(40),
    especificaciones: { guarnicion: "Papas fritas", bebida_incluida: false },
    tags: ["pollo", "brasas", "menu-ejecutivo"]
  },
  {
    comercio_id: nuevoComercio.insertedId,
    sku: "PO-002",
    nombre: "Jarra Chicha Morada Casera 1L",
    precio: 12.00,
    stock: NumberInt(60),
    especificaciones: { presentacion: "Jarra 1L" },
    tags: ["bebidas", "artesanal"]
  }
]);

// ============================================================================
// 2. OPERACIONES READ: Filtros por Operadores de Rango y Conjuntos
// ============================================================================
print("\n[2] Consultando productos con precio > 25.00:");
const productosCaros = db.productos.find(
  { precio: { $gt: 25.00 } },
  { _id: 0, sku: 1, nombre: 1, precio: 1, stock: 1 }
).toArray();
printjson(productosCaros);

print("\n[3] Consultando productos que tengan tags 'gourmet' o 'brasas':");
const productosTags = db.productos.find(
  { tags: { $in: ["gourmet", "brasas"] } },
  { _id: 0, sku: 1, nombre: 1, tags: 1 }
).toArray();
printjson(productosTags);

// ============================================================================
// 3. OPERACIONES EN SUBDOCUMENTOS Y ARRAYS ($elemMatch)
// ============================================================================
print("\n[4] Consultando pedidos con notación de punto (distrito Miraflores):");
const comerciosMiraflores = db.comercios.find(
  { "direccion.distrito": "Miraflores" },
  { _id: 0, nombre: 1, categoria: 1, "direccion.distrito": 1 }
).toArray();
printjson(comerciosMiraflores);

print("\n[5] Consulta con $elemMatch: Pedidos con items de precio >= 30 y cantidad >= 1:");
const pedidosGrandes = db.pedidos.find({
  items: {
    $elemMatch: {
      precio_unitario: { $gte: 30.00 },
      cantidad: { $gte: 1 }
    }
  }
}, { _id: 0, numero_orden: 1, total: 1, estado: 1, items: 1 }).toArray();
printjson(pedidosGrandes);

// ============================================================================
// 4. OPERACIONES UPDATE: Modificaciones Atómicas ($set, $inc, $push)
// ============================================================================
print("\n[6] Actualizando estado de pedido ORD-2026-0002 a 'ENTREGADO' con $set:");
db.pedidos.updateOne(
  { numero_orden: "ORD-2026-0002" },
  {
    $set: {
      estado: "ENTREGADO",
      fecha_entrega: new Date(),
      calificacion_cliente: 5
    }
  }
);

print("\n[7] Incrementando stock de PO-001 en +10 unidades con $inc:");
db.productos.updateOne(
  { sku: "PO-001" },
  { $inc: { stock: NumberInt(10) } }
);

print("\n[8] Agregando nueva etiqueta a PO-001 con $push:");
db.productos.updateOne(
  { sku: "PO-001" },
  { $push: { tags: "oferta-semanal" } }
);

// ============================================================================
// 5. OPERACIONES DELETE: Eliminación Segura
// ============================================================================
print("\n[9] Contando pedidos cancelados antes de limpiar:");
const cancelados = db.pedidos.countDocuments({ estado: "CANCELADO" });
print(`Total pedidos cancelados: ${cancelados}`);

if (cancelados > 0) {
  const result = db.pedidos.deleteMany({ estado: "CANCELADO" });
  print(`Pedidos eliminados: ${result.deletedCount}`);
}

print("\n--- SCRIPT 03 FINALIZADO CON ÉXITO ---");
