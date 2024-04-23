using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Epoca unaEpoca)
        {
            try
            {
                var epocaCreada = await _epocaService
                    .CreateAsync(unaEpoca);

                return Ok(epocaCreada);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

    }
}
