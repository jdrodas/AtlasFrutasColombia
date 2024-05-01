using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Models;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Clima unClima)
        {
            try
            {
                var climaCreado = await _climaService
                    .CreateAsync(unClima);

                return Ok(climaCreado);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Clima unClima)
        {
            try
            {
                var climaActualizado = await _climaService
                    .UpdateAsync(unClima);

                return Ok(climaActualizado);
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

        [HttpDelete("{clima_id:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid clima_id)
        {
            try
            {
                var nombreClimaBorrado = await _climaService
                    .RemoveAsync(clima_id);

                return Ok($"El clima {nombreClimaBorrado} fue eliminada correctamente!");
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
