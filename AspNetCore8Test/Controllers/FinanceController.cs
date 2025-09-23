using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models.FinanceModels;

namespace AspNetCore8Test.Controllers
{
    public class FinanceController : Controller
    {
        // 融資租賃管理主頁
        public IActionResult Index()
        {
            ViewData["Title"] = "融資租賃管理系統";
            return View();
        }

        // 車輛分期業務
        public IActionResult VehicleFinance()
        {
            ViewData["Title"] = "車輛分期業務";
            
            // 模擬測試資料
            var vehicleFinances = new List<VehicleFinance>
            {
                new VehicleFinance 
                { 
                    Id = 1, 
                    CustomerName = "王大明", 
                    VehicleType = "小客車", 
                    Brand = "Toyota", 
                    Model = "Camry", 
                    LoanAmount = 800000, 
                    MonthlyPayment = 25000, 
                    LoanPeriod = 36, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-30),
                    NextPaymentDate = DateTime.Now.AddDays(5)
                },
                new VehicleFinance 
                { 
                    Id = 2, 
                    CustomerName = "李小華", 
                    VehicleType = "重車", 
                    Brand = "Volvo", 
                    Model = "FH16", 
                    LoanAmount = 2500000, 
                    MonthlyPayment = 75000, 
                    LoanPeriod = 48, 
                    Status = "逾期", 
                    ApplicationDate = DateTime.Now.AddDays(-180),
                    NextPaymentDate = DateTime.Now.AddDays(-3)
                },
                new VehicleFinance 
                { 
                    Id = 3, 
                    CustomerName = "張美玲", 
                    VehicleType = "遊覽車", 
                    Brand = "Mercedes-Benz", 
                    Model = "Travego", 
                    LoanAmount = 3200000, 
                    MonthlyPayment = 85000, 
                    LoanPeriod = 60, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-90),
                    NextPaymentDate = DateTime.Now.AddDays(15)
                }
            };

            return View(vehicleFinances);
        }

        // 營建機具分期
        public IActionResult ConstructionEquipment()
        {
            ViewData["Title"] = "營建機具分期";
            
            var equipmentFinances = new List<ConstructionEquipmentFinance>
            {
                new ConstructionEquipmentFinance 
                { 
                    Id = 1, 
                    CustomerName = "建興營造有限公司", 
                    EquipmentType = "挖土機", 
                    Brand = "Caterpillar", 
                    Model = "320D", 
                    LoanAmount = 5500000, 
                    MonthlyPayment = 150000, 
                    LoanPeriod = 48, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-60),
                    NextPaymentDate = DateTime.Now.AddDays(10)
                },
                new ConstructionEquipmentFinance 
                { 
                    Id = 2, 
                    CustomerName = "大同建設股份有限公司", 
                    EquipmentType = "推土機", 
                    Brand = "Komatsu", 
                    Model = "D65EX", 
                    LoanAmount = 4200000, 
                    MonthlyPayment = 120000, 
                    LoanPeriod = 42, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-120),
                    NextPaymentDate = DateTime.Now.AddDays(20)
                }
            };

            return View(equipmentFinances);
        }

        // 產業機具分期
        public IActionResult IndustrialEquipment()
        {
            ViewData["Title"] = "產業機具分期";
            
            var industrialFinances = new List<IndustrialEquipmentFinance>
            {
                new IndustrialEquipmentFinance 
                { 
                    Id = 1, 
                    CustomerName = "台灣製造股份有限公司", 
                    EquipmentType = "CNC加工機", 
                    Brand = "MAZAK", 
                    Model = "INTEGREX i-400", 
                    LoanAmount = 8000000, 
                    MonthlyPayment = 200000, 
                    LoanPeriod = 60, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-45),
                    NextPaymentDate = DateTime.Now.AddDays(8)
                },
                new IndustrialEquipmentFinance 
                { 
                    Id = 2, 
                    CustomerName = "精密工業有限公司", 
                    EquipmentType = "射出成型機", 
                    Brand = "FANUC", 
                    Model = "ROBOSHOT α-S300iA", 
                    LoanAmount = 3500000, 
                    MonthlyPayment = 95000, 
                    LoanPeriod = 48, 
                    Status = "審核中", 
                    ApplicationDate = DateTime.Now.AddDays(-15),
                    NextPaymentDate = DateTime.Now.AddDays(30)
                }
            };

            return View(industrialFinances);
        }

        // 綠能融資
        public IActionResult GreenEnergy()
        {
            ViewData["Title"] = "綠能融資專案";
            
            var greenEnergyFinances = new List<GreenEnergyFinance>
            {
                new GreenEnergyFinance 
                { 
                    Id = 1, 
                    CustomerName = "永續能源股份有限公司", 
                    ProjectType = "太陽能發電系統", 
                    Capacity = "100KW", 
                    LoanAmount = 6000000, 
                    MonthlyPayment = 125000, 
                    LoanPeriod = 72, 
                    Status = "正常繳費", 
                    ApplicationDate = DateTime.Now.AddDays(-90),
                    NextPaymentDate = DateTime.Now.AddDays(12),
                    CarbonReduction = 85.6m
                },
                new GreenEnergyFinance 
                { 
                    Id = 2, 
                    CustomerName = "綠電科技有限公司", 
                    ProjectType = "風力發電設備", 
                    Capacity = "2MW", 
                    LoanAmount = 45000000, 
                    MonthlyPayment = 750000, 
                    LoanPeriod = 84, 
                    Status = "審核中", 
                    ApplicationDate = DateTime.Now.AddDays(-20),
                    NextPaymentDate = DateTime.Now.AddDays(25),
                    CarbonReduction = 1200.5m
                }
            };

            return View(greenEnergyFinances);
        }

        // 應收帳款受讓
        public IActionResult AccountsReceivable()
        {
            ViewData["Title"] = "應收帳款受讓業務";
            
            var arFinances = new List<AccountsReceivableFinance>
            {
                new AccountsReceivableFinance 
                { 
                    Id = 1, 
                    SellerCompany = "科技製造股份有限公司", 
                    BuyerCompany = "大型零售連鎖店", 
                    InvoiceAmount = 5000000, 
                    AdvanceRatio = 80, 
                    AdvanceAmount = 4000000, 
                    FeeRate = 2.5m, 
                    Status = "已放款", 
                    ApplicationDate = DateTime.Now.AddDays(-30),
                    DueDate = DateTime.Now.AddDays(60),
                    InvoiceNumber = "INV-2024-001"
                },
                new AccountsReceivableFinance 
                { 
                    Id = 2, 
                    SellerCompany = "傳統工業有限公司", 
                    BuyerCompany = "國際貿易商", 
                    InvoiceAmount = 3200000, 
                    AdvanceRatio = 75, 
                    AdvanceAmount = 2400000, 
                    FeeRate = 2.8m, 
                    Status = "審核中", 
                    ApplicationDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(85),
                    InvoiceNumber = "INV-2024-002"
                }
            };

            return View(arFinances);
        }

        // 不動產租賃
        public IActionResult RealEstate()
        {
            ViewData["Title"] = "不動產租賃業務";
            
            var realEstateLeases = new List<RealEstateLease>
            {
                new RealEstateLease 
                { 
                    Id = 1, 
                    LesseeName = "台北商業大樓股份有限公司", 
                    PropertyType = "商業大樓", 
                    Address = "台北市信義區信義路五段150號", 
                    FloorArea = 2500, 
                    MonthlyRent = 750000, 
                    LeasePeriod = 60, 
                    Status = "租賃中", 
                    LeaseStartDate = DateTime.Now.AddDays(-365),
                    LeaseEndDate = DateTime.Now.AddDays(1460),
                    Deposit = 2250000
                },
                new RealEstateLease 
                { 
                    Id = 2, 
                    LesseeName = "製造業廠房有限公司", 
                    PropertyType = "工業廠房", 
                    Address = "桃園市龜山區工業六路88號", 
                    FloorArea = 5000, 
                    MonthlyRent = 600000, 
                    LeasePeriod = 84, 
                    Status = "租賃中", 
                    LeaseStartDate = DateTime.Now.AddDays(-180),
                    LeaseEndDate = DateTime.Now.AddDays(2340),
                    Deposit = 1800000
                }
            };

            return View(realEstateLeases);
        }

        // 客戶管理
        public IActionResult Customers()
        {
            ViewData["Title"] = "客戶管理";
            
            var customers = new List<FinanceCustomer>
            {
                new FinanceCustomer 
                { 
                    Id = 1, 
                    Name = "王大明", 
                    CompanyName = "", 
                    IdNumber = "A123456789", 
                    Phone = "0912-345-678", 
                    Email = "wang@email.com", 
                    Address = "台北市中山區南京東路二段100號", 
                    CreditRating = "A", 
                    TotalLoanAmount = 800000, 
                    Status = "正常", 
                    CreateDate = DateTime.Now.AddDays(-30)
                },
                new FinanceCustomer 
                { 
                    Id = 2, 
                    Name = "", 
                    CompanyName = "建興營造有限公司", 
                    IdNumber = "12345678", 
                    Phone = "02-2345-6789", 
                    Email = "info@construction.com", 
                    Address = "新北市板橋區文化路一段200號", 
                    CreditRating = "A+", 
                    TotalLoanAmount = 5500000, 
                    Status = "正常", 
                    CreateDate = DateTime.Now.AddDays(-60)
                },
                new FinanceCustomer 
                { 
                    Id = 3, 
                    Name = "", 
                    CompanyName = "永續能源股份有限公司", 
                    IdNumber = "87654321", 
                    Phone = "03-456-7890", 
                    Email = "contact@greenenergy.com", 
                    Address = "桃園市中壢區中央路三段300號", 
                    CreditRating = "A", 
                    TotalLoanAmount = 6000000, 
                    Status = "正常", 
                    CreateDate = DateTime.Now.AddDays(-90)
                }
            };

            return View(customers);
        }

        // 風險控管
        public IActionResult RiskManagement()
        {
            ViewData["Title"] = "風險控管系統";
            return View();
        }

        // 財務報表
        public IActionResult FinancialReports()
        {
            ViewData["Title"] = "財務報表";
            return View();
        }
    }
}