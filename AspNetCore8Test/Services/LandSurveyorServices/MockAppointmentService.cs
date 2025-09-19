using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public class MockAppointmentService : IAppointmentService
    {
        private static readonly List<Appointment> _appointments = new List<Appointment>
        {
            new Appointment
            {
                Id = 1,
                Title = "王小明不動產諮詢",
                Description = "討論房屋所有權移轉相關事宜",
                CustomerId = 1,
                AppointmentDate = DateTime.Today,
                AppointmentTime = new TimeSpan(10, 0, 0),
                EstimatedDuration = 60,
                Location = "事務所會議室",
                Status = AppointmentStatus.Scheduled,
                ServiceType = ServiceType.Consultation,
                CreatedDate = DateTime.Now.AddDays(-2),
                Notes = "客戶初次諮詢"
            },
            new Appointment
            {
                Id = 2,
                Title = "李美華案件進度說明",
                Description = "說明抵押權設定進度及所需文件",
                CustomerId = 2,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(14, 0, 0),
                EstimatedDuration = 30,
                Location = "事務所會議室",
                Status = AppointmentStatus.Scheduled,
                ServiceType = ServiceType.CaseExplanation,
                CreatedDate = DateTime.Now.AddDays(-1)
            },
            new Appointment
            {
                Id = 3,
                Title = "張志明文件交付",
                Description = "交付土地測量完成文件",
                CustomerId = 3,
                AppointmentDate = DateTime.Today.AddDays(-3),
                AppointmentTime = new TimeSpan(16, 0, 0),
                EstimatedDuration = 15,
                Location = "事務所櫃台",
                Status = AppointmentStatus.Completed,
                ServiceType = ServiceType.DocumentPreparation,
                CreatedDate = DateTime.Now.AddDays(-5),
                LastModifiedDate = DateTime.Now.AddDays(-3),
                Notes = "文件已順利交付"
            }
        };

        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "王小明", IdNumber = "A123456789", Phone = "0912345678" },
            new Customer { Id = 2, Name = "李美華", IdNumber = "B987654321", Phone = "0987654321" },
            new Customer { Id = 3, Name = "張志明", IdNumber = "C111222333", Phone = "0955666777" }
        };

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            await Task.Delay(10); // 模擬異步操作
            
            // 設定客戶導航屬性
            foreach (var appointment in _appointments)
            {
                appointment.Customer = _customers.FirstOrDefault(c => c.Id == appointment.CustomerId) ?? new Customer();
            }
            
            return _appointments.OrderBy(a => a.AppointmentDate);
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            await Task.Delay(10);
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                appointment.Customer = _customers.FirstOrDefault(c => c.Id == appointment.CustomerId) ?? new Customer();
            }
            return appointment;
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(10);
            appointment.Id = _appointments.Max(a => a.Id) + 1;
            appointment.CreatedDate = DateTime.Now;
            appointment.Status = AppointmentStatus.Scheduled;
            _appointments.Add(appointment);
            return appointment;
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(10);
            var existingAppointment = _appointments.FirstOrDefault(a => a.Id == appointment.Id);
            if (existingAppointment != null)
            {
                existingAppointment.Title = appointment.Title;
                existingAppointment.Description = appointment.Description;
                existingAppointment.CustomerId = appointment.CustomerId;
                existingAppointment.AppointmentDate = appointment.AppointmentDate;
                existingAppointment.AppointmentTime = appointment.AppointmentTime;
                existingAppointment.EstimatedDuration = appointment.EstimatedDuration;
                existingAppointment.Location = appointment.Location;
                existingAppointment.Status = appointment.Status;
                existingAppointment.ServiceType = appointment.ServiceType;
                existingAppointment.LastModifiedDate = DateTime.Now;
                existingAppointment.Notes = appointment.Notes;
                return existingAppointment;
            }
            throw new ArgumentException("預約不存在");
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            await Task.Delay(10);
            var appointmentToRemove = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointmentToRemove != null)
            {
                return _appointments.Remove(appointmentToRemove);
            }
            return false;
        }

        public async Task<bool> AppointmentExistsAsync(int id)
        {
            await Task.Delay(10);
            return _appointments.Any(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            await Task.Delay(10);
            var appointments = _appointments.Where(a => a.CustomerId == customerId).ToList();
            foreach (var appointment in appointments)
            {
                appointment.Customer = _customers.FirstOrDefault(c => c.Id == appointment.CustomerId) ?? new Customer();
            }
            return appointments.OrderBy(a => a.AppointmentDate);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            await Task.Delay(10);
            var appointments = _appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();
            foreach (var appointment in appointments)
            {
                appointment.Customer = _customers.FirstOrDefault(c => c.Id == appointment.CustomerId) ?? new Customer();
            }
            return appointments.OrderBy(a => a.AppointmentDate);
        }

        public async Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync()
        {
            return await GetAppointmentsByDateAsync(DateTime.Today);
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int days = 7)
        {
            await Task.Delay(10);
            var endDate = DateTime.Today.AddDays(days);
            var appointments = _appointments.Where(a => a.AppointmentDate.Date >= DateTime.Today && 
                                                       a.AppointmentDate.Date <= endDate &&
                                                       a.Status == AppointmentStatus.Scheduled).ToList();
            foreach (var appointment in appointments)
            {
                appointment.Customer = _customers.FirstOrDefault(c => c.Id == appointment.CustomerId) ?? new Customer();
            }
            return appointments.OrderBy(a => a.AppointmentDate);
        }
    }
}