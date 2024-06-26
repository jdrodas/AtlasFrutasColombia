﻿using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IMunicipioRepository
    {
        public Task<IEnumerable<Municipio>> GetAllAsync();

        public Task<Municipio> GetByIdAsync(string? municipio_id);
    }
}