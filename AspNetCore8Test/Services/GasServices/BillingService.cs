using AspNetCore8Test.Models.GasModels;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Services.GasServices
{
    public interface IBillingService
    {
        Task<IEnumerable<BillDto>> GetAllBillsAsync();
        Task<BillDto?> GetBillByIdAsync(int id);
        Task<IEnumerable<BillDto>> GetBillsByCustomerIdAsync(int customerId);
        Task<BillDto> CreateBillAsync(CreateBillDto createBillDto);
        Task<BillDto?> ProcessPaymentAsync(PaymentDto paymentDto);
        Task<IEnumerable<BillDto>> GetUnpaidBillsAsync();
        Task<IEnumerable<BillDto>> GetOverdueBillsAsync();
        Task<decimal> CalculateUsageCharge(decimal usage, decimal unitPrice);
        Task<decimal> CalculateTaxAmount(decimal amount, decimal taxRate = 0.05m);
    }

    public class BillingService : IBillingService
    {
        private readonly ICustomerService _customerService;
        
        // 模擬資料庫
        private static List<Bill> _bills = new List<Bill>();
        private static List<Payment> _payments = new List<Payment>();
        private static List<GasMeter> _gasMeters = new List<GasMeter>
        {
            new GasMeter { Id = 1, MeterNumber = "M001", CustomerId = 1, InstallationAddress = "台北市信義區信義路一段100號", InstallationDate = DateTime.Now.AddYears(-2), LastReading = 1250.5m, MeterType = "智慧錶", MeterStatus = "Active" },
            new GasMeter { Id = 2, MeterNumber = "M002", CustomerId = 2, InstallationAddress = "台北市大安區復興南路二段200號", InstallationDate = DateTime.Now.AddYears(-1), LastReading = 2100.8m, MeterType = "智慧錶", MeterStatus = "Active" },
            new GasMeter { Id = 3, MeterNumber = "M003", CustomerId = 3, InstallationAddress = "新北市土城區工業路300號", InstallationDate = DateTime.Now.AddMonths(-6), LastReading = 5500.2m, MeterType = "智慧錶", MeterStatus = "Active" }
        };
        private static int _nextBillId = 1;
        private static int _nextPaymentId = 1;

        public BillingService(ICustomerService customerService)
        {
            _customerService = customerService;
            
            // 初始化範例帳單資料
            if (!_bills.Any())
            {
                InitializeSampleBills();
            }
        }

        public async Task<IEnumerable<BillDto>> GetAllBillsAsync()
        {
            await Task.Delay(1);
            var billDtos = new List<BillDto>();
            
            foreach (var bill in _bills)
            {
                var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
                var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
                
                billDtos.Add(MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? ""));
            }
            
            return billDtos.OrderByDescending(b => b.BillDate);
        }

        public async Task<BillDto?> GetBillByIdAsync(int id)
        {
            await Task.Delay(1);
            var bill = _bills.FirstOrDefault(b => b.Id == id);
            if (bill == null) return null;
            
            var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
            var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
            
            return MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? "");
        }

        public async Task<IEnumerable<BillDto>> GetBillsByCustomerIdAsync(int customerId)
        {
            await Task.Delay(1);
            var customerBills = _bills.Where(b => b.CustomerId == customerId);
            var billDtos = new List<BillDto>();
            
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            
            foreach (var bill in customerBills)
            {
                var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
                billDtos.Add(MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? ""));
            }
            
            return billDtos.OrderByDescending(b => b.BillDate);
        }

        public async Task<BillDto> CreateBillAsync(CreateBillDto createBillDto)
        {
            await Task.Delay(1);
            
            var usage = createBillDto.CurrentReading - createBillDto.PreviousReading;
            var usageCharge = await CalculateUsageCharge(usage, createBillDto.UnitPrice);
            var totalBeforeTax = createBillDto.BasicCharge + usageCharge;
            var taxAmount = await CalculateTaxAmount(totalBeforeTax);
            var totalAmount = totalBeforeTax + taxAmount;
            
            var bill = new Bill
            {
                Id = _nextBillId++,
                BillNumber = createBillDto.BillNumber,
                CustomerId = createBillDto.CustomerId,
                GasMeterId = createBillDto.GasMeterId,
                BillDate = createBillDto.BillDate,
                DueDate = createBillDto.DueDate,
                BillingPeriodStart = createBillDto.BillingPeriodStart,
                BillingPeriodEnd = createBillDto.BillingPeriodEnd,
                PreviousReading = createBillDto.PreviousReading,
                CurrentReading = createBillDto.CurrentReading,
                Usage = usage,
                UnitPrice = createBillDto.UnitPrice,
                BasicCharge = createBillDto.BasicCharge,
                UsageCharge = usageCharge,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                PaidAmount = 0,
                BalanceAmount = totalAmount,
                Status = "Pending"
            };

            _bills.Add(bill);
            
            var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
            var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
            
            return MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? "");
        }

        public async Task<BillDto?> ProcessPaymentAsync(PaymentDto paymentDto)
        {
            await Task.Delay(1);
            
            var bill = _bills.FirstOrDefault(b => b.Id == paymentDto.BillId);
            if (bill == null) return null;
            
            var payment = new Payment
            {
                Id = _nextPaymentId++,
                BillId = paymentDto.BillId,
                PaymentNumber = $"PAY{_nextPaymentId:D6}",
                PaymentDate = DateTime.Now,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                ReferenceNumber = paymentDto.ReferenceNumber,
                Status = "Completed",
                Notes = paymentDto.Notes
            };
            
            _payments.Add(payment);
            
            // 更新帳單狀態
            bill.PaidAmount += paymentDto.Amount;
            bill.BalanceAmount = bill.TotalAmount - bill.PaidAmount;
            
            if (bill.BalanceAmount <= 0)
            {
                bill.Status = "Paid";
                bill.PaymentDate = DateTime.Now;
                bill.PaymentMethod = paymentDto.PaymentMethod;
            }
            else
            {
                bill.Status = "Partial";
            }
            
            var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
            var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
            
            return MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? "");
        }

        public async Task<IEnumerable<BillDto>> GetUnpaidBillsAsync()
        {
            await Task.Delay(1);
            var unpaidBills = _bills.Where(b => b.Status == "Pending" || b.Status == "Partial");
            var billDtos = new List<BillDto>();
            
            foreach (var bill in unpaidBills)
            {
                var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
                var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
                billDtos.Add(MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? ""));
            }
            
            return billDtos.OrderBy(b => b.DueDate);
        }

        public async Task<IEnumerable<BillDto>> GetOverdueBillsAsync()
        {
            await Task.Delay(1);
            var overdueBills = _bills.Where(b => (b.Status == "Pending" || b.Status == "Partial") && b.DueDate < DateTime.Today);
            var billDtos = new List<BillDto>();
            
            foreach (var bill in overdueBills)
            {
                var customer = await _customerService.GetCustomerByIdAsync(bill.CustomerId);
                var gasMeter = _gasMeters.FirstOrDefault(m => m.Id == bill.GasMeterId);
                billDtos.Add(MapToBillDto(bill, customer?.Name ?? "", gasMeter?.MeterNumber ?? ""));
            }
            
            return billDtos.OrderBy(b => b.DueDate);
        }

        public async Task<decimal> CalculateUsageCharge(decimal usage, decimal unitPrice)
        {
            await Task.Delay(1);
            return usage * unitPrice;
        }

        public async Task<decimal> CalculateTaxAmount(decimal amount, decimal taxRate = 0.05m)
        {
            await Task.Delay(1);
            return Math.Round(amount * taxRate, 2);
        }

        private void InitializeSampleBills()
        {
            var sampleBills = new List<Bill>
            {
                new Bill
                {
                    Id = _nextBillId++,
                    BillNumber = "B202501001",
                    CustomerId = 1,
                    GasMeterId = 1,
                    BillDate = DateTime.Now.AddDays(-30),
                    DueDate = DateTime.Now.AddDays(-10),
                    BillingPeriodStart = DateTime.Now.AddDays(-60),
                    BillingPeriodEnd = DateTime.Now.AddDays(-30),
                    PreviousReading = 1200.0m,
                    CurrentReading = 1250.5m,
                    Usage = 50.5m,
                    UnitPrice = 12.5m,
                    BasicCharge = 100m,
                    UsageCharge = 631.25m,
                    TaxAmount = 36.56m,
                    TotalAmount = 767.81m,
                    PaidAmount = 767.81m,
                    BalanceAmount = 0m,
                    Status = "Paid",
                    PaymentDate = DateTime.Now.AddDays(-5),
                    PaymentMethod = "Transfer"
                },
                new Bill
                {
                    Id = _nextBillId++,
                    BillNumber = "B202501002",
                    CustomerId = 2,
                    GasMeterId = 2,
                    BillDate = DateTime.Now.AddDays(-15),
                    DueDate = DateTime.Now.AddDays(15),
                    BillingPeriodStart = DateTime.Now.AddDays(-45),
                    BillingPeriodEnd = DateTime.Now.AddDays(-15),
                    PreviousReading = 2050.0m,
                    CurrentReading = 2100.8m,
                    Usage = 50.8m,
                    UnitPrice = 12.5m,
                    BasicCharge = 150m,
                    UsageCharge = 635m,
                    TaxAmount = 39.25m,
                    TotalAmount = 824.25m,
                    PaidAmount = 0m,
                    BalanceAmount = 824.25m,
                    Status = "Pending"
                }
            };
            
            _bills.AddRange(sampleBills);
        }

        private static BillDto MapToBillDto(Bill bill, string customerName, string meterNumber)
        {
            return new BillDto
            {
                Id = bill.Id,
                BillNumber = bill.BillNumber,
                CustomerId = bill.CustomerId,
                CustomerName = customerName,
                GasMeterId = bill.GasMeterId,
                MeterNumber = meterNumber,
                BillDate = bill.BillDate,
                DueDate = bill.DueDate,
                BillingPeriodStart = bill.BillingPeriodStart,
                BillingPeriodEnd = bill.BillingPeriodEnd,
                PreviousReading = bill.PreviousReading,
                CurrentReading = bill.CurrentReading,
                Usage = bill.Usage,
                UnitPrice = bill.UnitPrice,
                BasicCharge = bill.BasicCharge,
                UsageCharge = bill.UsageCharge,
                TaxAmount = bill.TaxAmount,
                TotalAmount = bill.TotalAmount,
                PaidAmount = bill.PaidAmount,
                BalanceAmount = bill.BalanceAmount,
                Status = bill.Status,
                PaymentDate = bill.PaymentDate,
                PaymentMethod = bill.PaymentMethod
            };
        }
    }
}