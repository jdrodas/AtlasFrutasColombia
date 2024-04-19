using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesesController(MesService mesService) : Controller
    {
        private readonly MesService _mesService = mesService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losMeses = await _mesService
                .GetAllAsync();

            return Ok(losMeses);
        }

        [HttpGet("{mes_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int mes_id)
        {
            try
            {
                var unaFruta = await _mesService
                    .GetByIdAsync(mes_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{mes_id:int}/Produccion")]
        public async Task<IActionResult> GetProducedFruitsAsync(int mes_id)
        {
            try
            {
                var lasFrutasProducidas = await _mesService
                    .GetProducedFruitsAsync(mes_id);

                return Ok(lasFrutasProducidas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
