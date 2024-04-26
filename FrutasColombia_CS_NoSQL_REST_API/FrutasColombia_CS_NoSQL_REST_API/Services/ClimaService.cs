using FrutasColombia_CS_NoSQL_REST_API.Exceptions;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class ClimaService(IClimaRepository climaRepository)
    //public class ClimaService(IClimaRepository climaRepository,
    //                                    IFrutaRepository frutaRepository)
    {
        private readonly IClimaRepository _climaRepository = climaRepository;
        //private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Clima>> GetAllAsync()
        {
            return await _climaRepository
                .GetAllAsync();
        }

        public async Task<Clima> GetByIdAsync(string clima_id)
        {
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (string.IsNullOrEmpty(unClima.Id))
                throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

            return unClima;
        }

        //public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(string clima_id)
        //{
        //    Clima unClima = await _climaRepository
        //        .GetByIdAsync(clima_id);

        //    if (string.IsNullOrEmpty(unClima.Id))
        //        throw new AppValidationException($"Clima no encontrado con el id {clima_id}");

        //    var frutasProducidas = await _frutaRepository
        //        .GetByClimateAsync(clima_id);

        //    if (!frutasProducidas.Any())
        //        throw new AppValidationException($"Clima {unClima.Nombre} no tiene frutas producidas");

        //    return frutasProducidas;
        //}

        public async Task<Clima> CreateAsync(Clima unClima)
        {
            string resultadoValidacion = EvaluateClimateDetailsAsync(unClima);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos que las altitudesno estén asociadas a otros climas
            var totalClimasExistentes = await _climaRepository
                .GetTotalByAltitudeRangeAsync(unClima.Altitud_Minima, unClima.Altitud_Maxima);

            if (totalClimasExistentes > 0)
                throw new AppValidationException($"Ya existe un clima asociado las altitudes {unClima.Altitud_Minima} y  {unClima.Altitud_Maxima}");

            //Validamos que no exista previamente un clima con ese nombre
            var climaExistente = await _climaRepository
                .GetByNameAsync(unClima.Nombre!);

            if (!string.IsNullOrEmpty(climaExistente.Id))
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

        private static string EvaluateClimateDetailsAsync(Clima unClima)
        {
            //Validamos que el clima tenga nombre
            if (unClima.Nombre!.Length == 0)
                return "No se puede insertar una clima con nombre nulo";

            //Validamos que el clima tenga una altitud mínima válida
            if (unClima.Altitud_Minima < 0)
                return "No se puede insertar un clima con la altitud mínima fuera del rango";

            //Validamos que el clima tenga una altitud maxima válida
            if (unClima.Altitud_Maxima < 0)
                return "No se puede insertar un clima con la altitud máxima fuera del rango";

            if (unClima.Altitud_Minima > unClima.Altitud_Maxima)
                return "Las altitudes mínima y máxima del clima deben delimitar un rango";

            return string.Empty;
        }

        public async Task<Clima> UpdateAsync(Clima unClima)
        {
            string resultadoValidacion = EvaluateClimateDetailsAsync(unClima);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Que la época exista con ese Id:            
            var climaExistente = await _climaRepository
                .GetByIdAsync(unClima.Id!);

            if (string.IsNullOrEmpty(unClima.Id))
                throw new AppValidationException($"No existe el clima {unClima.Nombre} para actualizar");

            //Validamos que los meses no estén asociados a otras épocas
            var totalClimasExistentes = await _climaRepository
                .GetTotalByAltitudeRangeAsync(unClima.Altitud_Minima, unClima.Altitud_Maxima);

            if (totalClimasExistentes > 0 && climaExistente.Id != unClima.Id)
                throw new AppValidationException($"Ya existe un clima asociado a las alturas {unClima.Altitud_Minima} y al mes final {unClima.Altitud_Maxima}");

            //Que el nombre nuevo de la época no exista en otra época distinta            
            climaExistente = await _climaRepository
                .GetByNameAsync(unClima.Nombre!);

            if (!string.IsNullOrEmpty(climaExistente.Id) && climaExistente.Id != unClima.Id)
                throw new AppValidationException($"Ya existe el {climaExistente.Nombre} con el Id {climaExistente.Id}");

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


        public async Task<string> RemoveAsync(string clima_id)
        {
            //Validamos que exista una época con ese Id
            Clima unClima = await _climaRepository
                .GetByIdAsync(clima_id);

            if (string.IsNullOrEmpty(unClima.Id))
                throw new AppValidationException($"Época no encontrada con el id {clima_id}");

            //Validamos que no haya producción de frutas asociadas a la época
            //TODO Implementar validación de producción por época antes de borrarla
            //var totalProduccionPorEpoca = await _epocaRepository
            //    .GetTotalProductionById(epoca_id);

            //if (totalProduccionPorEpoca > 0)
            //    throw new AppValidationException($"Hay producción de frutas asociada a la época {unaEpoca.Nombre}");

            string nombreClimaBorrado = unClima.Nombre!;

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

            return nombreClimaBorrado;
        }
    }
}
