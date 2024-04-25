using FrutasColombia_CS_NoSQL_REST_API.Exceptions;
using FrutasColombia_CS_NoSQL_REST_API.Models;
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
                var epocaActualizada = await _climaService
                    .UpdateAsync(unClima);

                return Ok(epocaActualizada);
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

        [HttpDelete("{clima_id:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string clima_id)
        {
            try
            {
                var nombreClimaBorrado = await _climaService
                    .RemoveAsync(clima_id);

                return Ok($"El clima {nombreClimaBorrado} fue eliminado correctamente!");
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
