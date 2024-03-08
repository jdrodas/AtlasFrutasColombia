namespace FrutasColombia_CS_PoC_Consola
{
    public class PoC_Pgsql
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosPgsql.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            //R del CRUD - Lectura de registros existentes - SELECT
            VisualizaNombresFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //C del CRUD - Creación de un nuevo registro - INSERT
            Fruta nuevaFruta = new()
            {
                Nombre = "Zapote",
                Url_Wikipedia = "https://es.wikipedia.org/wiki/Matisia_cordata",
                Url_Imagen = "https://es.wikipedia.org/wiki/Matisia_cordata#/media/Archivo:Zapote_fruta.jpg"
            };

            Console.WriteLine($"\nRegistro de nueva fruta: {nuevaFruta.Nombre}:");

            bool resultadoInsercion = AccesoDatosPgsql.InsertaFruta(nuevaFruta);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para la fruta {nuevaFruta.Nombre}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue la fruta registrada");

                //Obtenemos la fruta por nombre
                nuevaFruta = AccesoDatosPgsql.ObtieneFruta(nuevaFruta.Nombre);
                Console.WriteLine($"Id: {nuevaFruta.Id}, Nombre: {nuevaFruta.Nombre}");
            }

            VisualizaFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //U del CRUD - Actualización de un nuevo registro - UPDATE
            Fruta  frutaActualizada = AccesoDatosPgsql.ObtieneFruta("Mango");

            frutaActualizada.Nombre = "Manguito Biche";
            Console.WriteLine($"\n\nActualizando la fruta No. {frutaActualizada.Id} " +
                $"al nuevo nombre de {frutaActualizada.Nombre}...");

            bool resultadoActualizacion = AccesoDatosPgsql.ActualizaFruta(frutaActualizada);

            if (resultadoActualizacion == false)
                Console.WriteLine($"Actualización fallida para la fruta {frutaActualizada.Nombre}");
            else
            {
                Console.WriteLine($"Actualización exitosa! Este fue la fruta actualizada");

                //Obtenemos la fruta por Id
                Fruta unaFruta = AccesoDatosPgsql.ObtieneFruta(frutaActualizada.Id);
                Console.WriteLine($"Id: {unaFruta.Id}, Nombre: {unaFruta.Nombre}");
            }

            VisualizaFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //Devolvemos la fruta a su valor orignal
            frutaActualizada.Nombre = "Mango";
            Console.WriteLine($"Devolviendo el nombre original a la fruta: {frutaActualizada.Nombre}");

            AccesoDatosPgsql.ActualizaFruta(frutaActualizada);

            VisualizaFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();


            //D del CRUD - Borrado de una fruta existente - DELETE
            nuevaFruta = AccesoDatosPgsql.ObtieneFruta(nuevaFruta.Nombre!);
            Console.WriteLine($"\n\nBorrando la fruta {nuevaFruta.Nombre} ...");

            bool resultadoEliminacion = AccesoDatosPgsql.EliminaFruta(nuevaFruta, out string mensajeEliminacion);

            if (resultadoEliminacion == false)
                Console.WriteLine(mensajeEliminacion);
            else
            {
                Console.WriteLine($"Eliminación exitosa! la fruta {nuevaFruta.Nombre} fue eliminada");
                VisualizaFrutas();
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();            


        }

        /// <summary>
        /// Visualiza la lista de nombres de cerveza registrados en la DB
        /// </summary>
        public static void VisualizaNombresFrutas()
        {
            Console.WriteLine($"Nombres de frutas registradas en la DB:");
            List<string> losNombresFrutas = AccesoDatosPgsql.ObtieneNombresFrutas();

            if (losNombresFrutas.Count == 0)
                Console.WriteLine("No se encontraron frutas");
            else
            {
                Console.WriteLine($"\nSe encontraron {losNombresFrutas.Count} frutas:");

                foreach (string unNombreFruta in losNombresFrutas)
                    Console.WriteLine($"- {unNombreFruta}");
            }
        }

        /// <summary>
        /// Visualiza la lista de frutas con sus propiedades básicas
        /// </summary>
        public static void VisualizaFrutas()
        {
            //Aqui demostramos la manipulación de una lista de objetos tipo Frutas
            List<Fruta> lasFrutas = AccesoDatosPgsql.ObtieneListaFrutas();

            Console.WriteLine("\n\nLas frutas con sus propiedades básicas son:");

            foreach (Fruta unaFruta in lasFrutas)
            {
                Console.WriteLine($"Id: {unaFruta.Id}\tNombre: {unaFruta.Nombre}");
                Console.WriteLine($"Enlace Wikipedia: {unaFruta.Url_Wikipedia}");
                Console.WriteLine($"Enlace Imagen: {unaFruta.Url_Imagen}\n");
            }                
        }
    }
}