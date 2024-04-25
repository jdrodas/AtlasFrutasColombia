using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IClasificacionRepository
    {
        public Task<List<Clasificacion>> GetAllAsync();

        public Task<List<Division>> GetAllDivisionsAsync();

        public Task<List<Clase>> GetAllClassesAsync();

        public Task<List<Orden>> GetAllOrdersAsync();

        public Task<List<Familia>> GetAllFamiliesAsync();

        public Task<List<Genero>> GetAllGenusAsync();

        public Task<Genero> GetGenusByIdAsync(Guid genero_id);

        public Task<GeneroDetallado> GetGenusDetailsByIdAsync(Guid genero_id);

        public Task<List<Especie>> GetAssociatedSpeciesToGenusById(Guid genero_id);

        public Task<Guid> GetTaxonomicElementIdAsync(string tipo_elemento, string nombre_elemento);

        public Task<bool> CreateAsync(Taxonomia unaClasificacion);

        public Task<bool> RemoveAsync(Taxonomia unaClasificacion);
    }
}
