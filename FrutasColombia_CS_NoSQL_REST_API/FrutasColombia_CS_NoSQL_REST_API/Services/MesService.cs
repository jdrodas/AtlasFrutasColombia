﻿using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class MesService(IMesRepository mesRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IMesRepository _mesRepository = mesRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Mes>> GetAllAsync()
        {
            return await _mesRepository
                .GetAllAsync();
        }

        public async Task<Mes> GetByIdAsync(int mes_id)
        {
            Mes unMes = await _mesRepository
                .GetByIdAsync(mes_id);

            if (unMes.Id == 0)
                throw new AppValidationException($"Mes no encontrado con el id {mes_id}");

            return unMes;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(int mes_id)
        {
            Mes unMes = await _mesRepository
                .GetByIdAsync(mes_id);

            if (unMes.Id == 0)
                throw new AppValidationException($"Mes no encontrado con el id {mes_id}");

            //var frutasProducidas = await _frutaRepository
            //    .GetByLocationAsync(departamento_id, null);

            var frutasProducidas = await _frutaRepository
                .GetByMonthAsync(mes_id);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Mes {unMes.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}