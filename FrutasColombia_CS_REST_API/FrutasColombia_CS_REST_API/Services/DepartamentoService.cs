﻿using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class DepartamentoService(IDepartamentoRepository departamentoRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IDepartamentoRepository _departamentoRepository = departamentoRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<List<Departamento>> GetAllAsync()
        {
            return await _departamentoRepository
                .GetAllAsync();
        }

        public async Task<Departamento> GetByIdAsync(Guid departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == Guid.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            return unDepartamento;
        }

        public async Task<List<Municipio>> GetAssociatedMunicipalityAsync(Guid departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == Guid.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            var municipiosAsociados = await _departamentoRepository
                .GetAssociatedMunicipalityAsync(unDepartamento.Id);

            if (municipiosAsociados.Count == 0)
                throw new AppValidationException($"Departamento {unDepartamento.Nombre} no tiene municipios asociados");

            return municipiosAsociados;
        }

        public async Task<List<FrutaProducida>> GetProducedFruitsAsync(Guid departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (unDepartamento.Id == Guid.Empty)
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            var frutasProducidas = await _frutaRepository
                .GetProducedByLocationAsync(departamento_id, Guid.Empty);

            if (frutasProducidas.Count == 0)
                throw new AppValidationException($"Departamento {unDepartamento.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }
    }
}
