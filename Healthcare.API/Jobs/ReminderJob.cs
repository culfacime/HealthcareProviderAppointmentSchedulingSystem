using Healthcare.Core.Entities;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;

namespace Healthcare.API.Jobs
{
    public class ReminderJob
    {
        public static object myLock = new object();
        private readonly IGenericService<Appointment> _appointmentService;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;


        public ReminderJob(IGenericService<Appointment> appointmentService, IEmailService emailService, ISmsService smsService, IUnitOfWork unitOfWork)
        {
            _appointmentService = appointmentService;
            _emailService = emailService;
            _smsService = smsService;
            _unitOfWork = unitOfWork;
        }

        public void Run()
        {
            lock (myLock)
            {
                var appointmentList = _appointmentService.GetList(x => x.Reminded == false && x.RemindingDate >= DateTime.Now, includes: new string[] { "Patient","Location" }).ToList();

                foreach(Appointment appointment in appointmentList)
                {
                    var message = $"Dear {appointment.Patient.Name} {appointment.Patient.Surname}, You have an appointment in {appointment.Location.LocationName} on {appointment.AppointmentDate.ToString("MMMM dd")} at {appointment.AppointmentDate.ToString("HH:mm:ss")}";

                    if (appointment.RemindPatientViaEmail)
                    {
                        _emailService.Send(message);
                    }

                    if (appointment.RemindPatientViaSms)
                    {
                        _smsService.Send(message);
                    }

                    appointment.Reminded = true;
                 
                    _appointmentService.UpdateAsync(appointment).Wait();
                    _unitOfWork.Commit();
                }
            }
        }
    }
}
