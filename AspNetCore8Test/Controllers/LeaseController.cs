using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models.FinanceModels;

namespace AspNetCore8Test.Controllers
{
    public class LeaseController : Controller
    {
        // 租賃管理主頁
        public IActionResult Index()
        {
            ViewData["Title"] = "租賃管理系統";
            return View();
        }

        // 進口機具代開狀業務
        public IActionResult ImportLetterOfCredit()
        {
            ViewData["Title"] = "進口機具代開狀業務";
            
            var lcApplications = new List<LetterOfCreditApplication>
            {
                new LetterOfCreditApplication 
                { 
                    Id = 1, 
                    ApplicantName = "精密機械進出口股份有限公司", 
                    BeneficiaryName = "German Machinery GmbH", 
                    LcAmount = 2500000, 
                    Currency = "USD", 
                    ExchangeRate = 31.5m, 
                    EquipmentDescription = "高精度CNC銑床", 
                    Status = "已開狀", 
                    ApplicationDate = DateTime.Now.AddDays(-30),
                    ExpiryDate = DateTime.Now.AddDays(120),
                    LcNumber = "LC2024001"
                },
                new LetterOfCreditApplication 
                { 
                    Id = 2, 
                    ApplicantName = "台灣重工業有限公司", 
                    BeneficiaryName = "Italian Heavy Equipment SRL", 
                    LcAmount = 4800000, 
                    Currency = "EUR", 
                    ExchangeRate = 34.2m, 
                    EquipmentDescription = "大型鍛造設備", 
                    Status = "審核中", 
                    ApplicationDate = DateTime.Now.AddDays(-10),
                    ExpiryDate = DateTime.Now.AddDays(150),
                    LcNumber = "LC2024002"
                }
            };

            return View(lcApplications);
        }

        // 大型重機分期與租賃
        public IActionResult MotorcycleLease()
        {
            ViewData["Title"] = "大型重機分期與租賃";
            
            var motorcycleLeases = new List<MotorcycleLease>
            {
                new MotorcycleLease 
                { 
                    Id = 1, 
                    CustomerName = "陳志明", 
                    Brand = "Harley-Davidson", 
                    Model = "Street Glide", 
                    EngineCapacity = 1745, 
                    LeaseType = "分期付款", 
                    TotalAmount = 1200000, 
                    MonthlyPayment = 35000, 
                    LeasePeriod = 36, 
                    Status = "租賃中", 
                    StartDate = DateTime.Now.AddDays(-90),
                    EndDate = DateTime.Now.AddDays(990),
                    Mileage = 15000
                },
                new MotorcycleLease 
                { 
                    Id = 2, 
                    CustomerName = "林美惠", 
                    Brand = "BMW", 
                    Model = "R1250GS", 
                    EngineCapacity = 1254, 
                    LeaseType = "營業租賃", 
                    TotalAmount = 980000, 
                    MonthlyPayment = 28000, 
                    LeasePeriod = 24, 
                    Status = "申請中", 
                    StartDate = DateTime.Now.AddDays(15),
                    EndDate = DateTime.Now.AddDays(750),
                    Mileage = 0
                }
            };

            return View(motorcycleLeases);
        }

        // 租賃合約管理
        public IActionResult Contracts()
        {
            ViewData["Title"] = "租賃合約管理";
            
            var contracts = new List<LeaseContract>
            {
                new LeaseContract 
                { 
                    Id = 1, 
                    ContractNumber = "LEASE-2024-001", 
                    LesseeName = "王大明", 
                    AssetType = "小客車", 
                    AssetDescription = "Toyota Camry 2024", 
                    LeaseAmount = 800000, 
                    MonthlyPayment = 25000, 
                    LeasePeriod = 36, 
                    Status = "執行中", 
                    SignDate = DateTime.Now.AddDays(-30),
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now.AddDays(1050)
                },
                new LeaseContract 
                { 
                    Id = 2, 
                    ContractNumber = "LEASE-2024-002", 
                    LesseeName = "建興營造有限公司", 
                    AssetType = "營建機具", 
                    AssetDescription = "Caterpillar 320D 挖土機", 
                    LeaseAmount = 5500000, 
                    MonthlyPayment = 150000, 
                    LeasePeriod = 48, 
                    Status = "執行中", 
                    SignDate = DateTime.Now.AddDays(-60),
                    StartDate = DateTime.Now.AddDays(-60),
                    EndDate = DateTime.Now.AddDays(1380)
                }
            };

            return View(contracts);
        }

        // 資產管理
        public IActionResult Assets()
        {
            ViewData["Title"] = "資產管理";
            
            var assets = new List<LeaseAsset>
            {
                new LeaseAsset 
                { 
                    Id = 1, 
                    AssetNumber = "ASSET-001", 
                    AssetType = "車輛", 
                    Brand = "Toyota", 
                    Model = "Camry", 
                    PurchaseDate = DateTime.Now.AddDays(-365),
                    PurchasePrice = 900000, 
                    CurrentValue = 720000, 
                    Status = "租賃中", 
                    Location = "台北市", 
                    Condition = "良好",
                    DepreciationRate = 20.0m
                },
                new LeaseAsset 
                { 
                    Id = 2, 
                    AssetNumber = "ASSET-002", 
                    AssetType = "營建機具", 
                    Brand = "Caterpillar", 
                    Model = "320D", 
                    PurchaseDate = DateTime.Now.AddDays(-180),
                    PurchasePrice = 6000000, 
                    CurrentValue = 5400000, 
                    Status = "租賃中", 
                    Location = "新北市", 
                    Condition = "良好",
                    DepreciationRate = 10.0m
                }
            };

            return View(assets);
        }

        // 保險管理
        public IActionResult Insurance()
        {
            ViewData["Title"] = "保險管理";
            return View();
        }

        // 維護管理
        public IActionResult Maintenance()
        {
            ViewData["Title"] = "維護管理";
            return View();
        }
    }
}