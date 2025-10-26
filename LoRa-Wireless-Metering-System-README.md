# 微電腦無線抄表 LoRa 系統

此模組為 ASP.NET Core 8 專案中的全新子系統，模擬公用事業以 LoRa 技術建置的遠端自動抄表平台。系統採用分層架構與 FluentValidation，提供裝置管理、閘道器監控、抄表資料蒐集與警報處理等核心能力。

## 系統特色

- ✅ **LoRa 裝置生命週期管理**：支援新增、更新、刪除與查詢（ID/DeviceEUI/表號）。
- ✅ **閘道器監控**：查詢閘道器 GPS 位置、韌體版本、上線狀態與連線裝置數。
- ✅ **抄表資料蒐集**：支援時間區間查詢與新增即時讀值，並自動更新裝置狀態。
- ✅ **警報流程**：提供裝置警報歷史、待處理警報，以及確認/結案操作。
- ✅ **健康度摘要**：輸出網路整體概況（成功率、平均電量、在線閘道數等）。

## 端點總覽

| 方法 | 路徑 | 說明 |
| --- | --- | --- |
| GET | `/api/lora-metering/devices` | 取得所有 LoRa 裝置 |
| GET | `/api/lora-metering/devices/{id}` | 透過 ID 查詢裝置 |
| GET | `/api/lora-metering/devices/by-eui/{deviceEui}` | 透過 DeviceEUI 查詢 |
| GET | `/api/lora-metering/devices/by-meter/{meterNumber}` | 透過表號查詢 |
| POST | `/api/lora-metering/devices` | 新增裝置（具資料驗證） |
| PUT | `/api/lora-metering/devices/{id}` | 更新裝置與即時狀態 |
| DELETE | `/api/lora-metering/devices/{id}` | 刪除裝置與其讀值、警報 |
| GET | `/api/lora-metering/devices/{id}/readings` | 裝置抄表歷史，支援時間範圍 |
| POST | `/api/lora-metering/devices/{id}/readings` | 新增抄表資料、更新狀態 |
| GET | `/api/lora-metering/devices/{id}/alerts` | 裝置警報歷史 |
| GET | `/api/lora-metering/gateways` | 取得所有閘道器資訊 |
| GET | `/api/lora-metering/alerts` | 取得未結案警報 |
| POST | `/api/lora-metering/alerts/{id}/acknowledge` | 警報確認 |
| POST | `/api/lora-metering/alerts/{id}/resolve` | 警報結案 |
| GET | `/api/lora-metering/summary` | 取得網路健康度摘要 |

## 主要類別與檔案

```
Models/
└── LoRaModels/
    ├── LoRaDevice.cs           # LoRa 端點裝置資料模型
    ├── LoRaGateway.cs          # 閘道器資訊模型
    ├── MeterReading.cs         # 抄表讀值紀錄
    └── LoRaAlert.cs            # 警報紀錄

Models/DTOs/LoRaDtos/
├── LoRaDeviceDtos.cs          # 裝置 DTO 與新增/更新輸入模型
├── LoRaGatewayDto.cs          # 閘道器輸出 DTO
├── MeterReadingDtos.cs        # 抄表資料 DTO
├── LoRaAlertDtos.cs           # 警報 DTO 與動作輸入
└── LoRaNetworkSummaryDto.cs   # 網路摘要 DTO

Services/LoRaServices/
└── LoRaMeteringService.cs     # 業務邏輯與模擬資料儲存

Controllers/LoRaControllers/
└── LoRaMeteringController.cs  # RESTful API 入口

Validators/LoRaValidators/
├── LoRaDeviceValidators.cs    # 裝置新增/更新驗證
├── MeterReadingValidators.cs  # 抄表資料驗證
└── AlertActionDtoValidator.cs # 警報操作驗證
```

## 驗證規則重點

- DeviceEUI 必須為 16 位元十六進位字串。
- 裝機時間、最後通訊時間與讀值時間不可晚於目前時間。
- 電池電量需落在 0–100%，訊號強度限制於 -150~-20 dBm。
- 操作人員姓名需提供並限制長度。

## 延伸應用

此模組可快速擴充至實際環境：

1. **資料庫整合**：將目前的 in-memory 集合替換為 EF Core repository。
2. **排程抄表任務**：可加入背景服務定期呼叫外部 LoRaWAN Network Server API。
3. **儀表板視覺化**：結合 Chart.js 或第三方監控平台呈現資料趨勢。
4. **多租戶管理**：依縣市或業務單位區分閘道器與裝置資源。

透過這套 API，可加速建置微電腦無線抄表系統的原型與測試場景。
