using InfomedicsPortal.Core.Dentists;
using Microsoft.AspNetCore.Mvc;

namespace InfomedicsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class DentistsController : ControllerBase
{
    private readonly DentistsService _service;
    private readonly ILogger<DentistsController> _logger;

    public DentistsController(DentistsService dentistsService, ILogger<DentistsController> logger)
    {
        _service = dentistsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Dentist[]> GetAllDentists()
    {
        var result = await _service.GetAllDentistsAsync();
        return result;
    }
}