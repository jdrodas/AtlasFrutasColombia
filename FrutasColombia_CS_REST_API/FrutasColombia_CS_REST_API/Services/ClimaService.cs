using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class ClimaService(IClimaRepository climaRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IClimaRepository _climaRepository = climaRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Clima>> GetAllAsync()
        {
            return await _climaRepository
                .GetAllAsync();
        }

        public async Task<Clima> GetByIdAsync(int clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (unClima.Id == 0)
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            return unClima;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(int clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (unClima.Id == 0)
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            //var frutasProducidas = await _frutaRepository
            //    .GetByLocationAsync(departamento_id, null);

            var frutasProducidas = await _frutaRepository
                .GetByClimateAsync(clima_id);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Clima {unClima.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}
