using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Patients;
using Moq;

namespace InfomedicsPortal.UnitTests.Patients;

public class PatientsServiceTests
{
    private readonly Mock<IPatientsStorage> _storageMock;
    private readonly Mock<IAppointmentsService> _appointmentsServiceMock;
    private readonly PatientsService _service;

    public PatientsServiceTests()
    {
        _storageMock = new Mock<IPatientsStorage>();
        _appointmentsServiceMock = new Mock<IAppointmentsService>(); 
        _service = new PatientsService(_appointmentsServiceMock.Object, _storageMock.Object);
    }

    [Fact]
    public async Task GetPatientAsync_ValidPatientId_ReturnsPatientWithAppointments()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient
        {
            Id = patientId,
            FullName = "John Doe",
            Address = "123 Main St",
            Photo = null,
            CreatedAtUtc = DateTime.UtcNow
        };

        var appointments = new[]
        {
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(1) },
            new Appointment { Id = Guid.NewGuid(), PatientId = patientId, DentistId = Guid.NewGuid(), TreatmentId = Guid.NewGuid(), AppointmentDateTime = DateTime.UtcNow.AddDays(2) }
        };

        _storageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);
        _appointmentsServiceMock.Setup(x => x.GetAppointmentsByPatientIdAsync(patientId)).ReturnsAsync(appointments);

        // Act
        var result = await _service.GetPatientAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patientId, result.Id);
        Assert.Equal(patient.FullName, result.FullName);
        Assert.Equal(patient.Address, result.Address);
        Assert.Equal(patient.Photo, result.Photo);
        Assert.Equal(patient.CreatedAtUtc, result.CreatedAtUtc);
        Assert.Equal(appointments, result.Appointments);
    }

    [Fact]
    public async Task GetPatientAsync_InvalidPatientId_ReturnsNull()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        _storageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync((Patient)null);
        _appointmentsServiceMock.Setup(x => x.GetAppointmentsByPatientIdAsync(patientId)).ReturnsAsync([]);

        // Act
        var result = await _service.GetPatientAsync(patientId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPatientAsync_NoAppointments_ReturnsPatientWithEmptyAppointments()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient
        {
            Id = patientId,
            FullName = "John Doe",
            Address = "123 Main St",
            Photo = null,
            CreatedAtUtc = DateTime.UtcNow
        };

        _storageMock.Setup(x => x.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);
        _appointmentsServiceMock.Setup(x => x.GetAppointmentsByPatientIdAsync(patientId)).ReturnsAsync(Array.Empty<Appointment>());

        // Act
        var result = await _service.GetPatientAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patientId, result.Id);
        Assert.Empty(result.Appointments);
    }

    [Fact]
    public async Task AddPatient_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new NewPatientRequest
        {
            FullName = "John Doe",
            Address = "123 Main St",
            Photo = null
        };

        var expectedPatient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Address = request.Address,
            Photo = request.Photo,
            CreatedAtUtc = DateTime.UtcNow
        };

        _storageMock.Setup(x => x.AddPatientAsync(It.IsAny<Patient>())).ReturnsAsync(expectedPatient);

        // Act
        var result = await _service.AddPatient(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPatient, result.Result);
    }

    [Fact]
    public async Task AddPatient_EmptyFullName_ReturnsFailure()
    {
        // Arrange
        var request = new NewPatientRequest
        {
            FullName = "",
            Address = "123 Main St",
            Photo = null
        };

        // Act
        var result = await _service.AddPatient(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Full name is required", result.Message);
    }

    [Fact]
    public async Task AddPatient_EmptyAddress_ReturnsFailure()
    {
        // Arrange
        var request = new NewPatientRequest
        {
            FullName = "John Doe",
            Address = "",
            Photo = null
        };

        // Act
        var result = await _service.AddPatient(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Address is required", result.Message);
    }

    [Fact]
    public async Task GetAllPatientsAsync_ReturnsPatients()
    {
        // Arrange
        var patients = new[]
        {
            new Patient { Id = Guid.NewGuid(), FullName = "John Doe", Address = "123 Main St", Photo = null, CreatedAtUtc = DateTime.UtcNow },
            new Patient { Id = Guid.NewGuid(), FullName = "Jane Smith", Address = "456 Oak St", Photo = null, CreatedAtUtc = DateTime.UtcNow }
        };

        _storageMock.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(patients);

        // Act
        var result = await _service.GetAllPatientsAsync();

        // Assert
        Assert.Equal(patients, result);
    }

    [Fact]
    public async Task GetAllPatientsAsync_Empty_ReturnsEmptyArray()
    {
        // Arrange
        _storageMock.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(Array.Empty<Patient>());

        // Act
        var result = await _service.GetAllPatientsAsync();

        // Assert
        Assert.Empty(result);
    }
}