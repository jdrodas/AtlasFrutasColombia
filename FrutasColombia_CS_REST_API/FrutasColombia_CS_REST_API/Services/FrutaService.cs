using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using FrutasColombia_CS_REST_API.Repositories;

namespace FrutasColombia_CS_REST_API.Services
{
    public class FrutaService(IFrutaRepository frutaRepository)
    {
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Fruta>> GetAllAsync()
        {
            return await _frutaRepository
                .GetAllAsync();
        }

        public async Task<Fruta> GetByIdAsync(int fruta_id)
        {
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id ==0)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            return unaFruta;
        }
    }
}
