using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class EpocaService(IEpocaRepository epocaRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IEpocaRepository _epocaRepository = epocaRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Epoca>> GetAllAsync()
        {
            return await _epocaRepository
                .GetAllAsync();
        }

        public async Task<Epoca> GetByIdAsync(Guid epoca_id)
        {
            Epoca unaEpoca = await _epocaRepository
                .GetByIdAsync(epoca_id);

            if (unaEpoca.Id == Guid.Empty)
                throw new AppValidationException($"Época no encontrada con el id {epoca_id}");

            return unaEpoca;
        }

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(Guid epoca_id)
        {
            Epoca unaEpoca = await _epocaRepository
                .GetByIdAsync(epoca_id);

            if (unaEpoca.Id == Guid.Empty)
                throw new AppValidationException($"Época no encontrado con el id {epoca_id}");

            var frutasProducidas = await _frutaRepository
                .GetByEpochAsync(epoca_id);

            if (!frutasProducidas.Any())
                throw new AppValidationException($"Época {unaEpoca.Nombre} no tiene frutas producidas");

            return frutasProducidas;
        }

        public async Task<Epoca> CreateAsync(Epoca unaEpoca)
        {
            var resultadoValidacion = await EvaluateEpochDetailsAsync(unaEpoca);

            if(!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos que los meses no estén asociados a otras épocas
            var totalEpocasExistentes = await _epocaRepository
                .GetTotalByMonthRangeAsync(unaEpoca.Mes_Inicio, unaEpoca.Mes_Final);

            if (totalEpocasExistentes > 0)
                throw new AppValidationException($"Ya existe una época asociada al mes de inicio {unaEpoca.Mes_Inicio} y al mes final {unaEpoca.Mes_Final}");

            //Validamos que no exista previamente una época con ese nombre
            var epocaExistente = await _epocaRepository
                .GetByNameAsync(unaEpoca.Nombre!);

            if (epocaExistente.Id != Guid.Empty)
                throw new AppValidationException($"Ya existe la época {unaEpoca.Nombre} ");

            try
            {
                bool resultadoAccion = await _epocaRepository
                    .CreateAsync(unaEpoca);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                epocaExistente = await _epocaRepository
                    .GetByNameAsync(unaEpoca.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return epocaExistente;
        }

        public async Task<Epoca> UpdateAsync(Epoca unaEpoca)
        {
            var resultadoValidacion = await EvaluateEpochDetailsAsync(unaEpoca);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Que la época exista con ese Id:            
            var epocaExistente = await _epocaRepository
                .GetByIdAsync(unaEpoca.Id);

            if (epocaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la época {unaEpoca.Nombre} para actualizar");

            //Validamos que los meses no estén asociados a otras épocas
            var totalEpocasExistentes = await _epocaRepository
                .GetTotalByMonthRangeAsync(unaEpoca.Mes_Inicio, unaEpoca.Mes_Final);

            if (totalEpocasExistentes > 0 && epocaExistente.Id != unaEpoca.Id)
                throw new AppValidationException($"Ya existe una época asociada al mes de inicio {unaEpoca.Mes_Inicio} y al mes final {unaEpoca.Mes_Final}");

            //Que el nombre nuevo de la época no exista en otra época distinta
            epocaExistente = await _epocaRepository
                .GetByNameAsync(unaEpoca.Nombre!);

            if (epocaExistente.Id != Guid.Empty && epocaExistente.Id != epocaExistente.Id)
                throw new AppValidationException($"Ya existe la época {epocaExistente.Nombre} con el Id {epocaExistente.Id}");

            try
            {
                bool resultadoAccion = await _epocaRepository
                    .UpdateAsync(unaEpoca);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                epocaExistente = await _epocaRepository
                    .GetByIdAsync(unaEpoca.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return epocaExistente;
        }

        public async Task<string> RemoveAsync(Guid epoca_id)
        {
            //Validamos que exista una época con ese Id
            Epoca unaEpoca = await _epocaRepository
                .GetByIdAsync(epoca_id);

            if (unaEpoca.Id == Guid.Empty)
                throw new AppValidationException($"Época no encontrada con el id {epoca_id}");

            //Validamos que no haya producción de frutas asociadas a la época
            var totalProduccionPorEpoca = await _epocaRepository
                .GetTotalProductionById(epoca_id);

            if(totalProduccionPorEpoca>0)
                throw new AppValidationException($"Hay producción de frutas asociada a la época {unaEpoca.Nombre}");

            string nombreFrutaEliminada = unaEpoca.Nombre!;

            try
            {
                bool resultadoAccion = await _epocaRepository
                    .RemoveAsync(epoca_id);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreFrutaEliminada;
        }

        private async Task<string> EvaluateEpochDetailsAsync(Epoca unaEpoca)
        {
            //Validamos que la época tenga nombre
            if (unaEpoca.Nombre!.Length == 0)
                return "No se puede insertar una época con nombre nulo";

            //Validamos que la época tenga un mes de inicio válido
            if (unaEpoca.Mes_Inicio == 0)
                return "No se puede insertar una época sin el mes de inicio";

            //Validamos que la época tenga un mes final válido
            if (unaEpoca.Mes_Final == 0)
                return "No se puede insertar una época sin el mes de final";

            if (unaEpoca.Mes_Inicio > unaEpoca.Mes_Final)
                return "Los meses inicial y final de la época deben delimitar un rango";

            return string.Empty;
        }
    }
}