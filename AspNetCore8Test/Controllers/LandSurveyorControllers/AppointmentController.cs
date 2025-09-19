using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCore8Test.Models.LandSurveyorModels;
using AspNetCore8Test.Services.LandSurveyorServices;

namespace AspNetCore8Test.Controllers.LandSurveyorControllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;

        public AppointmentController(IAppointmentService appointmentService, ICustomerService customerService)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/Create
        public async Task<IActionResult> Create()
        {
            await LoadCustomers();
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CustomerId,AppointmentDate,Duration,Location,Status,Notes")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.CreateAppointmentAsync(appointment);
                TempData["SuccessMessage"] = "預約已成功建立！";
                return RedirectToAction(nameof(Index));
            }
            await LoadCustomers();
            return View(appointment);
        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            await LoadCustomers();
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CustomerId,AppointmentDate,Duration,Location,Status,CreatedDate,LastModifiedDate,Notes")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appointment.LastModifiedDate = DateTime.Now;
                    await _appointmentService.UpdateAppointmentAsync(appointment);
                    TempData["SuccessMessage"] = "預約資料已成功更新！";
                }
                catch (Exception)
                {
                    if (!await _appointmentService.AppointmentExistsAsync(appointment.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            await LoadCustomers();
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "預約已成功刪除！";
            }
            else
            {
                TempData["ErrorMessage"] = "刪除預約時發生錯誤！";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewData["CustomerId"] = new SelectList(customers, "Id", "Name");
        }
    }
}