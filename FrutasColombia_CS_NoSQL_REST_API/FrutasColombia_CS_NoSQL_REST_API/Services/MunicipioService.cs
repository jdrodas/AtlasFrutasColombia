using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class MunicipioService(IMunicipioRepository municipioRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IMunicipioRepository _municipioRepository = municipioRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Municipio>> GetAllAsync()
        {
            return await _municipioRepository
                .GetAllAsync();
        }

        public async Task<Municipio> GetByIdAsync(string? municipio_id)
        {
            Municipio unMunicipio = await _municipioRepository
                .GetByIdAsync(municipio_id);

            if (unMunicipio.Id == string?.Empty)
                throw new AppValidationException($"Municipio no encontrado con el id {municipio_id}");

            return unMunicipio;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(string? municipio_id)
        {
            Municipio unMunicipio = await _municipioRepository
                .GetByIdAsync(municipio_id);

            if (unMunicipio.Id == string?.Empty)
                throw new AppValidationException($"Municipio no encontrado con el id {municipio_id}");

            var frutasProducidas = await _frutaRepository
                .GetByLocationAsync(string?.Empty, municipio_id);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Departamento {unMunicipio.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}
