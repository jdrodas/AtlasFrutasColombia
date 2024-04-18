using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_NoSQL_REST_API.Controllers
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

        [HttpGet("Divisiones")]
        public async Task<IActionResult> GetAllDivisionsAsync()
        {
            try
            {
                var lasDivisiones = await _clasificacionService
                    .GetAllDivisionsAsync();

                return Ok(lasDivisiones);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Clases")]
        public async Task<IActionResult> GetAllClassesAsync()
        {
            try
            {
                var lasClases = await _clasificacionService
                    .GetAllClassesAsync();

                return Ok(lasClases);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Ordenes")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            try
            {
                var losOrdenes = await _clasificacionService
                    .GetAllOrdersAsync();

                return Ok(losOrdenes);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Familias")]
        public async Task<IActionResult> GetAllFamiliesAsync()
        {
            try
            {
                var lasFamilias = await _clasificacionService
                    .GetAllFamiliesAsync();

                return Ok(lasFamilias);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Generos")]
        public async Task<IActionResult> GetAllGenusAsync()
        {
            try
            {
                var losGeneros = await _clasificacionService
                    .GetAllGenusAsync();

                return Ok(losGeneros);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Generos/{genero_id:Guid}")]
        public async Task<IActionResult> GetGenusByIdAsync(Guid genero_id)
        {
            try
            {
                var unGenero = await _clasificacionService
                    .GetGenusByIdAsync(genero_id);

                return Ok(unGenero);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("Generos/{genero_id:Guid}/Frutas")]
        public async Task<IActionResult> GetFruitsByGenusIdAsync(Guid genero_id)
        {
            try
            {
                var unGenero = await _clasificacionService
                    .GetFruitsByGenusIdAsync(genero_id);

                return Ok(unGenero);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
