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
            //Validamos que la época tenga nombre
            if (unaEpoca.Nombre!.Length == 0)
                throw new AppValidationException("No se puede insertar una época con nombre nulo");

            //Validamos que la época tenga un mes de inicio válido
            if (unaEpoca.Mes_Inicio == 0)
                throw new AppValidationException("No se puede insertar una época sin el mes de inicio");

            //Validamos que la fruta tenga url_imagen
            if (unaEpoca.Mes_Final == 0)
                throw new AppValidationException("No se puede insertar una época sin el mes de final");

            if(unaEpoca.Mes_Inicio > unaEpoca.Mes_Final)
                throw new AppValidationException("Los meses inicial y final de la época deben delimitar un rango");

            //Validamos que no exista previamente una fruta con ese nombre
            var frutaExistente = await _epocaRepository
                .GetByNameAsync(unaEpoca.Nombre);

            if (frutaExistente.Id != Guid.Empty)
                throw new AppValidationException($"Ya existe la época {unaEpoca.Nombre} ");

            try
            {
                bool resultadoAccion = await _epocaRepository
                    .CreateAsync(unaEpoca);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaExistente = await _epocaRepository
                    .GetByNameAsync(unaEpoca.Nombre);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaExistente;
        }
    }
}