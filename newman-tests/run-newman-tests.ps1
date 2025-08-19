# Newman API 測試執行腳本
Write-Host "====================================" -ForegroundColor Green
Write-Host "Running Newman API Tests" -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green
Write-Host ""

# 檢查 Newman 是否已安裝
try {
    $newmanVersion = newman --version
    Write-Host "Newman version: $newmanVersion" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Newman not found. Please install it with: npm install -g newman" -ForegroundColor Red
    exit 1
}

Write-Host "Starting Newman tests..." -ForegroundColor Yellow

# 執行 Newman 測試
newman run "Products-API.postman_collection.json" `
    --environment "development.postman_environment.json" `
    --reporters cli,json,html `
    --reporter-html-export "newman-report.html" `
    --reporter-json-export "newman-report.json" `
    --verbose

Write-Host ""
Write-Host "====================================" -ForegroundColor Green
Write-Host "Newman Tests Completed" -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green
Write-Host "📊 HTML Report generated: newman-report.html" -ForegroundColor Cyan
Write-Host "📄 JSON Report generated: newman-report.json" -ForegroundColor Cyan

# 詢問是否要開啟報告
$openReport = Read-Host "Do you want to open the HTML report? (y/n)"
if ($openReport -eq "y" -or $openReport -eq "Y") {
    Start-Process "newman-report.html"
}
