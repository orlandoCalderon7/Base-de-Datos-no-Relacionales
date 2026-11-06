# ============================================================================
# MEGAPSHOP - SCRIPT DE RESTAURACIÓN SEGURA DE BACKUP (POWERSHELL)
# Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
# ============================================================================
# Restaura el backup más reciente o un archivo específico con el flag --drop
# ============================================================================

param(
    [string]$ArchivoBackup = ""
)

$ContainerName = "mongo-megashop"
$MongoUser = "admin"
$MongoPass = "zegel2026"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$BackupDir = Join-Path $ScriptDir "backups"

if (-not $ArchivoBackup) {
    # Tomar el backup más reciente
    $Ultimo = Get-ChildItem -Path $BackupDir -Filter "backup_*.gz" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if (-not $Ultimo) {
        Write-Error "No se encontraron archivos de respaldo en $BackupDir"
        exit 1
    }
    $ArchivoBackup = $Ultimo.FullName
}

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "🔄 INICIANDO RESTAURACIÓN DESDE: $(Split-Path -Leaf $ArchivoBackup)" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan

Get-Content $ArchivoBackup -Raw -Encoding Byte | docker exec -i $ContainerName mongorestore `
  --username $MongoUser `
  --password $MongoPass `
  --authenticationDatabase admin `
  --gzip `
  --archive `
  --drop

Write-Host "✅ Restauración completada con éxito." -ForegroundColor Green
