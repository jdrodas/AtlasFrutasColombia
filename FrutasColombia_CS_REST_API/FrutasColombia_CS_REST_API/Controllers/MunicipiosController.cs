﻿using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipiosController(MunicipioService municipioService) : Controller
    {
        private readonly MunicipioService _municipioService = municipioService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losMunicipios = await _municipioService
                .GetAllAsync();

            return Ok(losMunicipios);
        }

        [HttpGet("{municipio_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid municipio_id)
        {
            try
            {
                var unMunicipio = await _municipioService
                    .GetByIdAsync(municipio_id);

                return Ok(unMunicipio);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{municipio_id:Guid}/Frutas")]
        public async Task<IActionResult> GetProducedFruitsAsync(Guid municipio_id)
        {
            try
            {
                var lasFrutasProducidas = await _municipioService
                    .GetProducedFruitsAsync(municipio_id);

                return Ok(lasFrutasProducidas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
