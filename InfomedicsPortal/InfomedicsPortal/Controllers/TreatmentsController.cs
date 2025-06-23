using InfomedicsPortal.Core.Treatments;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class TreatmentsController : ControllerBase
{
    private readonly TreatmentsService _service;
    private readonly ILogger<TreatmentsController> _logger;

    public TreatmentsController(TreatmentsService treatmentsService, ILogger<TreatmentsController> logger)
    {
        _service = treatmentsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Treatment[]> GetAll()
    {
        var result = await _service.GetAllTreatments();
        return result;
    }
}