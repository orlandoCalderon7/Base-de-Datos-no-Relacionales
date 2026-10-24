-- ============================================================================
-- MEGAPSHOP - APACHE CASSANDRA 4.1 CQL INITIALIZATION
-- Historial masivo de transacciones y telemetría de pedidos
-- ============================================================================

-- 1. Crear Keyspace con factor de replicación para clúster
CREATE KEYSPACE IF NOT EXISTS megashop_analytics
WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};

USE megashop_analytics;

-- 2. Tabla: Historial de Transacciones (Partition Key: comercio_id, Clustering Key: fecha_transaccion DESC)
CREATE TABLE IF NOT EXISTS transacciones_por_comercio (
    comercio_id uuid,
    fecha_transaccion timestamp,
    transaccion_id uuid,
    numero_orden text,
    monto_total decimal,
    metodo_pago text,
    estado text,
    PRIMARY KEY ((comercio_id), fecha_transaccion, transaccion_id)
) WITH CLUSTERING ORDER BY (fecha_transaccion DESC, transaccion_id ASC);

-- 3. Tabla: Telemetría GPS de Repartidores (Series Temporales)
CREATE TABLE IF NOT EXISTS telemetria_repartidor (
    repartidor_id uuid,
    fecha text, -- YYYY-MM-DD
    hora_registro timestamp,
    latitud double,
    longitud double,
    bateria_pct int,
    velocidad_kmh double,
    PRIMARY KEY ((repartidor_id, fecha), hora_registro)
) WITH CLUSTERING ORDER BY (hora_registro DESC);
