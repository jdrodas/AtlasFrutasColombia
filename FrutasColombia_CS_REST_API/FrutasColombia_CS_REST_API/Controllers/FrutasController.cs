using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Models;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrutasController(FrutaService frutaService) : Controller
    {
        private readonly FrutaService _frutaService = frutaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasFrutas = await _frutaService
                .GetAllAsync();

            return Ok(lasFrutas);
        }

        [HttpGet("{fruta_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int fruta_id)
        {
            try
            {
                var unaFruta = await _frutaService
                    .GetByIdAsync(fruta_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Fruta unaFruta)
        {
            try
            {
                var frutaCreada = await _frutaService
                    .CreateAsync(unaFruta);

                return Ok(frutaCreada);
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
        public async Task<IActionResult> UpdateAsync(Fruta unaFruta)
        {
            try
            {
                var frutaActualizada = await _frutaService
                    .UpdateAsync(unaFruta);

                return Ok(frutaActualizada);
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
