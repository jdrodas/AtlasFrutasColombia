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
    }
}
