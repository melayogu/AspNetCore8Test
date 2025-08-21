# 🔄 Swagger → Newman 執行流程

## 核心流程

```
📝 開發 API
    ↓
🚀 啟動應用程式 (dotnet run)
    ↓
📄 Swagger 自動生成文檔 (/swagger)
    ↓
📋 OpenAPI JSON 規格 (/swagger/v1/swagger.json)
    ↓
🧪 Postman Collection (手動建立或自動轉換)
    ↓
🤖 Newman 自動化測試
    ↓
📊 測試報告 (HTML/JSON)
```

## 實際執行命令

### 1. 啟動應用程式
```bash
cd AspNetCore8Test
dotnet run
# → 應用程式運行於 http://localhost:5198
# → Swagger UI: http://localhost:5198/swagger
# → OpenAPI JSON: http://localhost:5198/swagger/v1/swagger.json
```

### 2. 查看 Swagger 文檔
- 開啟瀏覽器：`http://localhost:5198/swagger`
- 測試 API 端點
- 查看請求/回應格式

### 3. 執行 Newman 測試
```bash
cd newman-tests

# 基本測試
newman run Products-API.postman_collection.json --environment development.postman_environment.json

# 完整測試 (含報告)
.\full-api-test.ps1 -GenerateReport
```

## 檔案說明

| 檔案 | 用途 | 來源 |
|------|------|------|
| `swagger.json` | OpenAPI 規格 | Swagger 自動生成 |
| `Products-API.postman_collection.json` | 測試集合 | 手動建立 |
| `development.postman_environment.json` | 環境變數 | 手動建立 |
| `newman-report.html` | 測試報告 | Newman 生成 |

## 優勢

✅ **自動化文檔**: Swagger 從程式碼自動生成  
✅ **標準化測試**: Postman Collection 格式  
✅ **CI/CD 整合**: Newman 命令列工具  
✅ **詳細報告**: HTML/JSON 多種格式  
✅ **版本控制**: 所有檔案可納入 Git  

## 更新流程

當您修改 API 時：

1. **修改 Controller** → Swagger 自動更新
2. **重啟應用程式** → 新的 OpenAPI 規格
3. **更新測試** (如需要) → 修改 Postman Collection
4. **執行測試** → Newman 驗證功能

這就是現代 API 開發的標準流程！🚀
