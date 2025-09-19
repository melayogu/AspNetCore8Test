using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;
using FluentValidation;

namespace AspNetCore8Test.Controllers.GasControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomersController(
            ICustomerService customerService,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerService = customerService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// 取得所有客戶
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// <summary>
        /// 根據ID取得客戶
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"找不到ID為 {id} 的客戶");
            }
            return Ok(customer);
        }

        /// <summary>
        /// 根據客戶編號取得客戶
        /// </summary>
        [HttpGet("by-number/{customerNumber}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByNumber(string customerNumber)
        {
            var customer = await _customerService.GetCustomerByNumberAsync(customerNumber);
            if (customer == null)
            {
                return NotFound($"找不到客戶編號為 {customerNumber} 的客戶");
            }
            return Ok(customer);
        }

        /// <summary>
        /// 搜尋客戶
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers([FromQuery] string searchTerm = "")
        {
            var customers = await _customerService.SearchCustomersAsync(searchTerm);
            return Ok(customers);
        }

        /// <summary>
        /// 建立新客戶
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            // 檢查客戶編號是否已存在
            if (await _customerService.CustomerNumberExistsAsync(createCustomerDto.CustomerNumber))
            {
                return Conflict($"客戶編號 {createCustomerDto.CustomerNumber} 已存在");
            }

            var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// 更新客戶資料
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
        {
            if (id != updateCustomerDto.Id)
            {
                return BadRequest("路徑中的ID與資料中的ID不符");
            }

            var validationResult = await _updateValidator.ValidateAsync(updateCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
            if (customer == null)
            {
                return NotFound($"找不到ID為 {id} 的客戶");
            }

            return Ok(customer);
        }

        /// <summary>
        /// 刪除客戶
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            if (!success)
            {
                return NotFound($"找不到ID為 {id} 的客戶");
            }

            return NoContent();
        }

        /// <summary>
        /// 檢查客戶是否存在
        /// </summary>
        [HttpHead("{id}")]
        public async Task<IActionResult> CustomerExists(int id)
        {
            var exists = await _customerService.CustomerExistsAsync(id);
            return exists ? Ok() : NotFound();
        }
    }
}