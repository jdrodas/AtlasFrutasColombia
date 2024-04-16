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

        [HttpGet("{fruta_id:Guid}")]
        public async Task<IActionResult> GetFruitDetailsByIdAsync(Guid fruta_id)
        {
            try
            {
                var unaFruta = await _frutaService
                    .GetFruitDetailsByIdAsync(fruta_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{fruta_id:Guid}/Nutricion")]
        public async Task<IActionResult> GetNutritiousFruitByIdAsync(Guid fruta_id)
        {
            try
            {
                var unaFrutaNutritiva = await _frutaService
                    .GetNutritiousFruitByIdAsync(fruta_id);

                return Ok(unaFrutaNutritiva);
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

        [HttpPost("{fruta_id:Guid}/Nutricion")]
        public async Task<IActionResult> CreateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
        {
            try
            {
                var frutaCreada = await _frutaService
                    .CreateNutritionDetailsAsync(fruta_id, unaInformacionNutricional);

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

        [HttpPut("{fruta_id:Guid}/Nutricion")]
        public async Task<IActionResult> UpdateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
        {
            try
            {
                var frutaActualizada = await _frutaService
                    .UpdateNutritionDetailsAsync(fruta_id, unaInformacionNutricional);

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

        [HttpDelete("{fruta_id:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid fruta_id)
        {
            try
            {
                var nombreFrutaBorrada = await _frutaService
                    .RemoveAsync(fruta_id);

                return Ok($"La fruta {nombreFrutaBorrada} fue eliminada correctamente!");
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

        [HttpDelete("{fruta_id:Guid}/Nutricion")]
        public async Task<IActionResult> RemoveNutritionDetailsAsync(Guid fruta_id)
        {
            try
            {
                var frutaSinNutricion = await _frutaService
                    .RemoveNutritionDetailsAsync(fruta_id);

                return Ok($"La fruta {frutaSinNutricion.Nombre} ya no tiene información nutricional!");
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
