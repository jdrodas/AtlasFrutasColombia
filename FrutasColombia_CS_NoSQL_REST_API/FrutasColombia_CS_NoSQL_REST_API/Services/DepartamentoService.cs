﻿using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class DepartamentoService(IDepartamentoRepository departamentoRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IDepartamentoRepository _departamentoRepository = departamentoRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await _departamentoRepository
                .GetAllAsync();
        }

        public async Task<Departamento> GetByIdAsync(string? departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == string?.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            return unDepartamento;
        }

        public async Task<IEnumerable<Municipio>> GetAssociatedMunicipalityAsync(string? departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == string?.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            var municipiosAsociados = await _departamentoRepository
                .GetAssociatedMunicipalityAsync(unDepartamento.Id);

            if (!municipiosAsociados.Any())
                throw new AppValidationException($"Departamento {unDepartamento.Nombre} no tiene municipios asociados");

            return municipiosAsociados;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(string? departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == string?.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            var frutasProducidas = await _frutaRepository
                .GetByLocationAsync(departamento_id, string?.Empty);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Departamento {unDepartamento.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}
