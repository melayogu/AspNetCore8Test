# Newman API 測試設置

這個資料夾包含了使用 Newman 測試 AspNetCore8Test Products API 的完整設置。

## 檔案說明

- **Products-API.postman_collection.json**: Postman 集合文件，包含所有 API 測試
- **development.postman_environment.json**: 開發環境變數設定
- **run-newman-tests.bat**: Windows 批次執行腳本
- **run-newman-tests.ps1**: PowerShell 執行腳本

## 安裝需求

確保您已經安裝了以下工具：

1. **Node.js** (已安裝 ✅)
2. **Newman**: 
   ```bash
   npm install -g newman
   ```

## 使用方法

### 方法 1: 使用 PowerShell 腳本 (推薦)
```powershell
cd newman-tests
.\run-newman-tests.ps1
```

### 方法 2: 使用批次檔
```cmd
cd newman-tests
run-newman-tests.bat
```

### 方法 3: 直接使用 Newman 命令
```bash
cd newman-tests
newman run Products-API.postman_collection.json --environment development.postman_environment.json
```

## 測試內容

這個 Postman 集合包含以下測試：

1. **Get All Products** - 取得所有商品列表
2. **Create Product** - 建立新商品
3. **Get Product by ID** - 根據 ID 取得特定商品
4. **Update Product** - 更新商品資訊
5. **Delete Product** - 刪除商品
6. **Get Product by ID (After Delete)** - 驗證商品已被刪除
7. **Create Product with Invalid Data** - 測試驗證錯誤處理

## 測試功能

每個測試都包含：
- ✅ **狀態碼驗證**
- ✅ **回應內容驗證**
- ✅ **效能測試** (回應時間)
- ✅ **資料完整性檢查**
- ✅ **錯誤處理測試**

## 報告生成

Newman 執行後會產生以下報告：
- **newman-report.html**: 詳細的 HTML 測試報告
- **newman-report.json**: JSON 格式的測試結果

## 前置條件

確保您的 ASP.NET Core 應用程式正在執行：
```bash
cd AspNetCore8Test
dotnet run
```

應用程式應該在 `http://localhost:5198` 上運行。

## 自訂設定

### 修改基礎 URL
編輯 `development.postman_environment.json` 文件中的 `baseUrl` 值：
```json
{
  "key": "baseUrl",
  "value": "http://localhost:5198",
  "enabled": true
}
```

### 添加新測試
您可以直接編輯 `Products-API.postman_collection.json` 文件來添加新的測試案例。

## 持續整合 (CI/CD)

這個設置可以輕易整合到 CI/CD 管道中：

```yaml
# GitHub Actions 範例
- name: Run Newman Tests
  run: |
    npm install -g newman
    newman run newman-tests/Products-API.postman_collection.json \
      --environment newman-tests/development.postman_environment.json \
      --reporters cli,json \
      --reporter-json-export newman-test-results.json
```

## 疑難排解

1. **Newman 找不到**: 確保已全域安裝 Newman
2. **連線錯誤**: 確保 ASP.NET Core 應用程式正在運行
3. **測試失敗**: 檢查 API 端點和資料格式是否正確
