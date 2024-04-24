using FrutasColombia_CS_NoSQL_REST_API.Exceptions;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using FrutasColombia_CS_NoSQL_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_NoSQL_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpocasController(EpocaService epocaService) : Controller
    {
        private readonly EpocaService _epocaService = epocaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasEpocas = await _epocaService
                .GetAllAsync();

            return Ok(lasEpocas);
        }

        [HttpGet("{epoca_id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string epoca_id)
        {
            try
            {
                var unaEpoca = await _epocaService
                    .GetByIdAsync(epoca_id);

                return Ok(unaEpoca);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        //[HttpGet("{epoca_id:Guid}/Frutas")]
        //public async Task<IActionResult> GetProducedFruitsAsync(Guid epoca_id)
        //{
        //    try
        //    {
        //        var lasFrutasProducidas = await _epocaService
        //            .GetProducedFruitsAsync(epoca_id);

        //        return Ok(lasFrutasProducidas);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return NotFound(error.Message);
        //    }
        //}

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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Epoca unaEpoca)
        {
            try
            {
                var epocaActualizada = await _epocaService
                    .UpdateAsync(unaEpoca);

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

        [HttpDelete("{epoca_id:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string epoca_id)
        {
            try
            {
                var nombreEpocaBorrada = await _epocaService
                    .RemoveAsync(epoca_id);

                return Ok($"La época {nombreEpocaBorrada} fue eliminada correctamente!");
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
