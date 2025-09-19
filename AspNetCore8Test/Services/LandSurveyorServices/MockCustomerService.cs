using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public class MockCustomerService : ICustomerService
    {
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "王小明",
                IdNumber = "A123456789",
                Phone = "0912345678",
                Email = "wang@example.com",
                Address = "台北市中正區重慶南路一段100號",
                CreatedDate = DateTime.Now.AddMonths(-6),
                LastModifiedDate = DateTime.Now.AddMonths(-1),
                IsActive = true
            },
            new Customer
            {
                Id = 2,
                Name = "李美華",
                IdNumber = "B987654321",
                Phone = "0987654321",
                Email = "li@example.com",
                Address = "新北市板橋區中山路二段50號",
                CreatedDate = DateTime.Now.AddMonths(-4),
                LastModifiedDate = DateTime.Now.AddDays(-10),
                IsActive = true
            },
            new Customer
            {
                Id = 3,
                Name = "陳志明",
                IdNumber = "C111222333",
                Phone = "0955123456",
                Email = "chen@example.com",
                Address = "桃園市桃園區復興路300號",
                CreatedDate = DateTime.Now.AddMonths(-2),
                LastModifiedDate = DateTime.Now.AddDays(-5),
                IsActive = true
            }
        };

        private static int _nextId = 4;

        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return Task.FromResult(_customers.Where(c => c.IsActive).AsEnumerable());
        }

        public Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id && c.IsActive);
            return Task.FromResult(customer);
        }

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            customer.Id = _nextId++;
            customer.CreatedDate = DateTime.Now;
            customer.LastModifiedDate = DateTime.Now;
            customer.IsActive = true;
            _customers.Add(customer);
            return Task.FromResult(customer);
        }

        public Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.IdNumber = customer.IdNumber;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                existingCustomer.Notes = customer.Notes;
                existingCustomer.LastModifiedDate = DateTime.Now;
            }
            return Task.FromResult(existingCustomer ?? customer);
        }

        public Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                customer.IsActive = false;
                customer.LastModifiedDate = DateTime.Now;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> CustomerExistsAsync(int id)
        {
            return Task.FromResult(_customers.Any(c => c.Id == id && c.IsActive));
        }

        public Task<bool> IdNumberExistsAsync(string idNumber, int? excludeId = null)
        {
            return Task.FromResult(_customers.Any(c => c.IdNumber == idNumber && c.IsActive && 
                                                      (excludeId == null || c.Id != excludeId)));
        }
    }
}