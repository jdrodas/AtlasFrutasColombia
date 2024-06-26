﻿using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IClasificacionRepository
    {
        public Task<IEnumerable<Taxonomia>> GetAllAsync();

        //public Task<IEnumerable<Division>> GetAllDivisionsAsync();

        //public Task<IEnumerable<Clase>> GetAllClassesAsync();

        //public Task<IEnumerable<Orden>> GetAllOrdersAsync();

        //public Task<IEnumerable<Familia>> GetAllFamiliesAsync();

        //public Task<IEnumerable<Genero>> GetAllGenusAsync();

        //public Task<Genero> GetGenusByIdAsync(string? genero_id);

        //public Task<GeneroDetallado> GetGenusDetailsByIdAsync(string? genero_id);

        //public Task<List<Especie>> GetAssociatedSpeciesToGenusById(string? genero_id);

        //public Task<string?> GetTaxonomicElementIdAsync(string tipo_elemento, string nombre_elemento);

        //public Task<bool> CreateAsync(Taxonomia unaClasificacion);

        //public Task<bool> RemoveAsync(Taxonomia unaClasificacion);
    }
}
