namespace FrutasColombia_CS_PoC_Consola
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("PoC - Atlas de Frutas de Colombia \n");

            Console.WriteLine("Ejecutando PoC en PostgreSQL...");
            PoC_Pgsql.Ejecuta_PoC();
        }
    }
}