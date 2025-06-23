using InfomedicsPortal.Core.Appointments;

namespace InfomedicsPortal.Core.Patients;

public partial class PatientsService
{
    public class PatientWithAppointments
    {
        public Guid Id { get; }
        public string FullName { get; }
        public string Address { get; }
        public Photo Photo { get; }
        public DateTime CreatedAtUtc { get; }
        public Appointment[] Appointments { get; }

        public PatientWithAppointments(Patient? getPatientResult, Appointment[] appointments)
        {
            if (getPatientResult == null)
            {
                throw new ArgumentNullException(nameof(getPatientResult));
            }

            if (appointments == null)
            {
                throw new ArgumentNullException(nameof(appointments));
            }
            
            Id = getPatientResult.Id;
            FullName = getPatientResult.FullName;
            Address = getPatientResult.Address;
            Photo = getPatientResult.Photo;
            CreatedAtUtc = getPatientResult.CreatedAtUtc;
            Appointments = appointments;
        }
    }
}