/**
 * ============================================================================
 * MEGAPSHOP - MONGODB 7.0: GENERADOR MASIVO PARA BENCHMARK EN VIVO
 * Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
 * Genera hasta 1,000,000 de documentos en la colección 'pedidos_benchmark'
 * para medir en tiempo real la diferencia abismal entre COLLSCAN y IXSCAN.
 * ============================================================================
 */

db = db.getSiblingDB("megashop_db");

function generarBenchmark(totalDocs = 100000) {
  print(`\n======================================================`);
  print(`🚀 INICIANDO GENERACIÓN MASIVA DE ${totalDocs.toLocaleString()} REGISTROS`);
  print(`======================================================`);

  const coll = db.pedidos_benchmark;
  
  // Limpiar colección previa para mediciones limpias
  coll.drop();

  const distritos = [
    "Miraflores", "San Isidro", "Surco", "San Borja", "Barranco",
    "La Molina", "Jesus Maria", "Lince", "Magdalena", "Pueblo Libre",
    "Los Olivos", "San Miguel", "Independencia", "Comas", "Ate"
  ];

  const estados = ["PENDIENTE", "PAGADO", "EN_CAMINO", "ENTREGADO", "CANCELADO"];
  const productosBase = [
    { nombre: "Burger Doble Angus", precio: 34.50 },
    { nombre: "Papas Rusticas", precio: 16.00 },
    { nombre: "Gaseosa Artesanal", precio: 8.50 },
    { nombre: "Pollo a la Brasa Familiar", precio: 68.00 },
    { nombre: "Pizza Suprema Familiar", precio: 45.00 },
    { nombre: "Pack Bebidas Energeticas", precio: 22.00 }
  ];

  const batchSize = 10000;
  const totalBatches = Math.ceil(totalDocs / batchSize);
  const startTime = new Date();

  let globalCount = 0;

  for (let b = 1; b <= totalBatches; b++) {
    const docs = [];
    const currentBatchLimit = Math.min(batchSize, totalDocs - globalCount);

    for (let i = 0; i < currentBatchLimit; i++) {
      globalCount++;
      const dist = distritos[Math.floor(Math.random() * distritos.length)];
      const est = estados[Math.floor(Math.random() * estados.length)];
      const prod = productosBase[Math.floor(Math.random() * productosBase.length)];
      const cant = Math.floor(Math.random() * 4) + 1;
      const subtotal = Math.round(prod.precio * cant * 100) / 100;
      const total = subtotal + Math.round((Math.random() * 20) * 100) / 100;

      // Fecha aleatoria en los últimos 180 días de 2026
      const randomDaysAgo = Math.floor(Math.random() * 180);
      const fecha = new Date(Date.now() - randomDaysAgo * 24 * 60 * 60 * 1000);

      docs.push({
        numero_orden: `ORD-2026-${String(globalCount).padStart(7, '0')}`,
        cliente_id: `CLI-${Math.floor(1000 + Math.random() * 9000)}`,
        ubicacion: {
          distrito: dist,
          ciudad: "Lima"
        },
        estado: est,
        total: total,
        fecha_creacion: fecha,
        items: [
          {
            nombre: prod.nombre,
            cantidad: cant,
            precio_unitario: prod.precio,
            subtotal: subtotal
          }
        ],
        metodo_pago: Math.random() > 0.4 ? "TARJETA" : "YAPE_PLIN"
      });
    }

    coll.insertMany(docs, { ordered: false });
    const percent = Math.round((globalCount / totalDocs) * 100);
    print(`   📦 Lote ${b}/${totalBatches} insertado [${globalCount.toLocaleString()} / ${totalDocs.toLocaleString()}] (${percent}%)`);
  }

  const durationSec = ((new Date() - startTime) / 1000).toFixed(2);
  print(`\n✅ GENERACIÓN COMPLETADA EXITOSAMENTE:`);
  print(`   • Total documentos en 'pedidos_benchmark': ${coll.countDocuments().toLocaleString()}`);
  print(`   • Tiempo transcurrido: ${durationSec} segundos`);
  print(`   • Tamaño en disco/RAM: ${(coll.stats().size / (1024 * 1024)).toFixed(2)} MB`);
  print(`======================================================\n`);
}

// Por defecto ejecutamos con 100,000 registros para demo rápida.
// Si el docente desea 1,000,000, puede llamar: generarBenchmark(1000000)
var totalArg = typeof TOTAL_DOCS !== 'undefined' ? TOTAL_DOCS : 100000;
generarBenchmark(totalArg);
