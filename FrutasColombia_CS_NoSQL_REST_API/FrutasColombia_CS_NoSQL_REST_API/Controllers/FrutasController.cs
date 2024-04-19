using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using FrutasColombia_CS_NoSQL_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_NoSQL_REST_API.Controllers
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

        [HttpGet("{fruta_id:string?}")]
        public async Task<IActionResult> GetFruitDetailsByIdAsync(string? fruta_id)
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

        [HttpGet("{fruta_id:string?}/Nutricion")]
        public async Task<IActionResult> GetNutritiousFruitByIdAsync(string? fruta_id)
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

        [HttpGet("{fruta_id:string?}/Taxonomia")]
        public async Task<IActionResult> GetClassifiedFruitByIdAsync(string? fruta_id)
        {
            try
            {
                var unaFrutaClasificada = await _frutaService
                    .GetClassifiedFruitByIdAsync(fruta_id);

                return Ok(unaFrutaClasificada);
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

        [HttpPost("{fruta_id:string?}/Nutricion")]
        public async Task<IActionResult> CreateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional)
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

        [HttpPost("{fruta_id:string?}/Taxonomia")]
        public async Task<IActionResult> CreateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            try
            {
                var frutaCreada = await _frutaService
                    .CreateTaxonomicDetailsAsync(fruta_id, unaInformacionTaxonomica);

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

        [HttpPut("{fruta_id:string?}/Nutricion")]
        public async Task<IActionResult> UpdateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional)
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

        [HttpPut("{fruta_id:string?}/Taxonomia")]
        public async Task<IActionResult> UpdateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            try
            {
                var frutaActualizada = await _frutaService
                    .UpdateTaxonomicDetailsAsync(fruta_id, unaInformacionTaxonomica);

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

        [HttpDelete("{fruta_id:string?}")]
        public async Task<IActionResult> RemoveAsync(string? fruta_id)
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

        [HttpDelete("{fruta_id:string?}/Nutricion")]
        public async Task<IActionResult> RemoveNutritionDetailsAsync(string? fruta_id)
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

        [HttpDelete("{fruta_id:string?}/Taxonomia")]
        public async Task<IActionResult> RemoveTaxonomicDetailsAsync(string? fruta_id)
        {
            try
            {
                var frutaSinClasificacion = await _frutaService
                    .RemoveTaxonomicDetailsAsync(fruta_id);

                return Ok($"La fruta {frutaSinClasificacion.Nombre} ya no tiene información taxonómica!");
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
