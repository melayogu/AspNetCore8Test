# 完整的 Swagger 到 Newman 自動化測試腳本
param(
    [string]$BaseUrl = "http://localhost:5198",
    [switch]$DownloadSpec,
    [switch]$GenerateReport
)

Write-Host "🚀 Swagger 到 Newman 完整測試流程" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""

# 1. 檢查應用程式是否在運行
Write-Host "🔍 檢查應用程式狀態..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/swagger" -UseBasicParsing -TimeoutSec 5
    Write-Host "✅ 應用程式正在運行於 $BaseUrl" -ForegroundColor Green
} catch {
    Write-Host "❌ 應用程式未運行，請先啟動：" -ForegroundColor Red
    Write-Host "   cd AspNetCore8Test" -ForegroundColor White
    Write-Host "   dotnet run" -ForegroundColor White
    exit 1
}

# 2. 檢查 Swagger 端點
Write-Host "📄 檢查 Swagger/OpenAPI 端點..." -ForegroundColor Yellow
try {
    $swaggerJson = Invoke-RestMethod -Uri "$BaseUrl/swagger/v1/swagger.json"
    Write-Host "✅ OpenAPI 規格可用" -ForegroundColor Green
    Write-Host "   版本: $($swaggerJson.info.version)" -ForegroundColor Cyan
    Write-Host "   端點數量: $($swaggerJson.paths.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ 無法取得 OpenAPI 規格" -ForegroundColor Red
    exit 1
}

# 3. 可選：下載 OpenAPI 規格
if ($DownloadSpec) {
    Write-Host "⬇️  下載 OpenAPI 規格..." -ForegroundColor Yellow
    $swaggerJson | ConvertTo-Json -Depth 10 | Out-File "openapi-spec.json" -Encoding UTF8
    Write-Host "✅ OpenAPI 規格已儲存至 openapi-spec.json" -ForegroundColor Green
}

# 4. 檢查 Newman 和 Postman Collection
Write-Host "🧪 檢查測試環境..." -ForegroundColor Yellow
try {
    $newmanVersion = newman --version
    Write-Host "✅ Newman 版本: $newmanVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Newman 未安裝，請執行：npm install -g newman" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path "Products-API.postman_collection.json")) {
    Write-Host "❌ 找不到 Postman Collection 文件" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path "development.postman_environment.json")) {
    Write-Host "❌ 找不到環境設定文件" -ForegroundColor Red
    exit 1
}

# 5. 執行 Newman 測試
Write-Host "🚀 執行 Newman API 測試..." -ForegroundColor Yellow
Write-Host ""

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$reportArgs = @()

if ($GenerateReport) {
    $reportArgs = @(
        "--reporters", "cli,html,json",
        "--reporter-html-export", "newman-report-$timestamp.html",
        "--reporter-json-export", "newman-report-$timestamp.json"
    )
}

$newmanArgs = @(
    "run", "Products-API.postman_collection.json",
    "--environment", "development.postman_environment.json",
    "--color"
) + $reportArgs

& newman @newmanArgs

$exitCode = $LASTEXITCODE

# 6. 測試結果摘要
Write-Host ""
Write-Host "📊 測試完成摘要" -ForegroundColor Green
Write-Host "=================" -ForegroundColor Green

if ($exitCode -eq 0) {
    Write-Host "✅ 所有測試通過！" -ForegroundColor Green
} else {
    Write-Host "⚠️  部分測試失敗，請檢查上述結果" -ForegroundColor Yellow
}

if ($GenerateReport) {
    Write-Host "📄 測試報告已生成：" -ForegroundColor Cyan
    Write-Host "   HTML: newman-report-$timestamp.html" -ForegroundColor White
    Write-Host "   JSON: newman-report-$timestamp.json" -ForegroundColor White
    
    $openReport = Read-Host "是否要開啟 HTML 報告？ (y/n)"
    if ($openReport -eq "y" -or $openReport -eq "Y") {
        Start-Process "newman-report-$timestamp.html"
    }
}

# 7. 工作流程說明
Write-Host ""
Write-Host "🔄 完整工作流程摘要" -ForegroundColor Green
Write-Host "===================" -ForegroundColor Green
Write-Host "1. ✅ ASP.NET Core App 運行中" -ForegroundColor White
Write-Host "2. ✅ Swagger UI 可用: $BaseUrl/swagger" -ForegroundColor White
Write-Host "3. ✅ OpenAPI JSON: $BaseUrl/swagger/v1/swagger.json" -ForegroundColor White
Write-Host "4. ✅ Postman Collection 測試執行完成" -ForegroundColor White
Write-Host "5. ✅ Newman 自動化測試結果可用" -ForegroundColor White

Write-Host ""
Write-Host "🛠 下次使用方式：" -ForegroundColor Yellow
Write-Host "  基本測試：   .\full-api-test.ps1" -ForegroundColor White
Write-Host "  生成報告：   .\full-api-test.ps1 -GenerateReport" -ForegroundColor White
Write-Host "  下載規格：   .\full-api-test.ps1 -DownloadSpec" -ForegroundColor White
Write-Host "  自定義URL：  .\full-api-test.ps1 -BaseUrl 'https://api.example.com'" -ForegroundColor White

exit $exitCode
