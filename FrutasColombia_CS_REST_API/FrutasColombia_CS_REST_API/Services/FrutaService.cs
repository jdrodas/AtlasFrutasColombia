using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class FrutaService(IFrutaRepository frutaRepository)
    {
        private readonly IFrutaRepository _frutaRepository  = frutaRepository;

        public async Task<IEnumerable<Fruta>> GetAllAsync()
        {
            return await _frutaRepository
                .GetAllAsync();
        }
    }
}
