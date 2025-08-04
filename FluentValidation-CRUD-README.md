# FluentValidation CRUD 範例

這個專案展示了如何在 ASP.NET Core 8 中使用 FluentValidation 來實現完整的 CRUD 操作。

## 📋 專案結構

### 主要功能模組
- **Models**: 商品模型和 DTO
- **Validators**: FluentValidation 驗證器
- **Services**: 商品服務層（記憶體儲存）
- **Controllers**: Web API 控制器
- **Tests**: 完整的單元測試

### 架構特點
- 使用 Repository 模式和依賴注入
- 分離 DTO 和實體模型
- 詳細的驗證規則和錯誤訊息
- 軟刪除機制
- 完整的測試覆蓋

## 🛠️ 安裝的套件

### 主專案 (AspNetCore8Test)
```xml
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.1" />
```

### 測試專案 (AspNetCore8Test.Tests)
```xml
<PackageReference Include="FluentValidation" Version="12.0.0" />
```

## 🏗️ API 端點

### 商品管理 API (`/api/products`)

| HTTP 方法 | 端點 | 描述 |
|-----------|------|------|
| GET | `/api/products` | 取得所有商品 |
| GET | `/api/products/{id}` | 取得特定商品 |
| POST | `/api/products` | 建立新商品 |
| PUT | `/api/products/{id}` | 更新商品 |
| DELETE | `/api/products/{id}` | 刪除商品（軟刪除） |

## 📝 驗證規則

### CreateProductDto 驗證規則
- **商品名稱 (Name)**:
  - 不能為空
  - 長度必須在 2-100 個字元之間
  - 只能包含字母、數字、中文和空格
- **商品描述 (Description)**:
  - 不能為空
  - 最大長度 500 個字元
- **商品價格 (Price)**:
  - 必須大於 0
  - 不能超過 1,000,000
- **商品分類 (Category)**:
  - 不能為空
  - 必須是以下之一：電子產品、服飾、書籍、運動、家居

### UpdateProductDto 驗證規則
- 包含 CreateProductDto 的所有規則
- **商品 ID (Id)**: 必須大於 0
- **是否啟用 (IsActive)**: 布林值

## 🧪 測試範例

### 1. 驗證器測試 (Fact + Theory)

```csharp
[Fact]
public void Should_Have_Error_When_Name_Is_Empty()
{
    var model = new CreateProductDto 
    { 
        Name = "",
        Description = "Valid description",
        Price = 100,
        Category = "電子產品"
    };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Name)
          .WithErrorMessage("商品名稱不能為空");
}

[Theory]
[InlineData("電子產品")]
[InlineData("服飾")]
[InlineData("書籍")]
[InlineData("運動")]
[InlineData("家居")]
public void Should_Not_Have_Error_When_Category_Is_Valid(string category)
{
    var model = new CreateProductDto 
    { 
        Name = "Valid Product",
        Description = "Valid description",
        Price = 100,
        Category = category
    };
    var result = _validator.TestValidate(model);
    result.ShouldNotHaveValidationErrorFor(x => x.Category);
}
```

### 2. 服務層測試

```csharp
[Fact]
public async Task CreateProductAsync_ShouldCreateProductWithCorrectProperties()
{
    // Arrange
    var service = new ProductService();
    var newProduct = new Product
    {
        Name = "Test Product",
        Description = "Test Description",
        Price = 199.99m,
        Category = "電子產品"
    };

    // Act
    var createdProduct = await service.CreateProductAsync(newProduct);

    // Assert
    Assert.NotNull(createdProduct);
    Assert.True(createdProduct.Id > 0);
    Assert.Equal("Test Product", createdProduct.Name);
    Assert.True(createdProduct.IsActive);
}
```

### 3. 控制器測試 (使用 Mock)

```csharp
[Fact]
public async Task CreateProduct_WithValidData_ShouldReturnCreatedAtAction()
{
    // Arrange
    var createDto = new CreateProductDto
    {
        Name = "New Product",
        Description = "New Description",
        Price = 299.99m,
        Category = "電子產品"
    };

    _mockProductService.Setup(s => s.CreateProductAsync(It.IsAny<Product>()))
                       .ReturnsAsync(createdProduct);

    // Act
    var result = await _controller.CreateProduct(createDto);

    // Assert
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
    var returnedProduct = Assert.IsType<ProductResponseDto>(createdAtActionResult.Value);
    Assert.Equal("New Product", returnedProduct.Name);
}
```

## 🚀 如何執行

### 1. 執行測試
```bash
dotnet test
```

### 2. 啟動應用程式
```bash
dotnet run --project AspNetCore8Test
```

### 3. API 測試範例

#### 建立商品
```bash
curl -X POST https://localhost:7001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "MacBook Pro",
    "description": "高效能筆記型電腦",
    "price": 55900,
    "category": "電子產品"
  }'
```

#### 取得所有商品
```bash
curl https://localhost:7001/api/products
```

## 📊 測試統計

- **總測試數**: 93 個
- **驗證器測試**: 24 個
- **服務層測試**: 12 個
- **控制器測試**: 17 個
- **HomeController 測試**: 5 個 (原有)
- **基本測試**: 10 個 (原有)

## 🎯 主要特色

1. **完整的驗證覆蓋**: 使用 FluentValidation 提供詳細的驗證規則
2. **多種測試方式**: 包含 [Fact] 和 [Theory] + [InlineData] 兩種測試風格
3. **依賴注入**: 正確使用 DI 容器管理服務生命週期
4. **錯誤處理**: 完整的錯誤訊息和狀態碼回傳
5. **軟刪除**: 保護資料不被意外刪除
6. **DTO 模式**: 分離輸入輸出與內部模型

這個範例展示了在企業級應用中實作 FluentValidation 的最佳實務，包含完整的測試策略和清晰的架構設計。
