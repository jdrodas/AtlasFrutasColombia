using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_NoSQL_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClimasController(ClimaService climaService) : Controller
    {
        private readonly ClimaService _climaService = climaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losDepartamentos = await _climaService
                .GetAllAsync();

            return Ok(losDepartamentos);
        }

        [HttpGet("{clima_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid clima_id)
        {
            try
            {
                var unaFruta = await _climaService
                    .GetByIdAsync(clima_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{clima_id:Guid}/Frutas")]
        public async Task<IActionResult> GetProducedFruitsAsync(Guid clima_id)
        {
            try
            {
                var lasFrutasProducidas = await _climaService
                    .GetProducedFruitsAsync(clima_id);

                return Ok(lasFrutasProducidas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

    }
}
