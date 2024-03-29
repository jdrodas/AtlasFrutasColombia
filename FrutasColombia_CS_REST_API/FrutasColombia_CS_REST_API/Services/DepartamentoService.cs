﻿using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class DepartamentoService(IDepartamentoRepository departamentoRepository)
    {
        private readonly IDepartamentoRepository _departamentoRepository = departamentoRepository;

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await _departamentoRepository
                .GetAllAsync();
        }

        public async Task<Departamento> GetByIdAsync(string departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (string.IsNullOrEmpty(unDepartamento.Id))
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            return unDepartamento;
        }

        public async Task<IEnumerable<Municipio>> GetAssociatedMunicipiosAsync(string departamento_id)
        {
            Departamento unDepartamento = await _departamentoRepository
                .GetByIdAsync(departamento_id);

            if (string.IsNullOrEmpty(unDepartamento.Id))
                throw new AppValidationException($"Departamento no encontrado con el id {departamento_id}");

            var municipiosAsociados = await _departamentoRepository
                .GetAssociatedMunicipiosAsync(unDepartamento.Id);

            if (!municipiosAsociados.Any())
                throw new AppValidationException($"Departamento {unDepartamento.Nombre} no tiene municipios asociados");

            return municipiosAsociados;
        }
    }
}
