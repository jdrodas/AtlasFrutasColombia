using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrutasColombia_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController(DepartamentoService departamentoService) : Controller
    {
        private readonly DepartamentoService _departamentoService = departamentoService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losDepartamentos = await _departamentoService
                .GetAllAsync();

            return Ok(losDepartamentos);
        }

        [HttpGet("{departamento_id:length(2)}")]
        public async Task<IActionResult> GetByIdAsync(string departamento_id)
        {
            try
            {
                var unDepartamento = await _departamentoService
                    .GetByIdAsync(departamento_id);

                return Ok(unDepartamento);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{departamento_id:length(2)}/Municipios")]
        public async Task<IActionResult> GetAssociatedMunicipalityAsync(string departamento_id)
        {
            try
            {
                var losMunicipiosAsociados = await _departamentoService
                    .GetAssociatedMunicipalityAsync(departamento_id);

                return Ok(losMunicipiosAsociados);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{departamento_id:length(2)}/Frutas")]
        public async Task<IActionResult> GetProducedFruitsAsync(string departamento_id)
        {
            try
            {
                var lasFrutasProducidas = await _departamentoService
                    .GetProducedFruitsAsync(departamento_id);

                return Ok(lasFrutasProducidas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

    }
}
