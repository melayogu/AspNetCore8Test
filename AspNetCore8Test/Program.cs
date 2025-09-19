using FluentValidation;
using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Validators;
using AspNetCore8Test.Services;
using AspNetCore8Test.Services.ParkServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 註冊 API 控制器
builder.Services.AddControllers();

// 配置 Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊服務
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ApprovalService>();

// 註冊公園管理服務
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IEnvironmentalService, EnvironmentalService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IEventService, EventService>();

// 註冊 FluentValidation 驗證器
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // 在開發環境中啟用 Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 新增 API 路由
app.MapControllers();

await app.RunAsync();
