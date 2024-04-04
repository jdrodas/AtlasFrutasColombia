using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<IEnumerable<Fruta>> GetAllAsync();

        public Task<Fruta> GetByIdAsync(int fruta_id);

        public Task<Fruta> GetByNameAsync(string fruta_nombre);

        public Task<IEnumerable<FrutaProducida>> GetByLocationAsync(string? departamento_id, string? municipio_id);

        public Task<FrutaDetallada> GetFruitDetailsByIdAsync(int fruta_id);

        public Task<List<Produccion>> GetFruitProductionDetails(int fruta_id, string? departamento_id, string? municipio_id);

        public Task<Nutricion> GetFruitNutritionDetails(int fruta_id);

        public Task<Taxonomia> GetFruitTaxonomicDetails(int fruta_id);

        public Task<bool> CreateAsync(Fruta unaFruta);

        public Task<bool> UpdateAsync(Fruta unaFruta);

        public Task<bool> RemoveAsync(int fruta_id);
    }
}