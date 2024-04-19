using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpocasController(EpocaService epocaService) : Controller
    {
        private readonly EpocaService _epocaService = epocaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losDepartamentos = await _epocaService
                .GetAllAsync();

            return Ok(losDepartamentos);
        }

        [HttpGet("{epoca_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid epoca_id)
        {
            try
            {
                var unaFruta = await _epocaService
                    .GetByIdAsync(epoca_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{epoca_id:Guid}/Frutas")]
        public async Task<IActionResult> GetProducedFruitsAsync(Guid epoca_id)
        {
            try
            {
                var lasFrutasProducidas = await _epocaService
                    .GetProducedFruitsAsync(epoca_id);

                return Ok(lasFrutasProducidas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
