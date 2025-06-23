namespace InfomedicsPortal.Core.Stats;

public partial class StatsService
{
    public class Stats
    {
        public int TotalPatients { get; set; }
        public int TotalDentists { get; set; }
        public int TotalUpcomingAppointments { get; set; }
        public int TotalAppointmentsToday { get; set; }
    }
}