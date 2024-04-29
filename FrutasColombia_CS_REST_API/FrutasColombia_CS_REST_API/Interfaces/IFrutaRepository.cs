using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<List<Fruta>> GetAllAsync();

        public Task<Fruta> GetByIdAsync(Guid fruta_id);

        public Task<Fruta> GetByNameAsync(string fruta_nombre);

        public Task<List<FrutaProducida>> GetProducedByLocationAsync(Guid departamento_id, Guid municipio_id);

        public Task<List<FrutaProducida>> GetProducedByClimateAsync(Guid clima_id);

        public Task<List<FrutaProducida>> GetProducedByEpochAsync(Guid epoca_id);

        public Task<List<FrutaProducida>> GetProducedByMonthAsync(int mes_id);

        public Task<FrutaDetallada> GetDetailsByIdAsync(Guid fruta_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, Guid departamento_id, Guid municipio_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, Guid clima_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, int mes_id);

        public Task<Nutricion> GetNutritionDetailsAsync(Guid fruta_id);

        public Task<FrutaNutritiva> GetNutritiousFruitByIdAsync(Guid fruta_id);

        public Task<FrutaClasificada> GetClassifiedFruitByIdAsync(Guid fruta_id);

        public Task<FrutaProducida> GetProducedFruitByIdAsync(Guid fruta_id);

        public Task<Taxonomia> GetTaxonomicDetailsAsync(Guid fruta_id);

        public Task<List<FrutaClasificada>> GetClassifiedByGenusIdAsync(Guid genero_id);

        public Task<bool> CreateAsync(Fruta unaFruta);

        public Task<bool> CreateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional);

        public Task<bool> CreateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica);

        public Task<bool> CreateProductionDetailsAsync(Guid fruta_id, Produccion unaInformacionProductiva);

        public Task<bool> UpdateAsync(Fruta unaFruta);

        public Task<bool> UpdateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional);

        public Task<bool> UpdateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica);

        public Task<bool> RemoveAsync(Guid fruta_id);

        public Task<bool> RemoveNutritionDetailsAsync(Guid fruta_id);

        public Task<bool> RemoveTaxonomicDetailsAsync(Guid fruta_id);

        public Task<bool> RemoveProductionDetailsAsync(Guid fruta_id, Produccion unaInformacionProductiva);

    }
}