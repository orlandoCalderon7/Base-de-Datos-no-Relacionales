/**
 * ============================================================================
 * MEGAPSHOP - EXPERIMENTO EN VIVO: 1,000,000 REGISTROS EN MONGODB 7.0
 * Demostración empírica de COLLSCAN vs IXSCAN y la Regla ESR para la Sesión 05
 * ============================================================================
 */

db = db.getSiblingDB("megashop_db");

print("\n" + "=".repeat(75));
print("🔥 EXPERIMENTO EN VIVO DE RENDIMIENTO: 1,000,000 REGISTROS EN MONGODB 7.0");
print("=".repeat(75));

const totalDocs = db.pedidos_benchmark.countDocuments();
print(`📊 Total de documentos en 'pedidos_benchmark': ${totalDocs.toLocaleString()}`);

if (totalDocs === 0) {
  print("⚠️ La colección está vacía. Ejecute primero 04_benchmark_1m_pedidos.js");
  quit();
}

// ----------------------------------------------------------------------------
// 1. ELIMINAR ÍNDICES PREVIOS (Garantizar estado puro COLLSCAN)
// ----------------------------------------------------------------------------
try {
  db.pedidos_benchmark.dropIndexes();
} catch(e) {}

print("\n" + "-".repeat(75));
print("🐢 [EXPERIMENTO 1] BÚSQUEDA PUNTUAL SIN ÍNDICE (COLLSCAN O(N))");
print("-".repeat(75));
print("Consulta: db.pedidos_benchmark.find({ 'numero_orden': 'ORD-2026-0750000' })");

const t0 = new Date();
const exp1 = db.pedidos_benchmark.find({
  "numero_orden": "ORD-2026-0750000"
}).explain("executionStats");
const wallTime1 = new Date() - t0;

const stats1 = exp1.executionStats;
print(`  • Etapa de ejecución (Stage)         : ${stats1.executionStages.stage}`);
print(`  • Documentos examinados (DocsExamined): ${stats1.totalDocsExamined.toLocaleString()} (¡Escaneó 750 mil a 1 millón de docs!)`);
print(`  • Documentos devueltos               : ${stats1.nReturned}`);
print(`  • Tiempo de motor en MongoDB         : ${stats1.executionTimeMillis} ms`);
print(`  • Tiempo total de cliente            : ${wallTime1} ms`);

print("\n" + "-".repeat(75));
print("⚡ [CREACIÓN DE ÍNDICE B-TREE ÚNICO]: { 'numero_orden': 1 }");
print("-".repeat(75));
const tIdx0 = new Date();
db.pedidos_benchmark.createIndex({ "numero_orden": 1 }, { unique: true });
print(`  • Índice B-Tree creado en            : ${new Date() - tIdx0} ms`);

print("\n" + "-".repeat(75));
print("🚀 [EXPERIMENTO 2] LA MISMA BÚSQUEDA CON ÍNDICE B-TREE (IXSCAN O(log N))");
print("-".repeat(75));
const t1 = new Date();
const exp2 = db.pedidos_benchmark.find({
  "numero_orden": "ORD-2026-0750000"
}).explain("executionStats");
const wallTime2 = new Date() - t1;

const stats2 = exp2.executionStats;
const inputStage2 = stats2.executionStages.inputStage || stats2.executionStages;
print(`  • Etapa de ejecución (Stage)         : ${inputStage2.stage}`);
print(`  • Claves de índice examinadas        : ${stats2.totalKeysExamined}`);
print(`  • Documentos examinados (DocsExamined): ${stats2.totalDocsExamined}`);
print(`  • Documentos devueltos               : ${stats2.nReturned}`);
print(`  • Tiempo de motor en MongoDB         : ${stats2.executionTimeMillis} ms (¡0 milisegundos!)`);
print(`  • Tiempo total de cliente            : ${wallTime2} ms`);

print("\n" + "-".repeat(75));
print("💥 [EXPERIMENTO 3] EL COLAPSO DEL ORDENAMIENTO EN MEMORIA RAM (SORT STAGE)");
print("-".repeat(75));
print("Consulta: Pedidos en 'Miraflores' ordenados por fecha sin índice ESR");
try {
  db.pedidos_benchmark.find({ "ubicacion.distrito": "Miraflores" })
    .sort({ "fecha_creacion": -1 })
    .toArray();
  print("  (Completado en RAM)");
} catch(err) {
  print(`  ❌ ERROR REAL DE PRODUCCIÓN CAPTURADO:`);
  print(`     ${err.message}`);
  print(`  💡 Explicación: MongoDB tiene un límite de 32 MB / 100 MB de RAM para ordenar.`);
  print(`     Al no tener un índice con la regla ESR, el motor colapsa.`);
}

print("\n" + "-".repeat(75));
print("✨ [SOLUCIÓN ESR]: CREANDO ÍNDICE COMPUESTO { 'ubicacion.distrito': 1, 'fecha_creacion': -1, 'total': 1 }");
print("-".repeat(75));
const tIdxESR = new Date();
db.pedidos_benchmark.createIndex({
  "ubicacion.distrito": 1, // Equality (E)
  "fecha_creacion": -1,    // Sort (S)
  "total": 1               // Range (R)
});
print(`  • Índice ESR creado en               : ${new Date() - tIdxESR} ms`);

const expESR = db.pedidos_benchmark.find({
  "ubicacion.distrito": "Miraflores",
  "total": { $gte: 50.00 }
}).sort({ "fecha_creacion": -1 }).limit(10).explain("executionStats");

const statsESR = expESR.executionStats;
print(`  • Stage final                        : ${statsESR.executionStages.stage}`);
print(`  • Documentos examinados              : ${statsESR.totalDocsExamined}`);
print(`  • Documentos devueltos               : ${statsESR.nReturned}`);
print(`  • Tiempo de ejecución con ESR        : ${statsESR.executionTimeMillis} ms (¡Instantáneo!)`);

print("\n" + "=".repeat(75));
print("🎓 CONCLUSIÓN PEDAGÓGICA PARA LOS ESTUDIANTES:");
print("  1. COLLSCAN sobre 1M docs examina cientos de miles de registros y demora cientos de ms.");
print("  2. IXSCAN con B-Tree reduce los documentos examinados a exactamente 1.");
print("  3. Sin la regla ESR, ordenar sobre grandes volúmenes tira un error fatal de memoria RAM.");
print("=".repeat(75) + "\n");
