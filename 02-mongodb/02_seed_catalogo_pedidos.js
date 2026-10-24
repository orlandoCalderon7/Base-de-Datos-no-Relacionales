/**
 * ============================================================================
 * MEGAPSHOP - MONGODB 7.0 SEED DATASET GENERATOR
 * Genera comercios, catálogo de productos y pedidos distribuidos
 * ============================================================================
 */

db = db.getSiblingDB("megashop_db");

print("--- Insertando datos semilla en MegaShop ---");

// Limpiar datos anteriores
db.pedidos.deleteMany({});
db.productos.deleteMany({});
db.comercios.deleteMany({});

// 1. Insertar Comercios
const r1 = db.comercios.insertOne({
  nombre: "Burger Master Gourmet",
  categoria: "Restaurante",
  ruc: "20123456789",
  direccion: { distrito: "Miraflores", ciudad: "Lima", coordenadas: [-12.1215, -77.0305] },
  activo: true
});

const r2 = db.comercios.insertOne({
  nombre: "SuperMarket Express",
  categoria: "Supermercado",
  ruc: "20987654321",
  direccion: { distrito: "San Isidro", ciudad: "Lima", coordenadas: [-12.0975, -77.0360] },
  activo: true
});

const r3 = db.comercios.insertOne({
  nombre: "PharmaSalud 24h",
  categoria: "Farmacia",
  ruc: "20555666777",
  direccion: { distrito: "Surco", ciudad: "Lima", coordenadas: [-12.1450, -76.9950] },
  activo: true
});

const c1_id = r1.insertedId;
const c2_id = r2.insertedId;
const c3_id = r3.insertedId;

// 2. Insertar Catálogo de Productos
const prodDocs = [
  { comercio_id: c1_id, sku: "BM-001", nombre: "Hamburguesa Doble Angus", precio: 34.50, stock: 50, especificaciones: { calorias: 850, tipo_carne: "Angus" }, tags: ["hamburguesa", "gourmet", "angus"] },
  { comercio_id: c1_id, sku: "BM-002", nombre: "Papas Rústicas con Queso Cheddar", precio: 16.00, stock: 80, especificaciones: { porcion: "Grande" }, tags: ["papas", "sides"] },
  { comercio_id: c1_id, sku: "BM-003", nombre: "Gaseosa Artesanal 500ml", precio: 8.50, stock: 120, especificaciones: { sabor: "Maracuyá" }, tags: ["bebidas"] },
  { comercio_id: c2_id, sku: "SM-101", nombre: "Pack Leche Entera 1L x6", precio: 28.90, stock: 200, especificaciones: { volumen_total: "6L" }, tags: ["lacteos", "desayuno"] },
  { comercio_id: c2_id, sku: "SM-102", nombre: "Aceite de Oliva Extra Virgen 500ml", precio: 32.00, stock: 45, especificaciones: { origen: "Espana" }, tags: ["abarrotes", "gourmet"] },
  { comercio_id: c3_id, sku: "PH-201", nombre: "Paracetamol 500mg (Caja 100 tab)", precio: 12.50, stock: 300, especificaciones: { laboratorio: "Genfar" }, tags: ["medicamentos", "analgesicos"] },
  { comercio_id: c3_id, sku: "PH-202", nombre: "Bloqueador Solar SPF 50+ 200ml", precio: 68.00, stock: 60, especificaciones: { proteccion: "UVA/UVB" }, tags: ["cuidado-personal"] }
];

const insertedProds = db.productos.insertMany(prodDocs);
print(`✅ ${prodDocs.length} productos insertados en catálogo.`);

// 3. Generar pedidos de prueba
const p1_id = Object.values(insertedProds.insertedIds)[0];
const p2_id = Object.values(insertedProds.insertedIds)[1];

db.pedidos.insertMany([
  {
    numero_orden: "ORD-2026-0001",
    cliente_id: "CLI-9901",
    comercio_id: c1_id,
    items: [
      { producto_id: p1_id, nombre: "Hamburguesa Doble Angus", cantidad: 2, precio_unitario: 34.50, subtotal: 69.00 },
      { producto_id: p2_id, nombre: "Papas Rústicas con Queso Cheddar", cantidad: 1, precio_unitario: 16.00, subtotal: 16.00 }
    ],
    total: 85.00,
    estado: "ENTREGADO",
    fecha_creacion: new Date("2026-08-10T19:30:00Z")
  },
  {
    numero_orden: "ORD-2026-0002",
    cliente_id: "CLI-9902",
    comercio_id: c1_id,
    items: [
      { producto_id: p1_id, nombre: "Hamburguesa Doble Angus", cantidad: 1, precio_unitario: 34.50, subtotal: 34.50 }
    ],
    total: 34.50,
    estado: "EN_CAMINO",
    fecha_creacion: new Date()
  }
]);

print("✅ Pedidos de prueba insertados.");
