using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<IEnumerable<Fruta>> GetAllAsync();

        public Task<Fruta> GetByIdAsync(string? fruta_id);

        public Task<Fruta> GetByNameAsync(string fruta_nombre);

        public Task<IEnumerable<FrutaProducida>> GetByLocationAsync(string? departamento_id, string? municipio_id);

        public Task<IEnumerable<FrutaProducida>> GetByClimateAsync(string? clima_id);

        public Task<IEnumerable<FrutaProducida>> GetByMonthAsync(int mes_id);

        public Task<FrutaDetallada> GetDetailsByIdAsync(string? fruta_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, string? departamento_id, string? municipio_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, string? clima_id);

        public Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, int mes_id);

        public Task<Nutricion> GetNutritionDetailsAsync(string? fruta_id);

        public Task<FrutaNutritiva> GetNutritiousFruitByIdAsync(string? fruta_id);

        public Task<FrutaClasificada> GetClassifiedFruitByIdAsync(string? fruta_id);

        public Task<Taxonomia> GetTaxonomicDetailsAsync(string? fruta_id);

        public Task<IEnumerable<FrutaClasificada>> GetByGenusIdAsync(string? genero_id);

        public Task<bool> CreateAsync(Fruta unaFruta);

        public Task<bool> CreateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional);

        public Task<bool> CreateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica);

        public Task<bool> UpdateAsync(Fruta unaFruta);

        public Task<bool> UpdateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional);

        public Task<bool> UpdateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica);

        public Task<bool> RemoveAsync(string? fruta_id);

        public Task<bool> RemoveNutritionDetailsAsync(string? fruta_id);

        public Task<bool> RemoveTaxonomicDetailsAsync(string? fruta_id);

    }
}