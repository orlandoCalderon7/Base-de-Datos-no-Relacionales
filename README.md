# 🛒 MegaShop — Proyecto Integrador de Cátedra
## Bases de Datos no Relacionales | Escuela de Tecnología — Zegel (Ciclo 2026-II)
**Docente Titular:** Ing. Félix Rafael Contreras Martínez

---

## 📁 Estructura del Repositorio de Laboratorio

```text
proyecto-megashop/
├── docker-compose.yml              # Orquestación de MongoDB 7.0, Redis 7.2, Cassandra 4.1 y Neo4j 5.15
├── README.md                       # Guía maestra de ejecución y comandos
├── 01-infraestructura/             # Comandos de inicio, verificación y salud
│   ├── start-lab.ps1 / .sh
│   └── stop-lab.ps1 / .sh
├── 02-mongodb/                     # Scripts BSON / mongosh (Catálogo, Pedidos y Agregaciones)
│   ├── 01_init_schema_validation.js
│   ├── 02_seed_catalogo_pedidos.js
│   ├── 03_consultas_crud_avanzadas.js
│   └── 04_aggregation_pipeline_reportes.js
├── 03-redis/                       # Scripts Clave-Valor / redis-cli (Caché, Carritos, TTL, Rankings)
│   ├── 01_sesiones_y_tokens.redis
│   ├── 02_carritos_compra_hashes.redis
│   └── 03_ranking_repartidores_sorted_sets.redis
├── 04-cassandra/                   # Scripts Columnares / cqlsh (Historial masivo de transacciones)
│   ├── 01_init_keyspace_tables.cql
│   └── 02_seed_transacciones_queries.cql
├── 05-neo4j/                       # Scripts de Grafos / Cypher (Red social, recomendaciones y antifraude)
│   ├── 01_init_nodos_relaciones.cypher
│   └── 02_queries_recomendaciones.cypher
└── datasets/                       # Archivos JSON de prueba (≥ 200 registros para Parcial, ≥ 500 para Final)
    ├── catalogo_productos_200.json
    └── clientes_pedidos_100.json
```

---

## 🚀 Inicio Rápido de la Infraestructura

### 1. Iniciar los contenedores con Docker Compose
```bash
docker compose up -d
```

### 2. Verificar estado de los contenedores
```bash
docker compose ps
```

### 3. Puertos y Servicios Disponibles
| Servicio | Motor NoSQL | Puerto Host | Interfaz / Cliente |
| :--- | :--- | :--- | :--- |
| **mongo-megashop** | MongoDB 7.0 Community | `27017` | `mongosh` / MongoDB Compass |
| **redis-megashop** | Redis 7.2 Stack Server | `6379` | `redis-cli` / RedisInsight (`http://localhost:8001`) |
| **cassandra-megashop** | Apache Cassandra 4.1 | `9042` | `cqlsh` / DBeaver |
| **neo4j-megashop** | Neo4j 5.15 Community | `7474`, `7687` | Neo4j Browser (`http://localhost:7474`) |

---

## 🧪 Ejecución de Scripts por Motor

### MongoDB (`mongosh`):
```bash
# Conectar a la consola interactiva de MongoDB
docker exec -it mongo-megashop mongosh -u admin -p zegel2026 --authenticationDatabase admin

# O ejecutar un script directamente
docker exec -i mongo-megashop mongosh -u admin -p zegel2026 --authenticationDatabase admin < 02-mongodb/01_init_schema_validation.js
```

### Redis (`redis-cli`):
```bash
# Conectar a Redis CLI
docker exec -it redis-megashop redis-cli -a redis2026

# Ejecutar script por pipeline
docker exec -i redis-megashop redis-cli -a redis2026 < 03-redis/01_sesiones_y_tokens.redis
```

### Cassandra (`cqlsh`):
```bash
docker exec -it cassandra-megashop cqlsh -u cassandra -p cassandra
```

### Neo4j (Browser / Cypher):
Acceder mediante el navegador web a [http://localhost:7474](http://localhost:7474) con credenciales:
* **Usuario:** `neo4j`
* **Contraseña:** `zegel2026`
