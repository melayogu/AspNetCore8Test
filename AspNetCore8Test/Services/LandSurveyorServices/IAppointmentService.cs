using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<bool> AppointmentExistsAsync(int id);
        Task<IEnumerable<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int days = 7);
    }
}