using FrutasColombia_CS_REST_API.Helpers;
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
    }
}
