namespace FrutasColombia_CS_PoC_Consola
{
    public class Fruta
    {
        public Guid Id { get; set; }
        public string? Nombre { get; set; } = String.Empty;
        public string? Url_Wikipedia { get; set; } = String.Empty;
        public string? Url_Imagen { get; set; } = String.Empty;
    }
}