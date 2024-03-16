using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using FrutasColombia_CS_REST_API.Repositories;

namespace FrutasColombia_CS_REST_API.Services
{
    public class ResumenService(IResumenRepository resumenRepository)
    {
        private readonly IResumenRepository _resumenRepository = resumenRepository;

        public async Task<Resumen> GetAllAsync()
        {
            return await _resumenRepository
                .GetAllAsync();
        }
    }
}