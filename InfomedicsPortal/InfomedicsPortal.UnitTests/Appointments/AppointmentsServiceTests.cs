using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Patients;
using Moq;

namespace InfomedicsPortal.UnitTests.Appointments;

public class AppointmentsServiceTests
{
    private readonly Mock<IAppointmentsStorage> _appointmentsStorageMock;
    private readonly Mock<IPatientsStorage> _patientsStorageMock;
    private readonly IAppointmentsService _service;

    public AppointmentsServiceTests()
    {
        _appointmentsStorageMock = new Mock<IAppointmentsStorage>();
        _patientsStorageMock = new Mock<IPatientsStorage>();
        _service = new AppointmentsService(_appointmentsStorageMock.Object, _patientsStorageMock.Object);
    }

    [Fact]
    public async Task AddAppointmentAsync_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new NewAppointmentRequest
        {
            PatientId = Guid.NewGuid(),
            DentistId = Guid.NewGuid(),
            TreatmentId = Guid.NewGuid(),
            AppointmentDateTime = DateTime.UtcNow.AddDays(1)
        };

        var expectedAppointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            DentistId = request.DentistId,
            TreatmentId = request.TreatmentId,
            AppointmentDateTime = request.AppointmentDateTime,
            CreationDateUtc = DateTime.UtcNow,
            LastModifiedDateUtc = DateTime.UtcNow
        };

        _appointmentsStorageMock.Setup(x => x.AddAppointmentAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(expectedAppointment);

        // Act
        var result = await _service.AddAppointmentAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedAppointment, result.Result);
    }

    [Fact]
    public async Task AddAppointmentAsync_PastDateTime_ReturnsFailure()
    {
        // Arrange
        var request = new NewAppointmentRequest
        {
            PatientId = Guid.NewGuid(),
            DentistId = Guid.NewGuid(),
            TreatmentId = Guid.NewGuid(),
            AppointmentDateTime = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var result = await _service.AddAppointmentAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Appointment date cannot be in the past", result.Message);
    }

    [Fact]
    public async Task AddAppointmentAsync_EmptyPatientId_ReturnsFailure()
    {
        // Arrange
        var request = new NewAppointmentRequest
        {
            PatientId = Guid.Empty,
            DentistId = Guid.NewGuid(),
            TreatmentId = Guid.NewGuid(),
            AppointmentDateTime = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _service.AddAppointmentAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Patient ID cannot be empty", result.Message);
    }

    [Fact]
    public async Task AddAppointmentAsync_EmptyDentistId_ReturnsFailure()
    {
        // Arrange
        var request = new NewAppointmentRequest
        {
            PatientId = Guid.NewGuid(),
            DentistId = Guid.Empty,
            TreatmentId = Guid.NewGuid(),
            AppointmentDateTime = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _service.AddAppointmentAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Dentist ID cannot be empty", result.Message);
    }

    [Fact]
    public async Task AddAppointmentAsync_EmptyTreatmentId_ReturnsFailure()
    {
        // Arrange
        var request = new NewAppointmentRequest
        {
            PatientId = Guid.NewGuid(),
            DentistId = Guid.NewGuid(),
            TreatmentId = Guid.Empty,
            AppointmentDateTime = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _service.AddAppointmentAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Treatment ID cannot be empty", result.Message);
    }

    [Fact]
    public async Task GetAllAppointmentAsync_ReturnsPatientAppointments()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(1) },
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(2) }
        };

        var patient = new Patient { Id = patientId, FullName = "John Doe", Address = "123 Main St", Photo = null };

        _appointmentsStorageMock.Setup(x => x.GetAllAppointmentsAsync()).ReturnsAsync(appointments);
        _patientsStorageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);

        // Act
        var appointmentsAsync = _service.GetAllAppointmentAsync();
        var results = new List<AppointmentsService.PatientAppointment>();
        await foreach (var appointment in appointmentsAsync)
        {
            results.Add(appointment);
        }

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.Equal(patientId, r.PatientId));
        Assert.All(results, r => Assert.Equal("scheduled", r.Status)); // Assuming future dates
    }

    [Fact]
    public async Task GetAllAppointmentAsync_NullPatient_SkipsAppointment()
    {
        // Arrange
        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(1) }
        };

        _appointmentsStorageMock.Setup(x => x.GetAllAppointmentsAsync()).ReturnsAsync(appointments);
        _patientsStorageMock.Setup(x => x.GetPatientByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Patient)null);

        // Act
        var appointmentsAsync = _service.GetAllAppointmentAsync();
        var results = new List<AppointmentsService.PatientAppointment>();
        await foreach (var appointment in appointmentsAsync)
        {
            results.Add(appointment);
        }

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_ValidPatientId_ReturnsAppointments()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(1) },
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(2) }
        };

        var patient = new Patient { Id = patientId, FullName = "John Doe", Address = "123 Main St", Photo = null };

        _patientsStorageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);
        _appointmentsStorageMock.Setup(x => x.GetAppointmentsByPatientId(patientId)).ReturnsAsync(appointments);

        // Act
        var result = await _service.GetAppointmentsByPatientIdAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_NullPatient_ReturnsNull()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        _patientsStorageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync((Patient)null);

        // Act
        var result = await _service.GetAppointmentsByPatientIdAsync(patientId);

        // Assert
        Assert.Null(result);
    }
}