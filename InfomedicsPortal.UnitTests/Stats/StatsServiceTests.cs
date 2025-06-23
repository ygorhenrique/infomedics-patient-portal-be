using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Dentists;
using InfomedicsPortal.Core.Patients;
using InfomedicsPortal.Core.Stats;
using InfomedicsPortal.Core.Treatments;
using Moq;

namespace InfomedicsPortal.UnitTests.Stats;

public class StatsServiceTests
{
    private readonly Mock<ITreatmentsStorage> _treatmentsStorageMock;
    private readonly Mock<IDentistsStorage> _dentistsStorageMock;
    private readonly Mock<IPatientsStorage> _patientsStorageMock;
    private readonly Mock<IAppointmentsStorage> _appointmentsStorageMock;
    private readonly StatsService _service;

    public StatsServiceTests()
    {
        _treatmentsStorageMock = new Mock<ITreatmentsStorage>();
        _dentistsStorageMock = new Mock<IDentistsStorage>();
        _patientsStorageMock = new Mock<IPatientsStorage>();
        _appointmentsStorageMock = new Mock<IAppointmentsStorage>();
        _service = new StatsService(
            _treatmentsStorageMock.Object,
            _dentistsStorageMock.Object,
            _patientsStorageMock.Object,
            _appointmentsStorageMock.Object);
    }

    [Fact]
    public async Task GetStatsAsync_ReturnsCorrectStats()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var patients = new[]
        {
            new Patient { Id = Guid.NewGuid(), FullName = "John Doe", Address = "123 Main St", Photo = null },
            new Patient { Id = Guid.NewGuid(), FullName = "Jane Smith", Address = "456 Oak St", Photo = null }
        };

        var dentists = new[]
        {
            new Dentist { Id = Guid.NewGuid(), Name = "Bruce Wayne" },
            new Dentist { Id = Guid.NewGuid(), Name = "Selina Kyle" }
        };

        var treatments = new[]
        {
            new Treatment { Id = Guid.NewGuid(), Name = "Teeth Whitening" },
            new Treatment { Id = Guid.NewGuid(), Name = "Dental Crown" }
        };

        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = patients[0].Id, DentistId = dentists[0].Id, TreatmentId = treatments[0].Id, AppointmentDateTime = today },
            new Appointment { Id = Guid.NewGuid(), PatientId = patients[1].Id, DentistId = dentists[1].Id, TreatmentId = treatments[1].Id, AppointmentDateTime = tomorrow },
            new Appointment { Id = Guid.NewGuid(), PatientId = patients[0].Id, DentistId = dentists[0].Id, TreatmentId = treatments[0].Id, AppointmentDateTime = today }
        };

        _patientsStorageMock.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(patients);
        _dentistsStorageMock.Setup(x => x.GetAllDentistsAsync()).ReturnsAsync(dentists);
        _treatmentsStorageMock.Setup(x => x.GetAllTreatmentsAsync()).ReturnsAsync(treatments);
        _appointmentsStorageMock.Setup(x => x.GetAllAppointmentsAsync()).ReturnsAsync(appointments);

        // Act
        var stats = await _service.GetStatsAsync();

        // Assert
        Assert.Equal(patients.Length, stats.TotalPatients); // 2
        Assert.Equal(appointments.Length, stats.TotalUpcomingAppointments); // 3 (all appointments counted as upcoming)
        Assert.Equal(appointments.Count(a => a.AppointmentDateTime.Date == today), stats.TotalAppointmentsToday); // 2
        Assert.Equal(dentists.Length, stats.TotalDentists); // 2
    }

    [Fact]
    public async Task GetStatsAsync_EmptyData_ReturnsZeroStats()
    {
        // Arrange
        var emptyPatients = Array.Empty<Patient>();
        var emptyDentists = Array.Empty<Dentist>();
        var emptyTreatments = Array.Empty<Treatment>();
        var emptyAppointments = Array.Empty<Appointment>();

        _patientsStorageMock.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(emptyPatients);
        _dentistsStorageMock.Setup(x => x.GetAllDentistsAsync()).ReturnsAsync(emptyDentists);
        _treatmentsStorageMock.Setup(x => x.GetAllTreatmentsAsync()).ReturnsAsync(emptyTreatments);
        _appointmentsStorageMock.Setup(x => x.GetAllAppointmentsAsync()).ReturnsAsync(emptyAppointments);

        // Act
        var stats = await _service.GetStatsAsync();

        // Assert
        Assert.Equal(0, stats.TotalPatients);
        Assert.Equal(0, stats.TotalUpcomingAppointments);
        Assert.Equal(0, stats.TotalAppointmentsToday);
        Assert.Equal(0, stats.TotalDentists);
    }

    [Fact]
    public async Task GetStatsAsync_NoAppointmentsToday_ReturnsZeroToday()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var patients = new[] { new Patient { Id = Guid.NewGuid(), FullName = "John Doe", Address = "123 Main St", Photo = null } };
        var dentists = new[] { new Dentist { Id = Guid.NewGuid(), Name = "Bruce Wayne" } };
        var treatments = new[] { new Treatment { Id = Guid.NewGuid(), Name = "Teeth Whitening" } };
        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = patients[0].Id, DentistId = dentists[0].Id, TreatmentId = treatments[0].Id, AppointmentDateTime = tomorrow }
        };

        _patientsStorageMock.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(patients);
        _dentistsStorageMock.Setup(x => x.GetAllDentistsAsync()).ReturnsAsync(dentists);
        _treatmentsStorageMock.Setup(x => x.GetAllTreatmentsAsync()).ReturnsAsync(treatments);
        _appointmentsStorageMock.Setup(x => x.GetAllAppointmentsAsync()).ReturnsAsync(appointments);

        // Act
        var stats = await _service.GetStatsAsync();

        // Assert
        Assert.Equal(patients.Length, stats.TotalPatients); // 1
        Assert.Equal(appointments.Length, stats.TotalUpcomingAppointments); // 1
        Assert.Equal(0, stats.TotalAppointmentsToday); // No appointments today
        Assert.Equal(dentists.Length, stats.TotalDentists); // 1
    }
}