using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<IEnumerable<Fruta>> GetAllAsync();

        public Task<Fruta> GetByIdAsync(Guid fruta_id);

        public Task<Fruta> GetByNameAsync(string fruta_nombre);

        public Task<IEnumerable<FrutaProducida>> GetByLocationAsync(Guid departamento_id, Guid municipio_id);

        public Task<IEnumerable<FrutaProducida>> GetByClimateAsync(Guid clima_id);

        public Task<FrutaDetallada> GetDetailsByIdAsync(Guid fruta_id);

        public Task<List<Produccion>> GetProductionDetails(Guid fruta_id, Guid departamento_id, Guid municipio_id);

        public Task<List<Produccion>> GetProductionDetails(Guid fruta_id, Guid clima_id);

        public Task<Nutricion> GetNutritionDetails(Guid fruta_id);

        public Task<Taxonomia> GetTaxonomicDetails(Guid fruta_id);

        public Task<IEnumerable<FrutaClasificada>> GetByGenusIdAsync(Guid genero_id);

        public Task<bool> CreateAsync(Fruta unaFruta);

        public Task<bool> UpdateAsync(Fruta unaFruta);

        public Task<bool> RemoveAsync(Guid fruta_id);
    }
}