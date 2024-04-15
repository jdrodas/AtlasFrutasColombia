using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumenController(ResumenService resumenService) : Controller
    {
        private readonly ResumenService _resumenService = resumenService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var elResumen = await _resumenService
                .GetAllAsync();

            return Ok(elResumen);
        }
    }
}