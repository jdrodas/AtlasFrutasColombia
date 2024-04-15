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

        [HttpGet("{departamento_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid departamento_id)
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

        [HttpGet("{departamento_id:Guid}/Municipios")]
        public async Task<IActionResult> GetAssociatedMunicipalityAsync(Guid departamento_id)
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

        [HttpGet("{departamento_id:Guid}/Frutas")]
        public async Task<IActionResult> GetProducedFruitsAsync(Guid departamento_id)
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
