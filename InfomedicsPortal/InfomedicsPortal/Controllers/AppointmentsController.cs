using InfomedicsPortal.Core.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _service;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(IAppointmentsService appointmentsService, ILogger<AppointmentsController> logger)
    {
        _service = appointmentsService;
        _logger = logger;
    }

    [HttpGet]
    public IAsyncEnumerable<AppointmentsService.PatientAppointment> GetAllAppointments()
    {
        return _service.GetAllAppointmentAsync();
    }
    
    [HttpGet("{patientId}")]
    public async Task<ActionResult<Appointment[]>> GetAppointmentsByPatientidAsync([FromRoute] Guid patientId)
    {
        var patientAppointments = await _service.GetAppointmentsByPatientIdAsync(patientId);
        if (patientAppointments == null)
        {
            _logger.LogWarning("No appointments found: Patient with ID {PatientId} not found", patientId);
            return BadRequest();
        }
        
        return Ok(patientAppointments);
    }
    
    [HttpPost]
    public async Task<ActionResult<Appointment>> ScheduleNewAppointment([FromBody] NewAppointmentRequest appointmentRequest)
    {
        var scheduleAppointmentResult = await _service.AddAppointmentAsync(appointmentRequest);
        if (!scheduleAppointmentResult.IsSuccess)
        {
            _logger.LogWarning(
                "Failed to schedule appointment for PatientId {PatientId}: {Message}",
                appointmentRequest.PatientId,
                scheduleAppointmentResult.Message
            );
            return BadRequest(scheduleAppointmentResult.Message);
        }

        return Ok(scheduleAppointmentResult.Result);
    }
}