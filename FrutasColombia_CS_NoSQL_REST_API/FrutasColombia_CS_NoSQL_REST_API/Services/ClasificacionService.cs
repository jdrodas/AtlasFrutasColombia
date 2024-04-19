using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class ClasificacionService(IClasificacionRepository clasificacionRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IClasificacionRepository _clasificacionRepository = clasificacionRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Clasificacion>> GetAllAsync()
        {
            return await _clasificacionRepository
                .GetAllAsync();
        }

        public async Task<IEnumerable<Division>> GetAllDivisionsAsync()
        {
            return await _clasificacionRepository
                .GetAllDivisionsAsync();
        }

        public async Task<IEnumerable<Clase>> GetAllClassesAsync()
        {
            return await _clasificacionRepository
                .GetAllClassesAsync();
        }

        public async Task<IEnumerable<Orden>> GetAllOrdersAsync()
        {
            return await _clasificacionRepository
                .GetAllOrdersAsync();
        }

        public async Task<IEnumerable<Familia>> GetAllFamiliesAsync()
        {
            return await _clasificacionRepository
                .GetAllFamiliesAsync();
        }

        public async Task<IEnumerable<Genero>> GetAllGenusAsync()
        {
            return await _clasificacionRepository
                .GetAllGenusAsync();
        }

        public async Task<GeneroDetallado> GetGenusByIdAsync(string? genero_id)
        {
            Genero unGenero = await _clasificacionRepository
                .GetGenusByIdAsync(genero_id);

            if (unGenero.Id == string?.Empty)
                throw new AppValidationException($"Género no encontrad0 con el id {genero_id}");

            GeneroDetallado unGeneroDetallado = await _clasificacionRepository
                .GetGenusDetailsByIdAsync(genero_id);

            return unGeneroDetallado;
        }

        public async Task<IEnumerable<FrutaClasificada>> GetFruitsByGenusIdAsync(string? genero_id)
        {
            Genero unGenero = await _clasificacionRepository
                .GetGenusByIdAsync(genero_id);

            if (unGenero.Id == string?.Empty)
                throw new AppValidationException($"Género no encontrad0 con el id {genero_id}");

            var frutasClasificadas = await _frutaRepository
                .GetByGenusIdAsync(genero_id);

            if (!frutasClasificadas.Any())
                throw new AppValidationException($"Género {unGenero.Nombre} no tiene frutas producidas");

            return frutasClasificadas;
        }

        public async Task<Taxonomia> CreateAsync(Taxonomia unaClasificacion)
        {
            // Validaciones que los campos no sean nulos
            if (string.IsNullOrEmpty(unaClasificacion.Reino))
                throw new AppValidationException("No se puede insertar un reino con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Division))
                throw new AppValidationException("No se puede insertar una división con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Orden))
                throw new AppValidationException("No se puede insertar un orden con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Clase))
                throw new AppValidationException("No se puede insertar una clase con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Familia))
                throw new AppValidationException("No se puede insertar una familia con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Genero))
                throw new AppValidationException("No se puede insertar un género con nombre nulo");

            if (string.IsNullOrEmpty(unaClasificacion.Especie))
                throw new AppValidationException("No se puede insertar una especie con nombre nulo");

            try
            {
                bool resultadoAccion = await _clasificacionRepository
                    .CreateAsync(unaClasificacion);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return unaClasificacion;
        }

        public async Task<string> RemoveAsync(Taxonomia unaClasificacion)
        {
            try
            {
                bool resultadoAccion = await _clasificacionRepository
                    .RemoveAsync(unaClasificacion);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return unaClasificacion.Especie!;
        }
    }
}