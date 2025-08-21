@echo off
echo ====================================
echo Running Newman API Tests
echo ====================================
echo.

echo Starting Newman tests...
newman run "Products-API.postman_collection.json" ^
    --environment "development.postman_environment.json" ^
    --reporters cli,json,html ^
    --reporter-html-export "newman-report.html" ^
    --reporter-json-export "newman-report.json" ^
    --verbose

echo.
echo ====================================
echo Newman Tests Completed
echo ====================================
echo Report generated: newman-report.html
echo JSON Report: newman-report.json
pause
