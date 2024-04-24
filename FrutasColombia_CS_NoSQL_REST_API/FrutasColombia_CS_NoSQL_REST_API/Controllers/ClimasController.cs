using FrutasColombia_CS_NoSQL_REST_API.Exceptions;
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
            var losClimas = await _climaService
                .GetAllAsync();

            return Ok(losClimas);
        }

        [HttpGet("{clima_id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string clima_id)
        {
            try
            {
                var unClima = await _climaService
                    .GetByIdAsync(clima_id);

                return Ok(unClima);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        //[HttpGet("{clima_id:string}/Frutas")]
        //public async Task<IActionResult> GetProducedFruitsAsync(string clima_id)
        //{
        //    try
        //    {
        //        var lasFrutasProducidas = await _climaService
        //            .GetProducedFruitsAsync(clima_id);

        //        return Ok(lasFrutasProducidas);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return NotFound(error.Message);
        //    }
        //}
    }
}
