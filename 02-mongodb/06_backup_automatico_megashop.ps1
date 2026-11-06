# ============================================================================
# MEGAPSHOP - SCRIPT AUTOMÁTICO DE BACKUP PERIÓDICO PARA WINDOWS (POWERSHELL)
# Bases de Datos no Relacionales | Zegel (Ciclo 2026-II)
# ============================================================================
# Ejecuta un respaldo comprimido .gz y aplica retención automática de 7 días.
# Puede ser programado en el Programador de Tareas de Windows (Task Scheduler).
# ============================================================================

$ErrorActionPreference = "Stop"

$ContainerName = "mongo-megashop"
$MongoUser = "admin"
$MongoPass = "zegel2026"
$TargetDb = "megashop_db"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$BackupDir = Join-Path $ScriptDir "backups"
if (-not (Test-Path $BackupDir)) {
    New-Item -ItemType Directory -Path $BackupDir | Out-Null
}

$Timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$BackupFile = Join-Path $BackupDir "backup_${TargetDb}_${Timestamp}.gz"
$RetentionDays = 7

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "🕒 INICIANDO BACKUP AUTOMÁTICO DE '$TargetDb' - $(Get-Date)" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan

$Stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

# Ejecutar mongodump dentro del contenedor redirigiendo stdout al archivo .gz
& docker exec -i $ContainerName mongodump `
  --username $MongoUser `
  --password $MongoPass `
  --authenticationDatabase admin `
  --db $TargetDb `
  --gzip `
  --archive > $BackupFile

$Stopwatch.Stop()
$FileSizeMB = [Math]::Round(((Get-Item $BackupFile).Length / 1MB), 2)

Write-Host "✅ BACKUP GENERADO EXITOSAMENTE:" -ForegroundColor Green
Write-Host "   • Archivo de Respaldo : $BackupFile" -ForegroundColor Green
Write-Host "   • Tamaño Comprimido   : $FileSizeMB MB" -ForegroundColor Green
Write-Host "   • Duración            : $($Stopwatch.Elapsed.TotalSeconds.ToString('0.00')) segundos" -ForegroundColor Green

# Política de retención: Eliminar backups con más de 7 días
Write-Host "🧹 Aplicando política de retención ($RetentionDays días)..." -ForegroundColor Yellow
$CutoffDate = (Get-Date).AddDays(-$RetentionDays)
Get-ChildItem -Path $BackupDir -Filter "backup_${TargetDb}_*.gz" | Where-Object { $_.LastWriteTime -lt $CutoffDate } | Remove-Item -Force
Write-Host "✅ Limpieza de backups obsoletos finalizada." -ForegroundColor Green
Write-Host "============================================================================" -ForegroundColor Cyan
