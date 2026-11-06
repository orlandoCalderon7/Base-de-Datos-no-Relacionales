#!/bin/bash
# ============================================================================
# MEGAPSHOP - SCRIPT AUTOMÁTICO DE BACKUP PERIÓDICO (MONGODB 7.0)
# Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
# ============================================================================
# Automatiza respaldos lógicos consistentes en BSON comprimido con gzip.
# Diseñado para ejecutarse manualmente o de forma programada vía cron.
#
# Para programar en crontab diariamente a las 02:00 AM:
# crontab -e
# 0 2 * * * /ruta/a/06_backup_automatico_megashop.sh >> /var/log/megashop_backup.log 2>&1
# ============================================================================

set -e

# Configuración
CONTAINER_NAME="mongo-megashop"
MONGO_USER="admin"
MONGO_PASS="zegel2026"
MONGO_AUTH_DB="admin"
TARGET_DB="megashop_db"

BACKUP_DIR="$(dirname "$0")/backups"
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
BACKUP_FILE="${BACKUP_DIR}/backup_${TARGET_DB}_${TIMESTAMP}.gz"
RETENTION_DAYS=7

mkdir -p "${BACKUP_DIR}"

echo "============================================================================"
echo "🕒 INICIANDO BACKUP AUTOMÁTICO DE '${TARGET_DB}' - $(date)"
echo "============================================================================"

# Ejecutar mongodump dentro del contenedor con compresión gzip y stream único
START_TIME=$(date +%s)

docker exec -t "${CONTAINER_NAME}" mongodump \
  --username "${MONGO_USER}" \
  --password "${MONGO_PASS}" \
  --authenticationDatabase "${MONGO_AUTH_DB}" \
  --db "${TARGET_DB}" \
  --gzip \
  --archive > "${BACKUP_FILE}"

END_TIME=$(date +%s)
DURATION=$((END_TIME - START_TIME))
FILE_SIZE=$(du -h "${BACKUP_FILE}" | cut -f1)

echo "✅ BACKUP GENERADO EXITOSAMENTE:"
echo "   • Archivo de Respaldo : ${BACKUP_FILE}"
echo "   • Tamaño Comprimido   : ${FILE_SIZE}"
echo "   • Duración            : ${DURATION} segundos"

# Política de Retención: Eliminar backups con más de 7 días de antigüedad
echo "🧹 Aplicando política de retención (${RETENTION_DAYS} días)..."
find "${BACKUP_DIR}" -name "backup_${TARGET_DB}_*.gz" -type f -mtime +${RETENTION_DAYS} -exec rm -f {} \;
echo "✅ Limpieza de backups obsoletos finalizada."
echo "============================================================================"
