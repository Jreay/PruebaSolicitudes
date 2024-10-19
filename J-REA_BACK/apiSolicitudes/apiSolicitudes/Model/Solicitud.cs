namespace apiSolicitudes.Model
{
    public class Solicitud
    {
        public int SolicitudId { get; set; }
        public int ClienteId { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Justificativo { get; set; }
        public string Estado { get; set; }
        public string? DetalleGestion { get; set; }
        public string FechaIngreso { get; set; }
        public string? FechaActualizacion { get; set; }
        public string? FechaGestion { get; set; }
    }
}
