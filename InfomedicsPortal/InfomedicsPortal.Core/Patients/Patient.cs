namespace InfomedicsPortal.Core.Patients;

public class Patient
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string Photo { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}