using InfomedicsPortal.Core.Stats;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _service;
    private readonly ILogger<AppointmentsController> _logger;

    public StatsController(IStatsService service, ILogger<AppointmentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public Task<StatsService.Stats> GetAllAppointments()
    {
        return _service.GetStatsAsync();
    }
}