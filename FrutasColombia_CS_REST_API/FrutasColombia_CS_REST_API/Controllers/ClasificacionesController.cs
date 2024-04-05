using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClasificacionesController(ClasificacionService clasificacionService) : Controller
    {
        private readonly ClasificacionService _clasificacionService = clasificacionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasClasificaciones = await _clasificacionService
                .GetAllAsync();

            return Ok(lasClasificaciones);
        }
    }
}
