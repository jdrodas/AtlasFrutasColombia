﻿using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Services
{
    public class FrutaService(IFrutaRepository frutaRepository)
    {
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

        public async Task<IEnumerable<Fruta>> GetAllAsync()
        {
            return await _frutaRepository
                .GetAllAsync();
        }

        public async Task<FrutaDetallada> GetFruitDetailsByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id == Guid.Empty)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            FrutaDetallada unaFrutaDetallada = await _frutaRepository
                .GetDetailsByIdAsync(fruta_id);

            return unaFrutaDetallada;
        }

        public async Task<FrutaNutritiva> GetNutritiousFruitByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id == Guid.Empty)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            FrutaNutritiva unaFrutaNutritiva = await _frutaRepository
                .GetNutritiousFruitByIdAsync(fruta_id);

            return unaFrutaNutritiva;
        }

        public async Task<FrutaClasificada> GetClassifiedFruitByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id == Guid.Empty)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            FrutaClasificada unaFrutaClasificada = await _frutaRepository
                .GetClassifiedFruitByIdAsync(fruta_id);

            return unaFrutaClasificada;
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

            if (frutaExistente.Id != Guid.Empty)
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

            return frutaExistente;
        }

        public async Task<FrutaNutritiva> CreateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
        {
            //Validamos que exista la fruta previamente
            var frutaNutritivaExistente = await _frutaRepository
                .GetNutritiousFruitByIdAsync(fruta_id);

            if (frutaNutritivaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .CreateNutritionDetailsAsync(fruta_id, unaInformacionNutricional);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaNutritivaExistente = await _frutaRepository
                    .GetNutritiousFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaNutritivaExistente;
        }

        public async Task<FrutaClasificada> CreateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            //Validamos que exista la fruta previamente
            var frutaClasificadaExistente = await _frutaRepository
                .GetClassifiedFruitByIdAsync(fruta_id);

            if (frutaClasificadaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            //Aqui evaluamos que la información taxonómica no venga con valores nulos 
            string resultadoValidacionTaxonomia = EvaluaInformacionTaxonomica(unaInformacionTaxonomica);
            if (!string.IsNullOrEmpty(resultadoValidacionTaxonomia))
                throw new AppValidationException($"No se puede registrar información taxonómica con {resultadoValidacionTaxonomia} nulo. ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .CreateTaxonomicDetailsAsync(fruta_id, unaInformacionTaxonomica);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaClasificadaExistente = await _frutaRepository
                    .GetClassifiedFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaClasificadaExistente;
        }

        public async Task<Fruta> UpdateAsync(Fruta unaFruta)
        {
            //Validamos que la fruta tenga Id
            if (unaFruta.Id == Guid.Empty)
                throw new AppValidationException("El Id de fruta se requiere especificar para realizar actualización");

            //Validamos que la fruta tenga nombre
            if (unaFruta.Nombre!.Length == 0)
                throw new AppValidationException("No se puede actualizar una fruta con nombre nulo");

            //Validamos que la fruta tenga url_wikipedia
            if (unaFruta.Url_Wikipedia!.Length == 0)
                throw new AppValidationException("No se puede actualizar una fruta con Url de Wikipedia nulo");

            //Validamos que la fruta tenga url_imagen
            if (unaFruta.Url_Imagen!.Length == 0)
                throw new AppValidationException("No se puede actualizar una fruta con Url de la imagen nulo");

            //Que la fruta exista con ese Id:            
            var frutaExistente = await _frutaRepository
                .GetByIdAsync(unaFruta.Id);

            if (frutaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta {unaFruta.Nombre} para actualizar");

            //Que el nombre nuevo de la fruta no exista en otra fruta distinta
            frutaExistente = await _frutaRepository
                .GetByNameAsync(unaFruta.Nombre!);

            if (frutaExistente.Id != Guid.Empty && frutaExistente.Id != unaFruta.Id)
                throw new AppValidationException($"Ya existe la fruta {frutaExistente.Nombre} con el Id {frutaExistente.Id}");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .UpdateAsync(unaFruta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaExistente = await _frutaRepository
                    .GetByIdAsync(unaFruta.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaExistente;
        }

        public async Task<FrutaNutritiva> UpdateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
        {
            //Validamos que exista la fruta previamente
            var frutaNutritivaExistente = await _frutaRepository
                .GetNutritiousFruitByIdAsync(fruta_id);

            if (frutaNutritivaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .UpdateNutritionDetailsAsync(fruta_id, unaInformacionNutricional);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaNutritivaExistente = await _frutaRepository
                    .GetNutritiousFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaNutritivaExistente;
        }

        public async Task<FrutaClasificada> UpdateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            //Validamos que exista la fruta previamente
            var frutaClasificadaExistente = await _frutaRepository
                .GetClassifiedFruitByIdAsync(fruta_id);

            if (frutaClasificadaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .UpdateTaxonomicDetailsAsync(fruta_id, unaInformacionTaxonomica);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaClasificadaExistente = await _frutaRepository
                    .GetClassifiedFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaClasificadaExistente;
        }

        public async Task<string> RemoveAsync(Guid fruta_id)
        {
            //Validamos que exista una fruta con ese Id
            Fruta unaFruta = await _frutaRepository
                .GetByIdAsync(fruta_id);

            if (unaFruta.Id == Guid.Empty)
                throw new AppValidationException($"Fruta no encontrada con el id {fruta_id}");

            string nombreFrutaEliminada = unaFruta.Nombre!;

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .RemoveAsync(fruta_id);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreFrutaEliminada;
        }

        public async Task<FrutaNutritiva> RemoveNutritionDetailsAsync(Guid fruta_id)
        {
            //Validamos que exista la fruta previamente
            var frutaNutritivaExistente = await _frutaRepository
                .GetNutritiousFruitByIdAsync(fruta_id);

            if (frutaNutritivaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .RemoveNutritionDetailsAsync(fruta_id);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaNutritivaExistente = await _frutaRepository
                    .GetNutritiousFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaNutritivaExistente;
        }

        public async Task<FrutaClasificada> RemoveTaxonomicDetailsAsync(Guid fruta_id)
        {
            //Validamos que exista la fruta previamente
            var frutaClasificadaExistente = await _frutaRepository
                .GetClassifiedFruitByIdAsync(fruta_id);

            if (frutaClasificadaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe la fruta con el ID {fruta_id} ");

            try
            {
                bool resultadoAccion = await _frutaRepository
                    .RemoveTaxonomicDetailsAsync(fruta_id);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                frutaClasificadaExistente = await _frutaRepository
                    .GetClassifiedFruitByIdAsync(fruta_id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return frutaClasificadaExistente;
        }

        private string EvaluaInformacionTaxonomica(Taxonomia unaInformacionTaxonomica)
        {
            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Reino))
                return "Reino";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Division))
                return "Division";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Orden))
                return "Orden";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Clase))
                return "Clase";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Familia))
                return "Familia";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Genero))
                return "Genero";

            if (string.IsNullOrEmpty(unaInformacionTaxonomica.Especie))
                return "Especie";

            return string.Empty;
        }
    }
}