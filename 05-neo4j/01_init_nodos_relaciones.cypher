// ============================================================================
// MEGAPSHOP - NEO4J 5.15 CYPHER INITIALIZATION
// Red de usuarios, interacciones sociales, compras y recomendaciones
// ============================================================================

// 1. Crear restricciones de unicidad e índices
CREATE CONSTRAINT unique_usuario_id IF NOT EXISTS FOR (u:Usuario) REQUIRE u.id IS UNIQUE;
CREATE CONSTRAINT unique_producto_id IF NOT EXISTS FOR (p:Producto) REQUIRE p.id IS UNIQUE;
CREATE CONSTRAINT unique_categoria_nombre IF NOT EXISTS FOR (c:Categoria) REQUIRE c.nombre IS UNIQUE;

// 2. Crear Nodos de prueba
MERGE (u1:Usuario {id: "USR-01", nombre: "Carlos Sanchez", ciudad: "Lima"})
MERGE (u2:Usuario {id: "USR-02", nombre: "Valeria Mendoza", ciudad: "Lima"})
MERGE (u3:Usuario {id: "USR-03", nombre: "Jorge Ramirez", ciudad: "Arequipa"})

MERGE (p1:Producto {id: "PROD-01", nombre: "Hamburguesa Doble Angus", precio: 34.50})
MERGE (p2:Producto {id: "PROD-02", nombre: "Papas Rusticas", precio: 16.00})
MERGE (p3:Producto {id: "PROD-03", nombre: "Gaseosa Artesanal", precio: 8.50})

MERGE (cat1:Categoria {nombre: "Fast Food Gourmet"})

// 3. Crear Relaciones
MERGE (p1)-[:PERTENECE_A]->(cat1)
MERGE (p2)-[:PERTENECE_A]->(cat1)

MERGE (u1)-[:COMPRO {fecha: "2026-08-10", calificacion: 5}]->(p1)
MERGE (u1)-[:COMPRO {fecha: "2026-08-10", calificacion: 4}]->(p2)
MERGE (u2)-[:COMPRO {fecha: "2026-08-11", calificacion: 5}]->(p1)

MERGE (u1)-[:SIGUE_A]->(u2)
MERGE (u3)-[:SIGUE_A]->(u1);

// 4. Consulta de Recomendación Social ("Quienes compraron esto también compraron...")
MATCH (u:Usuario {id: "USR-02"})-[:COMPRO]->(p:Producto)<-[:COMPRO]-(otro:Usuario)-[:COMPRO]->(rec:Producto)
WHERE NOT (u)-[:COMPRO]->(rec)
RETURN rec.nombre AS Recomendacion, count(*) AS Frecuencia
ORDER BY Frecuencia DESC;
