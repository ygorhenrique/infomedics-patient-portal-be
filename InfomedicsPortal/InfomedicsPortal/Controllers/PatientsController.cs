using InfomedicsPortal.Core.Patients;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ILogger<PatientsController> _logger;
    private readonly PatientsService _patientsService;

    public PatientsController(PatientsService patientsService, ILogger<PatientsController> logger)
    {
        _patientsService = patientsService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> PostPatient([FromBody] PatientsService.NewPatientRequest patient)
    {
        var execResult = await _patientsService.AddPatient(patient);
        if (!execResult.IsSuccess)
        {
            return BadRequest(execResult.Message);
        }
        
        return Ok(execResult.Result);
    }
    
    [HttpGet("{patientId}")]
    public async Task<ActionResult<Patient?>> GetPatient([FromRoute] Guid patientId)
    {
        var patient = await _patientsService.GetPatientAsync(patientId);
        if (patient == null)
        {
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