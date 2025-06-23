using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Dentists;
using InfomedicsPortal.Core.Patients;
using InfomedicsPortal.Core.Treatments;

namespace InfomedicsPortal.Core.Stats;

public partial class StatsService : IStatsService
{
    private readonly ITreatmentsStorage _treatmentsStorage;
    private readonly IDentistsStorage _dentistsStorage;
    private readonly IPatientsStorage _patientsStorage;
    private readonly IAppointmentsStorage _appointmentsStorage;

    public StatsService(
        ITreatmentsStorage storage,
        IDentistsStorage dentists,
        IPatientsStorage patients,
        IAppointmentsStorage appointments)
    {
        this._appointmentsStorage =  appointments;
        this._dentistsStorage = dentists;
        this._patientsStorage = patients;
        this._treatmentsStorage = storage;
    }

    public async Task<Stats> GetStatsAsync()
    {
        var getTotalAppointments = _appointmentsStorage.GetAllAppointmentsAsync();
        var getTotalDentists = _dentistsStorage.GetAllDentistsAsync();
        var getTotalPatients = _patientsStorage.GetAllPatientsAsync();
        var  getTotalTreatments = _treatmentsStorage.GetAllTreatmentsAsync();

        await Task.WhenAll(getTotalAppointments, getTotalDentists, getTotalPatients, getTotalTreatments);
        
        var totalPatients = getTotalPatients.Result.Length;
        var totalAppointments = getTotalAppointments.Result.Length;
        var totalDentists = getTotalDentists.Result.Length;
        
        var appointmentsToday = getTotalAppointments.Result
            .Count(it => it.AppointmentDateTime.Date == DateTime.UtcNow.Date);
        
        return new Stats()
        {
            TotalPatients = totalPatients,
            TotalUpcomingAppointments = totalAppointments,
            TotalAppointmentsToday = appointmentsToday,
            TotalDentists = totalDentists
        };
    }
}