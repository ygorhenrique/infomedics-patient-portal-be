namespace InfomedicsPortal.Core.Patients;

public class NewPatientRequest
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public Photo? Photo { get; set; }
}