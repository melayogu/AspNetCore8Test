using FluentValidation;
using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Validators;
using AspNetCore8Test.Validators.LoRaValidators;
using AspNetCore8Test.Services;
using AspNetCore8Test.Services.LoRaServices;
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
builder.Services.AddScoped<LibraryService>();

// 註冊地政士系統服務
builder.Services.AddScoped<AspNetCore8Test.Services.LandSurveyorServices.ICustomerService, 
    AspNetCore8Test.Services.LandSurveyorServices.MockCustomerService>();
builder.Services.AddScoped<AspNetCore8Test.Services.LandSurveyorServices.ICaseService, 
    AspNetCore8Test.Services.LandSurveyorServices.MockCaseService>();
builder.Services.AddScoped<AspNetCore8Test.Services.LandSurveyorServices.IAppointmentService, 
    AspNetCore8Test.Services.LandSurveyorServices.MockAppointmentService>();
builder.Services.AddScoped<AspNetCore8Test.Services.LandSurveyorServices.ICaseProgressService, 
    AspNetCore8Test.Services.LandSurveyorServices.MockCaseProgressService>();

// 註冊公園管理服務
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IEnvironmentalService, EnvironmentalService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IReportService, ReportService>();

// 註冊 FluentValidation 驗證器
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();

// 註冊天然氣公司相關服務
builder.Services.AddScoped<AspNetCore8Test.Services.GasServices.ICustomerService, AspNetCore8Test.Services.GasServices.CustomerService>();
builder.Services.AddScoped<AspNetCore8Test.Services.GasServices.IBillingService, AspNetCore8Test.Services.GasServices.BillingService>();
builder.Services.AddScoped<AspNetCore8Test.Services.GasServices.IPipelineService, AspNetCore8Test.Services.GasServices.PipelineService>();

// 註冊天然氣公司相關驗證器
builder.Services.AddScoped<IValidator<AspNetCore8Test.Models.DTOs.GasDTOs.CreateCustomerDto>, AspNetCore8Test.Validators.GasValidators.CreateCustomerDtoValidator>();
builder.Services.AddScoped<IValidator<AspNetCore8Test.Models.DTOs.GasDTOs.UpdateCustomerDto>, AspNetCore8Test.Validators.GasValidators.UpdateCustomerDtoValidator>();

// 註冊 LoRa 無線抄表系統服務
builder.Services.AddScoped<ILoRaDeviceService, LoRaDeviceService>();
builder.Services.AddScoped<ILoRaGatewayService, LoRaGatewayService>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddScoped<ILoRaAlertService, LoRaAlertService>();

// 註冊 LoRa 無線抄表系統驗證器
builder.Services.AddScoped<IValidator<CreateLoRaDeviceDto>, CreateLoRaDeviceDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateLoRaDeviceDto>, UpdateLoRaDeviceDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateLoRaDeviceStatusDto>, UpdateLoRaDeviceStatusDtoValidator>();
builder.Services.AddScoped<IValidator<CreateLoRaGatewayDto>, CreateLoRaGatewayDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateLoRaGatewayDto>, UpdateLoRaGatewayDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateLoRaGatewayStatusDto>, UpdateLoRaGatewayStatusDtoValidator>();
builder.Services.AddScoped<IValidator<CreateMeterReadingDto>, CreateMeterReadingDtoValidator>();
builder.Services.AddScoped<IValidator<CreateLoRaAlertDto>, CreateLoRaAlertDtoValidator>();
builder.Services.AddScoped<IValidator<AcknowledgeLoRaAlertDto>, AcknowledgeLoRaAlertDtoValidator>();

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
