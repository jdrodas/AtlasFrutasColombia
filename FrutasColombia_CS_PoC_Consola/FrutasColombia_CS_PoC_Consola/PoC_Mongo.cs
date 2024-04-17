using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrutasColombia_CS_PoC_Consola
{
    public class PoC_Mongo
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosMongo.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            //R del CRUD - Lectura de registros existentes - SELECT
            VisualizaNombresFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaFrutas();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

        }



        //    //C del CRUD - Creación de un nuevo registro - INSERT
        //    Fruta nuevaFruta = new()
        //    {
        //        Nombre = "Chontaduro",
        //        Url_Wikipedia = "https://es.wikipedia.org/wiki/Bactris_gasipaes",
        //        Url_Imagen = "https://en.wikipedia.org/wiki/Bactris_gasipaes#/media/File:Pupunha_(Bactris_gasipaes)_7.jpg"
        //    };

        //    Console.WriteLine($"\nRegistro de nueva fruta: {nuevaFruta.Nombre}:");

        //    bool resultadoInsercion = AccesoDatosMongo.InsertaFruta(nuevaFruta);

        //    if (resultadoInsercion == false)
        //        Console.WriteLine($"Inserción fallida para la fruta {nuevaFruta.Nombre}");
        //    else
        //    {
        //        Console.WriteLine($"Inserción exitosa! Este fue la fruta registrada");

        //        //Obtenemos la fruta por nombre
        //        nuevaFruta = AccesoDatosMongo.ObtieneFruta(nuevaFruta.Nombre);
        //        Console.WriteLine($"Id: {nuevaFruta.Id}, Nombre: {nuevaFruta.Nombre}");
        //    }

        //    VisualizaFrutas();

        //    Console.WriteLine("\nPresiona una tecla para continuar...");
        //    Console.ReadKey();

        //    //U del CRUD - Actualización de un nuevo registro - UPDATE
        //    Fruta frutaActualizada = AccesoDatosMongo.ObtieneFruta("Mango");

        //    frutaActualizada.Nombre = "Manguito Biche";
        //    Console.WriteLine($"\n\nActualizando la fruta No. {frutaActualizada.Id} " +
        //        $"al nuevo nombre de {frutaActualizada.Nombre}...");

        //    bool resultadoActualizacion = AccesoDatosMongo.ActualizaFruta(frutaActualizada);

        //    if (resultadoActualizacion == false)
        //        Console.WriteLine($"Actualización fallida para la fruta {frutaActualizada.Nombre}");
        //    else
        //    {
        //        Console.WriteLine($"Actualización exitosa! Este fue la fruta actualizada");

        //        //Obtenemos la fruta por Id
        //        Fruta unaFruta = AccesoDatosMongo.ObtieneFruta(frutaActualizada.Id!);
        //        Console.WriteLine($"Id: {unaFruta.Id}, Nombre: {unaFruta.Nombre}");
        //    }

        //    VisualizaFrutas();

        //    Console.WriteLine("\nPresiona una tecla para continuar...");
        //    Console.ReadKey();

        //    //Devolvemos la fruta a su valor orignal
        //    frutaActualizada.Nombre = "Mango";
        //    Console.WriteLine($"Devolviendo el nombre original a la fruta: {frutaActualizada.Nombre}");

        //    AccesoDatosMongo.ActualizaFruta(frutaActualizada);

        //    VisualizaFrutas();

        //    Console.WriteLine("\nPresiona una tecla para continuar...");
        //    Console.ReadKey();


        //    //D del CRUD - Borrado de una fruta existente - DELETE
        //    nuevaFruta = AccesoDatosMongo.ObtieneFruta(nuevaFruta.Nombre!);
        //    Console.WriteLine($"\n\nBorrando la fruta {nuevaFruta.Nombre} ...");

        //    bool resultadoEliminacion = AccesoDatosMongo.EliminaFruta(nuevaFruta, out string mensajeEliminacion);

        //    if (resultadoEliminacion == false)
        //        Console.WriteLine(mensajeEliminacion);
        //    else
        //    {
        //        Console.WriteLine($"Eliminación exitosa! la fruta {nuevaFruta.Nombre} fue eliminada");
        //        VisualizaFrutas();
        //    }
        //}

        /// <summary>
        /// Visualiza la lista de nombres de frutas registrados en la DB
        /// </summary>
        public static void VisualizaNombresFrutas()
        {
            Console.WriteLine($"Nombres de frutas registradas en la DB:");
            List<string> losNombresFrutas = AccesoDatosMongo.ObtieneNombresFrutas();

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
            List<Fruta> lasFrutas = AccesoDatosMongo.ObtieneListaFrutas();

            Console.WriteLine("\n\nLas frutas con sus propiedades básicas son:");

            foreach (Fruta unaFruta in lasFrutas)
            {
                Console.WriteLine($"Id: {unaFruta.ObjectId}\tNombre: {unaFruta.Nombre}");
                Console.WriteLine($"Enlace Wikipedia: {unaFruta.Url_Wikipedia}");
                Console.WriteLine($"Enlace Imagen: {unaFruta.Url_Imagen}\n");
            }
        }
    }
}