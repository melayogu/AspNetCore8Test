using AspNetCore8Test.Models.GasModels;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Services.GasServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto?> GetCustomerByNumberAsync(string customerNumber);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm);
        Task<bool> CustomerExistsAsync(int id);
        Task<bool> CustomerNumberExistsAsync(string customerNumber);
    }

    public class CustomerService : ICustomerService
    {
        // 模擬資料庫，實際應用中應該使用 Entity Framework 或其他 ORM
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                CustomerNumber = "C001",
                Name = "張三",
                Type = "住宅",
                Address = "台北市信義區信義路一段100號",
                Phone = "02-12345678",
                Email = "zhang@example.com",
                TaxId = "12345678",
                RegisterDate = DateTime.Now.AddYears(-2),
                IsActive = true,
                Status = "Active"
            },
            new Customer
            {
                Id = 2,
                CustomerNumber = "C002",
                Name = "李四",
                Type = "商業",
                Address = "台北市大安區復興南路二段200號",
                Phone = "02-87654321",
                Email = "li@example.com",
                TaxId = "87654321",
                RegisterDate = DateTime.Now.AddYears(-1),
                IsActive = true,
                Status = "Active"
            },
            new Customer
            {
                Id = 3,
                CustomerNumber = "C003",
                Name = "王五工業股份有限公司",
                Type = "工業",
                Address = "新北市土城區工業路300號",
                Phone = "02-98765432",
                Email = "wang@industry.com",
                TaxId = "23456789",
                RegisterDate = DateTime.Now.AddMonths(-6),
                IsActive = true,
                Status = "Active"
            }
        };
        private static int _nextId = 4;

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            await Task.Delay(1); // 模擬異步操作
            return _customers.Select(MapToDto);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            await Task.Delay(1);
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<CustomerDto?> GetCustomerByNumberAsync(string customerNumber)
        {
            await Task.Delay(1);
            var customer = _customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            await Task.Delay(1);
            
            var customer = new Customer
            {
                Id = _nextId++,
                CustomerNumber = createCustomerDto.CustomerNumber,
                Name = createCustomerDto.Name,
                Type = createCustomerDto.Type,
                Address = createCustomerDto.Address,
                Phone = createCustomerDto.Phone,
                Email = createCustomerDto.Email,
                TaxId = createCustomerDto.TaxId,
                RegisterDate = DateTime.Now,
                IsActive = true,
                Status = "Active"
            };

            _customers.Add(customer);
            return MapToDto(customer);
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
        {
            await Task.Delay(1);
            
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return null;

            customer.CustomerNumber = updateCustomerDto.CustomerNumber;
            customer.Name = updateCustomerDto.Name;
            customer.Type = updateCustomerDto.Type;
            customer.Address = updateCustomerDto.Address;
            customer.Phone = updateCustomerDto.Phone;
            customer.Email = updateCustomerDto.Email;
            customer.TaxId = updateCustomerDto.TaxId;
            customer.IsActive = updateCustomerDto.IsActive;
            customer.Status = updateCustomerDto.Status;

            return MapToDto(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            await Task.Delay(1);
            
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return false;

            _customers.Remove(customer);
            return true;
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm)
        {
            await Task.Delay(1);
            
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCustomersAsync();

            var results = _customers.Where(c =>
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.CustomerNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );

            return results.Select(MapToDto);
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            await Task.Delay(1);
            return _customers.Any(c => c.Id == id);
        }

        public async Task<bool> CustomerNumberExistsAsync(string customerNumber)
        {
            await Task.Delay(1);
            return _customers.Any(c => c.CustomerNumber == customerNumber);
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                Name = customer.Name,
                Type = customer.Type,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                TaxId = customer.TaxId,
                RegisterDate = customer.RegisterDate,
                IsActive = customer.IsActive,
                Status = customer.Status
            };
        }
    }
}