# 地政士資訊系統

這是一個完整的地政士事務所管理系統，包含客戶管理、案件管理、不動產登記、預約服務等功能模組。

## 主要功能模組

### 1. 客戶管理 (CRM)
- 客戶基本資料維護
- 身分證字號驗證
- 客戶案件關聯
- 客戶搜尋與篩選

### 2. 案件管理
- 案件收件與編號自動產生
- 案件狀態追蹤
- 案件類型分類管理
- 案件進度記錄
- 預計完成日期管理

### 3. 不動產登記管理
- 土地登記資料管理
- 建物登記資料管理
- 地籍資料查詢
- 登記異動追蹤

### 4. 預約服務系統
- 線上預約排程
- 預約衝突檢查
- 預約狀態管理
- 行事曆檢視
- 預約提醒功能

### 5. 財務管理
- 收費記錄管理
- 發票開立追蹤
- 收款狀態管理
- 財務報表分析

### 6. 系統儀表板
- 業務統計總覽
- 案件狀態分析
- 今日預約顯示
- 最近案件列表
- 收入統計圖表

## 資料庫設定

### 1. 更新連接字串
在 `appsettings.json` 中已預設使用 LocalDB：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LandSurveyorSystem;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 2. 建立資料庫遷移
開啟 Package Manager Console 或終端機，執行以下命令：

```bash
# 新增 Entity Framework Core 工具 (如果尚未安裝)
dotnet tool install --global dotnet-ef

# 新增遷移
dotnet ef migrations add InitialCreate --context LandSurveyorDbContext

# 更新資料庫
dotnet ef database update --context LandSurveyorDbContext
```

### 3. 資料表說明

- **Customers**: 客戶基本資料
- **Cases**: 案件管理
- **CaseDocuments**: 案件文件管理
- **CaseProgresses**: 案件進度追蹤
- **RealEstateRegistrations**: 不動產登記資料
- **Appointments**: 預約服務記錄
- **FinancialRecords**: 財務交易記錄

## 系統架構

### Controllers
- `CustomersController`: 客戶管理
- `CasesController`: 案件管理
- `RealEstateRegistrationsController`: 不動產登記
- `AppointmentsController`: 預約管理
- `LandSurveyorDashboardController`: 系統儀表板

### Models
所有模型都位於 `Models/LandSurveyorModels` 目錄下，包含完整的驗證特性和關聯設定。

### Views
使用 Bootstrap 5 和 Font Awesome 圖示，提供響應式設計和友善的使用者介面。

## 功能特色

1. **自動案件編號**: 依日期自動產生唯一案件編號
2. **身分證字號驗證**: 客戶端即時驗證身分證字號格式
3. **預約衝突檢查**: 自動檢查預約時間衝突
4. **狀態追蹤**: 完整的案件和預約狀態管理
5. **響應式設計**: 支援各種裝置尺寸
6. **搜尋與篩選**: 強大的資料搜尋和篩選功能
7. **圖表統計**: 使用 Chart.js 提供視覺化統計圖表

## 安全性考量

- 使用參數化查詢防止 SQL 注入
- 模型驗證防止無效資料
- CSRF 防護
- 輸入清理和驗證

## 使用說明

1. 啟動應用程式
2. 從導航選單的「地政士系統」進入
3. 建議先從「客戶管理」開始建立客戶資料
4. 再建立相關案件和預約
5. 使用儀表板監控整體業務狀況

## 開發環境

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server LocalDB
- Bootstrap 5
- Font Awesome 6
- Chart.js

此系統提供完整的地政士事務所管理解決方案，可根據實際需求進行客製化調整。