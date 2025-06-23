using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Patients;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ILogger<PatientsController> _logger;
    private readonly IPatientsService _patientsService;

    public PatientsController(
        IPatientsService patientsService,
        ILogger<PatientsController> logger)
    {
        _patientsService = patientsService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> PostPatient([FromBody] NewPatientRequest patient)
    {
        var execResult = await _patientsService.AddPatientAsync(patient);
        if (!execResult.IsSuccess)
        {
            _logger.LogWarning("Failed to create patient: {Message}", execResult.Message);
            return BadRequest(execResult.Message);
        }
        
        return Ok(execResult.Result);
    }
    
    [HttpGet("{patientId}")]
    public async Task<ActionResult<PatientsService.PatientWithAppointments>> GetPatient([FromRoute] Guid patientId)
    {
        var patient = await _patientsService.GetPatientAsync(patientId);
        if (patient == null)
        {
            _logger.LogWarning("Patient with ID {PatientId} not found", patientId);
            return NotFound($"Patient({patientId}) not found");
        }
        return Ok(patient);
    }
    
    [HttpGet]
    public async Task<ActionResult<Patient?>> GetAllPatients()
    {
        var patient = await _patientsService.GetAllPatientsAsync();
        return Ok(patient);
    }
}