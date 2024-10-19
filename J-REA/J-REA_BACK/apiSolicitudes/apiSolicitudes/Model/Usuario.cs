namespace apiSolicitudes.Model
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public bool EsAdministrador { get; set; }
    }
}
