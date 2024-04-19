using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class EpocaService(IEpocaRepository epocaRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IEpocaRepository _epocaRepository = epocaRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Epoca>> GetAllAsync()
        {
            return await _epocaRepository
                .GetAllAsync();
        }

        public async Task<Epoca> GetByIdAsync(Guid epoca_id)
        {
            Epoca unaEpoca = await _epocaRepository
                .GetByIdAsync(epoca_id);

            if (unaEpoca.Id == Guid.Empty)
                throw new AppValidationException($"Época no encontrada con el id {epoca_id}");

            return unaEpoca;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(Guid epoca_id)
        {
            Epoca unaEpoca = await _epocaRepository
                .GetByIdAsync(epoca_id);

            if (unaEpoca.Id == Guid.Empty)
                throw new AppValidationException($"Época no encontrado con el id {epoca_id}");

            var frutasProducidas = await _frutaRepository
                .GetByEpochAsync(epoca_id);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Época {unaEpoca.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}