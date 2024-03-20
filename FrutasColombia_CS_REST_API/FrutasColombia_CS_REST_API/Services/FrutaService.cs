using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using FrutasColombia_CS_REST_API.Repositories;

namespace FrutasColombia_CS_REST_API.Services
{
    public class FrutaService(IFrutaRepository frutaRepository)
    {
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Fruta>> GetAllAsync()
        {
            return await _frutaRepository
                .GetAllAsync();
        }

        public async Task<Fruta> GetByIdAsync(int fruta_id)
        {
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id ==0)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            return unaFruta;
        }

        public async Task<Fruta> CreateAsync(Fruta unaFruta)
        {
            //Validamos que la fruta tenga nombre
            if (unaFruta.Nombre!.Length == 0)
                throw new AppValidationException("No se puede insertar una fruta con nombre nulo");

            //Validamos que la fruta tenga url_wikipedia
            if (unaFruta.Url_Wikipedia!.Length == 0)
                throw new AppValidationException("No se puede insertar una fruta con Url de Wikipedia nulo");

            //Validamos que la fruta tenga url_imagen
            if (unaFruta.Url_Imagen!.Length == 0)
                throw new AppValidationException("No se puede insertar una fruta con Url de la imagen nulo");

            //Validamos que no exista previamente una fruta con ese nombre
            var frutaExistente = await _frutaRepository
                .GetByNameAsync(unaFruta.Nombre);

            if (frutaExistente.Id != 0)
                throw new AppValidationException($"Ya existe la fruta {unaFruta.Nombre} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .CreateAsync(unaFruta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaExistente = await _frutaRepository
                    .GetByNameAsync(unaFruta.Nombre);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return (frutaExistente);
        }
    }
}
