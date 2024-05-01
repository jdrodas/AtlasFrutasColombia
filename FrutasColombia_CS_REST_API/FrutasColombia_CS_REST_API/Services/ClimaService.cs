using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class ClimaService(IClimaRepository climaRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IClimaRepository _climaRepository = climaRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<List<Clima>> GetAllAsync()
        {
            return await _climaRepository
                .GetAllAsync();
        }

        public async Task<Clima> GetByIdAsync(Guid clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (unClima.Id == Guid.Empty)
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            return unClima;
        }

        public async Task<List<FrutaProducida>> GetProducedFruitsAsync(Guid clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (unClima.Id == Guid.Empty)
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            var frutasProducidas = await _frutaRepository
                .GetProducedByClimateAsync(clima_id);

            if (frutasProducidas.Count == 0)
                throw new AppValidationException($"Clima {unClima.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }

        public async Task<Clima> CreateAsync(Clima unClima)
        {
            string resultadoValidacion = EvaluateClimateDetailsAsync(unClima);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var validacionAltitudesClima = await _climaRepository
                .ValidateAltitudeRangeAsync(unClima);

            if (!string.IsNullOrEmpty(validacionAltitudesClima))
                throw new AppValidationException(validacionAltitudesClima);

            var totalRangosAlturas = await _climaRepository
                .GetTotalByAltitudeRangeAsync(unClima.Altitud_Minima, unClima.Altitud_Maxima);

            if (totalRangosAlturas > 0)
                throw new AppValidationException($"Ya existe un clima con el rango de alturas [{unClima.Altitud_Minima}:{unClima.Altitud_Maxima}]");

            var climaExistente = await _climaRepository
                .GetByNameAsync(unClima.Nombre!);

            if (climaExistente.Id != Guid.Empty)
                throw new AppValidationException($"Ya existe el clima {unClima.Nombre} ");

            try
            {
                bool resultadoAccion = await _climaRepository
                    .CreateAsync(unClima);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                climaExistente = await _climaRepository
                    .GetByNameAsync(unClima.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return climaExistente;
        }

        public async Task<Clima> UpdateAsync(Clima unClima)
        {
            string resultadoValidacion = EvaluateClimateDetailsAsync(unClima);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var climaExistente = await _climaRepository
                .GetByNameAsync(unClima.Nombre!);

            if (climaExistente.Id != unClima.Id && climaExistente.Id != Guid.Empty)
                throw new AppValidationException($"Ya existe un clima con el nombre {unClima.Nombre}");

            climaExistente = await _climaRepository
                .GetByAltitudeRangeAsync(unClima.Altitud_Minima, unClima.Altitud_Maxima);

            if (climaExistente.Id != unClima.Id)
                throw new AppValidationException($"Ya existe un clima con el rango de alturas [{unClima.Altitud_Minima}:{unClima.Altitud_Maxima}]");

            var validacionAltitudesClima = await _climaRepository
                .ValidateAltitudeRangeAsync(unClima);

            if (!string.IsNullOrEmpty(validacionAltitudesClima))
                throw new AppValidationException(validacionAltitudesClima);

            try
            {
                bool resultadoAccion = await _climaRepository
                    .UpdateAsync(unClima);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                climaExistente = await _climaRepository
                    .GetByIdAsync(unClima.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return climaExistente;
        }

        public async Task<string> RemoveAsync(Guid clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (unClima.Id == Guid.Empty)
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            var totalProduccionPorClima = await _climaRepository
                .GetTotalProductionById(clima_id);

            if (totalProduccionPorClima > 0)
                throw new AppValidationException($"No se puede borrar. Hay producción de frutas asociada al clima {unClima.Nombre}");

            string nombreFrutaEliminada = unClima.Nombre!;

            try
            {
                bool resultadoAccion = await _climaRepository
                    .RemoveAsync(clima_id);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreFrutaEliminada;
        }

        private static string EvaluateClimateDetailsAsync(Clima unClima)
        {
            if (unClima.Nombre!.Length == 0)
                return "No se puede insertar un clima con nombre nulo";

            if (unClima.Altitud_Minima < 0 || unClima.Altitud_Maxima < 0)
                return "No se puede insertar un clima con alturas negativas.";

            if (unClima.Altitud_Minima >= unClima.Altitud_Maxima)
                return "Las alturas mínima y máxima deben delimitar un rango";

            return string.Empty;
        }
    }
}
