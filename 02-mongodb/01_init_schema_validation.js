/**
 * ============================================================================
 * MEGAPSHOP - MONGODB 7.0 INITIALIZATION & SCHEMA VALIDATION
 * Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
 * ============================================================================
 */

db = db.getSiblingDB("megashop_db");

print("--- Inicializando colecciones con esquema de validacion $jsonSchema ---");

// 1. Coleccion: comercios
db.createCollection("comercios", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["nombre", "categoria", "ruc", "direccion", "activo"],
      properties: {
        nombre: { bsonType: "string", description: "Nombre comercial obligatorio" },
        categoria: { enum: ["Restaurante", "Supermercado", "Farmacia", "Tecnologia"], description: "Categoria permitida" },
        ruc: { bsonType: "string", pattern: "^[0-9]{11}$", description: "RUC peruano de 11 digitos" },
        direccion: {
          bsonType: "object",
          required: ["distrito", "ciudad"],
          properties: {
            distrito: { bsonType: "string" },
            ciudad: { bsonType: "string" },
            coordenadas: {
              bsonType: "array",
              minItems: 2,
              maxItems: 2,
              items: { bsonType: "double" }
            }
          }
        },
        activo: { bsonType: "bool" }
      }
    }
  }
});

// 2. Coleccion: productos (Catálogo Polimórfico)
db.createCollection("productos", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["comercio_id", "sku", "nombre", "precio", "stock"],
      properties: {
        comercio_id: { bsonType: "objectId" },
        sku: { bsonType: "string" },
        nombre: { bsonType: "string" },
        precio: { bsonType: "number", minimum: 0 },
        stock: { bsonType: "int", minimum: 0 },
        especificaciones: { bsonType: "object" },
        tags: { bsonType: "array", items: { bsonType: "string" } }
      }
    }
  }
});

// 3. Coleccion: pedidos (Documentos con subdocumentos embebidos)
db.createCollection("pedidos", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["numero_orden", "cliente_id", "comercio_id", "items", "total", "estado", "fecha_creacion"],
      properties: {
        numero_orden: { bsonType: "string" },
        cliente_id: { bsonType: "string" },
        comercio_id: { bsonType: "objectId" },
        items: {
          bsonType: "array",
          minItems: 1,
          items: {
            bsonType: "object",
            required: ["producto_id", "nombre", "cantidad", "precio_unitario", "subtotal"],
            properties: {
              producto_id: { bsonType: "objectId" },
              nombre: { bsonType: "string" },
              cantidad: { bsonType: "int", minimum: 1 },
              precio_unitario: { bsonType: "number", minimum: 0 },
              subtotal: { bsonType: "number", minimum: 0 }
            }
          }
        },
        total: { bsonType: "number", minimum: 0 },
        estado: { enum: ["PENDIENTE", "CONFIRMADO", "EN_PREPARACION", "EN_CAMINO", "ENTREGADO", "CANCELADO"] },
        fecha_creacion: { bsonType: "date" }
      }
    }
  }
});

// 4. Índices para optimización de consultas
db.comercios.createIndex({ categoria: 1, activo: 1 });
db.productos.createIndex({ comercio_id: 1, precio: 1 });
db.productos.createIndex({ sku: 1 }, { unique: true });
db.pedidos.createIndex({ numero_orden: 1 }, { unique: true });
db.pedidos.createIndex({ cliente_id: 1, fecha_creacion: -1 });
db.pedidos.createIndex({ estado: 1, fecha_creacion: -1 });

print("✅ Colecciones e indices creados exitosamente en megashop_db.");
