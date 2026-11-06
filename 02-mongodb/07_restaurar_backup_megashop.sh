#!/bin/bash
# ============================================================================
# MEGAPSHOP - SCRIPT DE RESTAURACIÓN SEGURA DE BACKUP (MONGODB 7.0)
# Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
# ============================================================================

set -e

CONTAINER_NAME="mongo-megashop"
MONGO_USER="admin"
MONGO_PASS="zegel2026"
BACKUP_DIR="$(dirname "$0")/backups"

BACKUP_FILE="$1"
if [ -z "$BACKUP_FILE" ]; then
  # Buscar el backup más reciente
  BACKUP_FILE=$(ls -t "${BACKUP_DIR}"/backup_*.gz 2>/dev/null | head -n 1)
  if [ -z "$BACKUP_FILE" ]; then
    echo "❌ Error: No se encontraron respaldos en ${BACKUP_DIR}"
    exit 1
  fi
fi

echo "============================================================================"
echo "🔄 RESTAURANDO DESDE: ${BACKUP_FILE}"
echo "============================================================================"

docker exec -i "${CONTAINER_NAME}" mongorestore \
  --username "${MONGO_USER}" \
  --password "${MONGO_PASS}" \
  --authenticationDatabase admin \
  --gzip \
  --archive \
  --drop < "${BACKUP_FILE}"

echo "✅ Restauración completada exitosamente."
