# xUnit 測試方法總覽

本專案展示了 xUnit 框架中五種主要的測試寫法，每種方法都有其適用的情境和特性。

## 📁 測試檔案結構

```
AspNetCore8Test.Tests/Examples/
├── FactExampleTests.cs              # [Fact] 測試範例
├── TheoryInlineDataTests.cs         # [Theory] + InlineData 測試範例  
├── TheoryMemberDataTests.cs         # [Theory] + MemberData 測試範例
├── TheoryClassDataTests.cs          # [Theory] + ClassData 測試範例
└── IClassFixtureExampleTests.cs     # IClassFixture<T> 測試範例
```

## 🧪 五種測試方法詳解

### 1. [Fact] - 單一測試方法
- **關鍵屬性**：單一測試方法
- **適合情境**：固定測資、不需參數
- **特性說明**：最基本、最常見的寫法
- **範例檔案**：`FactExampleTests.cs`

```csharp
[Fact]
public async Task GetAllProductsAsync_ShouldReturnAllActiveProducts()
{
    // Arrange, Act, Assert
}
```

### 2. [Theory] + InlineData - 多組參數
- **關鍵屬性**：多組參數
- **適合情境**：簡單多組測資（值類型為主）
- **特性說明**：直接在屬性列舉每組參數
- **範例檔案**：`TheoryInlineDataTests.cs`

```csharp
[Theory]
[InlineData("iPhone", "手機", 1000, true)]
[InlineData("MacBook", "筆電", 50000, true)]
[InlineData("", "無名稱產品", 100, false)]
public void Product_Validation_ShouldMatchExpectedResult(
    string name, string description, decimal price, bool expectedValid)
{
    // 測試邏輯
}
```

### 3. [Theory] + MemberData - 複雜資料結構
- **關鍵屬性**：複雜資料結構
- **適合情境**：多組帶物件或時間的測資
- **特性說明**：資料來自 static 屬性或方法
- **範例檔案**：`TheoryMemberDataTests.cs`

```csharp
public static IEnumerable<object[]> ProductTestData =>
    new List<object[]>
    {
        new object[]
        {
            new Product { Name = "iPhone 15", Price = 32900 },
            "電子產品",
            true
        }
    };

[Theory]
[MemberData(nameof(ProductTestData))]
public async Task CreateProductAsync_WithVariousData_ShouldHandleCorrectly(
    Product product, string expectedCategory, bool shouldSucceed)
{
    // 測試邏輯
}
```

### 4. [Theory] + ClassData - 複雜邏輯生成的測資
- **關鍵屬性**：複雜邏輯生成的測資
- **適合情境**：測資來源需要封裝邏輯時
- **特性說明**：建立類別繼承 `IEnumerable<object[]>`
- **範例檔案**：`TheoryClassDataTests.cs`

```csharp
public class ProductValidationTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new Product { Name = "有效產品", Price = 100 },
            "create",
            true,
            "有效的產品應該能夠創建"
        };
    }
}

[Theory]
[ClassData(typeof(ProductValidationTestData))]
public async Task ProductService_Validation_ShouldMatchExpectedBehavior(
    Product product, string operationType, bool expectedSuccess, string expectedMessage)
{
    // 測試邏輯
}
```

### 5. IClassFixture<T> - 測試初始化
- **關鍵屬性**：測試初始化（非測資）
- **適合情境**：共用資料庫連線、初始化邏輯
- **特性說明**：測試前置動作（非測資），搭配 constructor
- **範例檔案**：`IClassFixtureExampleTests.cs`

```csharp
public class ProductServiceFixture : IDisposable
{
    public IProductService ProductService { get; private set; }
    public List<Product> InitialProducts { get; private set; }

    public ProductServiceFixture()
    {
        // 初始化共用資源
    }
}

public class IClassFixtureExampleTests : IClassFixture<ProductServiceFixture>
{
    private readonly ProductServiceFixture _fixture;

    public IClassFixtureExampleTests(ProductServiceFixture fixture)
    {
        _fixture = fixture;
    }
}
```

## 🚀 執行測試

### 執行所有測試
```bash
dotnet test
```

### 執行特定測試檔案
```bash
dotnet test --filter "ClassName=FactExampleTests"
dotnet test --filter "ClassName=TheoryInlineDataTests"
dotnet test --filter "ClassName=TheoryMemberDataTests"
dotnet test --filter "ClassName=TheoryClassDataTests"
dotnet test --filter "ClassName=IClassFixtureExampleTests"
```

### 執行特定測試方法
```bash
dotnet test --filter "MethodName=GetAllProductsAsync_ShouldReturnAllActiveProducts"
```

### 顯示詳細輸出
```bash
dotnet test --verbosity normal
```

## 📊 測試方法選擇指南

| 情境 | 建議方法 | 原因 |
|------|----------|------|
| 簡單的單一功能測試 | `[Fact]` | 最直接明瞭 |
| 需要測試多組簡單參數 | `[Theory] + InlineData` | 參數直接可見 |
| 需要測試複雜物件或多個相關參數 | `[Theory] + MemberData` | 可以建立複雜的測試資料 |
| 測試資料需要動態生成或有複雜邏輯 | `[Theory] + ClassData` | 封裝測試資料邏輯 |
| 需要共用初始化邏輯或資源 | `IClassFixture<T>` | 減少重複的初始化程式碼 |

## 🔍 範例中測試的功能

每個測試檔案都測試了 `ProductService` 的不同面向：

- **產品基本 CRUD 操作**：建立、讀取、更新、刪除
- **產品驗證邏輯**：名稱、價格、分類的有效性
- **服務行為測試**：錯誤處理、邊界條件
- **整合測試**：多個操作的組合

## 💡 最佳實踐

1. **選擇適合的測試方法**：根據測試需求選擇最合適的方法
2. **保持測試獨立**：每個測試都應該能夠獨立執行
3. **使用描述性的測試名稱**：清楚表達測試的目的
4. **遵循 AAA 模式**：Arrange（準備）、Act（執行）、Assert（驗證）
5. **適當使用夾具**：只在真正需要共用資源時使用 `IClassFixture<T>`

這個專案展示了如何在實際的 ASP.NET Core 應用程式中應用這五種 xUnit 測試方法，每種方法都有其獨特的優勢和適用場景。
